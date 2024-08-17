#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["POS.Orders.API/POS.Orders.API.csproj", "POS.Orders.API/"]
COPY ["POS.CommonBase/POS.CommonBase.csproj", "POS.CommonBase/"]
COPY ["POS.Orders.Business/POS.Orders.Business.csproj", "POS.Orders.Business/"]
COPY ["POS.Orders.Data/POS.Orders.Data.csproj", "POS.Orders.Data/"]
RUN dotnet restore "./POS.Orders.API/POS.Orders.API.csproj"
COPY . .
WORKDIR "/src/POS.Orders.API"
RUN dotnet build "./POS.Orders.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./POS.Orders.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POS.Orders.API.dll"]