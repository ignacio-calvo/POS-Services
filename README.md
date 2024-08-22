# POS Services

Backend solution for supporting Point Of Sale (POS) business. 

Written in .NET Core 8 where most microservices implement a layered architecture (API -> Business -> Data) and Repository pattern on an Entity Framework Code First approach using AutoMapper for simplifying Entity and DTO mapping.

Solution has been dockerized and orchestrated using Docker Compose. 
Database supported by a SQL Server container. 

Microservice implemented for Identity management to Authenticate against using .NET Identity Framework. 


## Architecture
![Component Diagram](https://github.com/ignacio-calvo/POS-Services/blob/develop/Documentation/POSitive-Components.png)

*React frontend project for Online Ordering can be found here <https://github.com/ignacio-calvo/POS-Frontend>*

## Getting Started

### Docker considerations

For running solution through docker-compose you need to have Docker installed in your system. 
You will need to create a ".env" file in the root path right next to the docker-compose.yaml 
Setup environment variables needed by the docker-compose.yml in the ".env" file (The ones following the "${Variable}" format).

Content of your .env file may look something like: 

    # Password used to sign the self-signed certificate used for supporting SSL
    ASPNETCOREKestrelCertificatesDefaultPassword=Y0urPassW0rd
    # Password used for the SA (SuperAdmin) user upon creating the SQL Server Database
    MSSQL_SA_PASSWORD=Y0urPassW0rd
    # Passwords used for accessing the SQL Server DBs with the SA user. Should all be the same as MSSQL_SA_PASSWORD but this allows for using different SQL Server instances for each DB if eventually needed. 
    IdentityDBPassword=Y0urPassW0rd    
    OrderDBPassword=Y0urPassW0rd
    CustomerDBPassword=Y0urPassW0rd
    ProductDBPassword=Y0urPassW0rd 
    # JWT Key used for authenticating against services
    IdentityJwtKey=ThisIsMyUltraS3cr3tK3y!
    # HTTP URLs of the APIs needed for APIs talking to each other inside the docker-compose environment when running on Development mode. 
    IdentityApiUrl=http://pos.identity.api:8080
    CustomersApiUrl=http://pos.customers.api:8080

### Running locally (EF Migrations)

For running APIs locally without Docker (and for being able to run Entity Framework Migrations, which end up executing APIs locally) you will need to setup .NET Secrets on each API project to configure Environment variables as needed by each Program.cs. 

Example for POS.Customers.API:  

    {
      "Kestrel:Certificates:Development:Password": "0d4a14bc-6b35-4849-96e4-021858592015",
      "IdentityJwtKey": "ThisIsMyUltraS3cr3tK3y!",
      "IdentityApiUrl": "https://localhost:7010",
      "CustomerDBPassword": "Y0urPassW0rd",
      "ASPNETCOREKestrelCertificatesDefaultPassword": "Y0urPassW0rd"
    }

### SSL 
For accesing APIs from your host environment through SSL you will also need to make sure you have a self-signed certificate for enabling consuming APIs through secure SSL protocol (https). For more insights on generating self-signed dev certs: <https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs>

