# ASP.NET EndPointsApi
 Web API REST using EndPoints approach instead controllers

# Solution structure

## Source

[1. ApplicationCore](#1-applicationCore)

[2. Infrastructure](#2-infrastructure)

[3. API](#3-api)

## 1. ApplicationCore

 This project contains the database and auxiliar entities, the specifications to query in repository, the services with it's logic, the interfaces and some extensions.

## 2. Infrastructure

 This project contains EntityFramework migrations, database context and its configuration, a concrete repository, csv mapper to correct parse the locations.csv header with the Location.cs. 
 The SeedData folder contains the csv file to easily fill the database by reading it and a static class SeedLocationsData that allows to save all locations in memory or in the data base, the latter takes around 3 minutes at first execution.

## 3. API

 This project contains a folder called LocationEndPoint containing a file called List.cs that inherites from BaseAsyncEndpoint that transforms this class file to an endpoint without using a Controller, with that we can have single files per call instead a controller with many calls within.
 List.cs group and use the ListLocationRequest and ListLocationResponse giving us the external structure of the requests and responses. Dto classes have a minimum properties required to be used on requests and responses.
 MappingProfile have the AutoMapper conversions.

## Tests

[1. UnitTests](#1-unitTests)

[2. IntegrationTests](#2-integrationTests)

[3. FunctionalTests](#3-functionalTests)

## 1. UnitTests

 This project contains the specifications and services tests, also a builder to help us to create lists of Locations easly.

## 2. IntegrationTests

 This project contains the services tests.

## 3. FunctionalTests

 This project contains the endpoint tests, also have the CustomWebApplicationFactory to provide us the testing environment for all our tests.

# Prerequisites

 Requires to have installed the .Net5 runtime.
 https://dotnet.microsoft.com/en-us/download/dotnet/5.0
 
 Requires LocalDB which can be installed with SQL Server Express 2019
 https://www.microsoft.com/en-us/Download/details.aspx?id=101064

# Insights
 I decided not to use the common aproach of MVC controllers as we want the best maintainability for this project, to do this I chose https://github.com/ardalis/ApiEndpoints that offer me an easly implementation of it.
 Also I'm using the specification pattern that give us a powerful tool to manage repository calls without having alot of concrete repositories and repeat our where clauses in many methods. 

 About how to calculate the locations we are looking for, I used the method given for the OrderBy clause and before use it, I used a MaxDistanceSquare class, which gives me a square coordinates containing the requested location. 
 With this calculation I can put the Where clause from the specification that is translated into an SQL where clause, at first I tried to use CalculateDistance inside the specification where but it fails because is not translatable to SQL.
