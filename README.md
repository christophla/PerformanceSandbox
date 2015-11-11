# PerformanceSandbox
A performance comparison of various middle-ware and data storage configurations.

MSSQL, MongoDB, Azure DocumentDB, Azure SQL, WebAPI IIS, WebAPI Self-Host (owin), Node.JS

## Dependencies
This project requires that Node.JS tools for Visual Studio tools is installed.

https://www.visualstudio.com/en-us/features/node-js-vs.aspx

## Installation (MSSQL)

The project leverages Entity Framework code migrations to build the MSSQL database. 

### From the Visual Studio Package Manager Console

    Select the PerformanceSandbox.MSSQL.WebAPI.DataAccess as default project
    Run PM>Update-Database

## Installation (MongoDB)

The project leverages built-in controller to build the database.

### From Visual Studio 

    Start the PerformanceSandbox.MongoDB.WebAPI.IIS project
    Visit the url (POST) http://localhost/PerformanceSandbox.MongoDB.WebAPI.IIS/products/load

## Installation (Azure DocumentDB)

The project leverages built-in controller to build the database.

### From Visual Studio 

    Start the PerformanceSandbox.DocumentDB.WebAPI.IIS project
    Visit the url (POST) http://localhost/PerformanceSandbox.DocumentDB.WebAPI.IIS/products/load

## JMeter
A JMeter configuration file can be found under /jmeter

## Whitepaper
The load results can be found under /docs.



