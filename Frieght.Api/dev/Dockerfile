#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7070
EXPOSE 4431

# Use the ASP.NET Core SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# .

WORKDIR /src
COPY ["Frieght.Api.csproj", "."]
RUN dotnet restore "Frieght.Api.csproj"
COPY . .
RUN dotnet build "Frieght.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frieght.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET Core SDK as the final image
# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final
# Use the ASP.NET Core runtime as the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the Migrations directory
# COPY Infrastructure/Data/Migrations /app/Migrations

# Copy the .csproj file
COPY . /app/

# If you have an appsettings.Production.json without sensitive data
COPY ["appsettings.json", "/app/"]

# Set the ASP.NET Core environment to Development
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy the entry point script
COPY docker-entrypoint.sh .

# Grant execute permissions to the entry point script
RUN chmod +x docker-entrypoint.sh

# Set the entry point to the script
ENTRYPOINT ["./docker-entrypoint.sh"]