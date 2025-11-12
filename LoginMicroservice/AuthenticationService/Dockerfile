# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiér hele solutionen (alle projekter)
COPY . .

# Restore på solution-niveau
RUN dotnet restore BlazorApp3semesterEksamensProjektMappe.sln

# Byg AuthenticationService
WORKDIR /src/LoginMicroservice/AuthenticationService
RUN dotnet publish -c Release -o /app

# 2. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AuthenticationService.dll"]