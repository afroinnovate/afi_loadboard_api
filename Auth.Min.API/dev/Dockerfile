# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Use the ASP.NET Core SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Auth.Min.API.csproj", "."]
RUN dotnet restore "Auth.Min.API.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "Auth.Min.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.Min.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET Core SDK as the final image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the Migrations directory
COPY Migrations /app/Migrations

# Copy the .csproj file
COPY . /app/

# Set the ASP.NET Core environment to Development
ENV ASPNETCORE_ENVIRONMENT=Development

# Install the Entity Framework Core tools
RUN dotnet tool install --global dotnet-ef --version 6.0.0-* --add-source https://api.nuget.org/v3/index.json

# Add the tools to the PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy the entry point script
COPY docker-entrypoint.sh .

# Set the entry point to the script
ENTRYPOINT ["./docker-entrypoint.sh"]