# POS Services

Backend solution for supporting POS concepts. 

Written in .NET Core 8 implementing a layered microservice architecture (API -> Business -> Data). 

Implemented Repository pattern on an Entity Framework Code First approach on SQL Server using AutoMapper for simplifying Entity and DTO mapping.
Also contains Microservice for Identity management to Authenticate against. 

Solution has been dockerized and orchestrated using Docker Compose. 


## Getting Started

You need to have Docker installed in your system. 
You will need to create a ".env" file in the root path containing the variables and values needed by the docker-compose.yaml (The ones following the "${Variable}" format).

Content of your .env file may look something like: 

    ASPNETCOREKestrelCertificatesDefaultPassword=Y0urPassW0rd
    MSSQL_SA_PASSWORD=Y0urPassW0rd
    IdentityDBPassword=Y0urPassW0rd
    IdentityJwtKey=ThisIsMyUltraS3cr3tK3y!
    OrderDBPassword=Y0urPassW0rd
    CustomerDBPassword=Y0urPassW0rd
    ProductDBPassword=Y0urPassW0rd

You will also need to make sure you have a self-signed certificate for enabling consuming APIs through secure SSL protocol (https). For more insights on generating self-signed dev certs: <https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs>

## Architecture
![Component Diagram](https://github.com/ignacio-calvo/POS-Services/blob/develop/Documentation/POSitive-Components.jpg)

*React frontend project for Online Ordering can be found here <https://github.com/ignacio-calvo/POS-Frontend>*
