# OilPrices

A .Net 6 Web API that retrieves oil prices data from https://datahub.io/ and returns an interval from the provided date range.
## Getting Started & Prerequisites

Either clone using git or download the project.  
Set up Docker to run .NET core applications  
[DOCKER](https://docs.docker.com/)


### Installing & Running
The project can be run as it is form localhost or from the provided dockerimage file.

Navigate to the project folder, the one with the .sln and dockerimage files.  
Run the following Docker CLI command to build the application image.  
```  
	docker build -t oilpricesserver .
```  
Then start an instance of the image, running on port 6001
```  
	docker run --name oilpricesserver --rm -it -p 6001:6001 oilpricesserver
```  
	
Finally you can try the endpoints at:
``` 
	http://localhost:6001/
``` 

Example request payload
``` 
{
"id": 1,
"jsonrpc": "2.0",
"method": "GetOilPriceTrend",
"params": {
"startDateISO8601": "2020-01-01",
"endDateISO8601": "2020-01-05"
}
} 
``` 
Example response payload
``` 
{
"jsonrpc": "2.0",
"id": 1,
"result": {
"prices": [
{
"dateISO8601": "2020-01-01",
"price": 12.3
},
{
"dateISO8601": "2020-01-02",
"price": 13.4
},
{
"dateISO8601": "2020-01-03",
"price": 14.5
},
{
"dateISO8601": "2020-01-04",
"price": 16.7
},
{
"dateISO8601": "2020-01-05",
"price": 18.9
}
 ]}
}
``` 

## Running the tests
Included in the solution there is a test project for unit testing.  

### Unit Tests: 
  Testing a correct response and a failure mode
 * ShouldReturnValidOilPricesForGivenDateAsync  
 * ShouldReturnEmptyForInvalidDateAsync  


## Authors  

Andrea Bolzoni  
