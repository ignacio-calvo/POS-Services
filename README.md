# POS Services

Backend solution for supporting POS concepts. 

Written in .NET Core 8 implementing a layered microservice architecture (API -> Business -> Data). 

Implemented Repository pattern on an Entity Framework Code First approach on SQL Server using AutoMapper for simplifying Entity and DTO mapping.
Also contains Microservice for Identity management to Authenticate against. 

Solution has been dockerized and orchestrated using Docker Compose. 


## Getting Started

You will need to create a ".env" file in the root path containing the variables and values needed by the docker-compose.yaml (all the ones formatted like ${Variable})
