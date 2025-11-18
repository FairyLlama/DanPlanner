FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiér hele solutionen (alle projekter)
COPY . .

# Restore på solution-niveau
RUN dotnet restore

# Byg Gateway
RUN dotnet publish -c Release -o /app

# 2. Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Gateway.dll"]