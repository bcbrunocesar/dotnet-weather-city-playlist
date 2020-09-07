FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /application
COPY . .
RUN dotnet restore Ingaia.Challenge.WebApi.csproj
RUN dotnet publish Ingaia.Challenge.WebApi.csproj --no-restore --configuration Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://*:$PORT
ENTRYPOINT ["dotnet", "Ingaia.Challenge.WebApi.dll"]
