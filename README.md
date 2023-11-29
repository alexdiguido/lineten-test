# lineten-test

This repository contains a C# .NET Web API project that demonstrates CRUD (Create, Read, Update, Delete) operations on Order, Product, and Customer entities. The application is containerized with Docker Compose for easy deployment.

## Features
Order Management: Create, retrieve, update, and delete orders.
Product Management: Create, retrieve, update, and delete products.
Customer Management: Create, retrieve, update, and delete customers.
## Prerequisites
Before you begin, ensure you have the following installed:

.NET 6 SDK
Docker

## Dependencies 
- [Entity Framework 6](https://github.com/dotnet/ef6)
- [Ardalis.GuardClauses](https://github.com/ardalis/GuardClauses) : A library for argument validation.
- [Ardalis.Specification](https://github.com/ardalis/Specification) : for db query creation. 
- [MediatR](https://github.com/jbogard/MediatR): A simple mediator pattern implementation.

## Run application
`docker-compose up --build`

## Run Unit test

`dotnet test LineTenTest.Api.Tests`
`dotnet test LineTenTest.Domain.Tests`

## Run integration tests
`dotnet test LineTenTest.Api.IntegrationTests  --environment "IntegrationTest"`

## Postman collection
you can find in the [root folder of the repo](https://github.com/alexdiguido/lineten-test/blob/master/LineTenTest.Api.postman_collection.json)
