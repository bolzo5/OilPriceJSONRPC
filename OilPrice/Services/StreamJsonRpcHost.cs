using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Connections;
using StreamJsonRpc;
using System.Text;
using OilPricesServer.Internals;
using OilPricesServer.Data;
using OilPricesServer.Models;
using OilPricesContract;

namespace OilPricesServer.Services
{
    internal class StreamJsonRpcHost : BackgroundService
    {
        private readonly IConnectionListenerFactory _connectionListenerFactory;
        private readonly ConcurrentDictionary<string, (ConnectionContext Context, Task ExecutionTask)> _connections = new ConcurrentDictionary<string, (ConnectionContext, Task)>();
        private readonly ILogger<StreamJsonRpcHost> _logger;
        private readonly IOilPricesRepository _oilPricesRepository;
        public List<OilPriceAtDate>? _oilPrices { get; set; }

        private IConnectionListener _connectionListener;

        public StreamJsonRpcHost(IConnectionListenerFactory connectionListenerFactory, ILogger<StreamJsonRpcHost> logger, IOilPricesRepository oilPricesRepository)
        {
            _connectionListenerFactory = connectionListenerFactory;
            _logger = logger;
            _oilPricesRepository = oilPricesRepository;
            _ = GetBrentDailyValueAsync();
        }

        private async Task GetBrentDailyValueAsync()
        {
            _oilPrices = await _oilPricesRepository.GetBrentDailyValuesAsync("https://datahub.io/core/oil-prices/r/brent-daily.json");
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connectionListener = await _connectionListenerFactory.BindAsync(new IPEndPoint(IPAddress.Loopback, 6000), stoppingToken);

            while (true)
            {
                ConnectionContext connectionContext = await _connectionListener.AcceptAsync(stoppingToken);

                // AcceptAsync will return null upon disposing the listener
                if (connectionContext == null)
                {
                    break;
                }

                _connections[connectionContext.ConnectionId] = (connectionContext, AcceptAsync(connectionContext));
            }

            List<Task> connectionsExecutionTasks = new List<Task>(_connections.Count);

            foreach (var connection in _connections)
            {
                connectionsExecutionTasks.Add(connection.Value.ExecutionTask);
                connection.Value.Context.Abort();
            }

            await Task.WhenAll(connectionsExecutionTasks);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connectionListener.DisposeAsync();
        }

        private async Task AcceptAsync(ConnectionContext connectionContext)
        {
            try
            {
                await Task.Yield();

                _logger.LogInformation("Connection {ConnectionId} connected", connectionContext.ConnectionId);

                IJsonRpcMessageFormatter jsonRpcMessageFormatter = new JsonMessageFormatter(Encoding.UTF8);
                IJsonRpcMessageHandler jsonRpcMessageHandler = new LengthHeaderMessageHandler(connectionContext.Transport, jsonRpcMessageFormatter);

                using (var jsonRpc = new JsonRpc(jsonRpcMessageHandler, new OilPricesService(_oilPrices)))
                {
                    jsonRpc.StartListening();
                    
                    await jsonRpc.Completion;
                }

                await connectionContext.ConnectionClosed.WaitAsync();
            }
            catch (ConnectionResetException)
            { }
            catch (ConnectionAbortedException)
            { }
            catch (Exception e)
            {
                _logger.LogError(e, "Connection {ConnectionId} threw an exception", connectionContext.ConnectionId);
            }
            finally
            {
                await connectionContext.DisposeAsync();

                _connections.TryRemove(connectionContext.ConnectionId, out _);

                _logger.LogInformation("Connection {ConnectionId} disconnected", connectionContext.ConnectionId);
            }
        }
    }
}
