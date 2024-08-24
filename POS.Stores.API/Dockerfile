#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["POS.Stores.API/POS.Stores.API.csproj", "POS.Stores.API/"]
COPY ["POS.CommonBase/POS.CommonBase.csproj", "POS.CommonBase/"]
COPY ["POS.Stores.Business/POS.Stores.Business.csproj", "POS.Stores.Business/"]
COPY ["POS.Stores.Data/POS.Stores.Data.csproj", "POS.Stores.Data/"]
RUN dotnet restore "./POS.Stores.API/POS.Stores.API.csproj"
COPY . .
WORKDIR "/src/POS.Stores.API"
RUN dotnet build "./POS.Stores.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./POS.Stores.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POS.Stores.API.dll"]