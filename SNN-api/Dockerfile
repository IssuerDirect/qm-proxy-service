FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /source

COPY . .
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

ENV ASPNETCORE_URLS=http://*:5000

WORKDIR /app
ENTRYPOINT ["dotnet", "SNN-api.dll"]
