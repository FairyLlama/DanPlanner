FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/server

FROM base AS final
WORKDIR /app
COPY --from=build /app/server .
ENTRYPOINT ["dotnet", "Danplanner.dll"]