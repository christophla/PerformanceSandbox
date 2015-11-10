# PerformanceSandbox
A performance comparison of various middle-ware and data storage configurations.

## Installation (MSSQL)

The project leverages Entity Framework code migrations to build the MSSQL database. 

### From the Visual Studio Package Manager Console

    Select the PerformanceSandbox.MSSQL.WebAPI.DataAccess as default project
    Run PM>Update-Database

## Installation (MongoDB)

The project leverages built-in controller to build the database.

### From Visual Studio 

    Start the PerformanceSandbox.MongoDB.WebAPI.IIS project
    Visit the url http://localhost/PerformanceSandbox.MongoDB.WebAPI.IIS/products/load

## JMeter
A JMeter configuration file can be found under /jmeter

## Whitepaper
The load results can be found under /docs.



