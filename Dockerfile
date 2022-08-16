FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln ./
COPY SuggestionApplication.Api/log.txt /app
COPY SuggestionApplication.Api/*.csproj /app/SuggestionApplication.Api/
COPY SuggestionApplication.Application/*.csproj /app/SuggestionApplication.Application/
COPY SuggestionApplication.Domain/*.csproj /app/SuggestionApplication.Domain/
COPY SuggestionApplication.Infrastructure/*.csproj /app/SuggestionApplication.Infrastructure/
COPY SuggestionApplication.Persistence/*.csproj /app/SuggestionApplication.Persistence/

RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

RUN apt-get update
RUN	apt-get -y install procps && rm -rf /var/lib/apt/lists/*
RUN apt-get update
RUN apt-get -y install curl
RUN apt-get update
RUN apt-get -y install net-tools

COPY --from=build-env /app/out .

EXPOSE 7020

ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:7020

ENTRYPOINT ["dotnet", "SuggestionApplication.Api.dll"]
