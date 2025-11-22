FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app


# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

# Restore på solution-niveau
RUN dotnet restore

# Byg AuthenticationService
RUN dotnet publish -c Release -o /app

FROM base AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AuthenticationService.dll"]