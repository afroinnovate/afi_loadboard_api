# afi_loadboad_api

Welcome to the AFI Transportation Management Service API. This project is built using .NET Core with C# 8, leverages a microservices architecture, and utilizes PostgreSQL as its primary database, interfaced with the Dapper ORM.

## Table of Contents

- [Dependencies](#dependencies)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Contributing](#contributing)
- [License](#license)

## Dependencies

The Auth.API was created off of .NET 7 and has the following core dependencies:

- `.NET Core SDK 3.1`: The software development kit for .NET Core.
- `.NET 7`: 
- `Entity framework`: A micro ORM for .NET. It simplifies data access and is highly performant.
- `Npgsql`: .NET data provider for PostgreSQL.
- ... _(Include other specific packages or libraries as necessary)_

The Auth.Min.API was created off of .NET 8 with minimal API and has the following core dependencies:

- `.NET Core SDK 3.1`: The software development kit for .NET Core.
- `.NET 8`: 
- `Entity framework`: A micro ORM for .NET. It simplifies data access and is highly performant.
- `Npgsql`: .NET data provider for PostgreSQL.
- ... _(Include other specific packages or libraries as necessary)_

## How to run the app using docker.
### Dependencies.
    Create .env on the root folder and add  following values to it. filling in appropriate values.
    DB_PASSWORD="your-server-password"
    DB_USERNAME="postgres"
    AUTH_DB_NAME=User
    FRIEGHT_DB_NAME=LoadBoard
    DB_HOST=host.docker.internal - This is required when you're running on docker

1. Make sure if you're just starting run the migration - ```dotnet ef migrations add pgInitial -o Infrastructure/Data/Migrations``` - for Freight API

2. 1. Make sure if you're just starting run the migration - ```dotnet ef migrations add pgInitial``` - for Auth API

3. try to update your local db with an outstanding migration for each project ```dotnet ef database update```

**Run the APIs**

1. on the root folder
2. To build the image ```docker compose build``` optional
3. run run the api ```docker-compose -f dev-docker-compose.yaml up```
Access the apis 
- ```localhost:8080/swagger/index.html``` for Authapi
- ```localhost:7070/swagger/index.html``` for Load and Carrier API


### Run it locally without docker
- Make sure you create appsettings.Development.json file in the service you're trying to run, example in Auth.Min.API I can fill in the contents like.
```{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=Loadboard;Username=postgres;Password=afroinnovate2023;"
    },
    "ApiSettings": {
        "JwtOptions": {
            "Issuer": "afroinnovate.com",
            "Audience": "app.loadboard.afroinnovate.com",
            "SecretKey": "default_secret_key",
            "ValidFor": "1:00:00"
        }
    }
}
```
## Getting Started to contribute

1. **Clone the Repository**
    ```bash
    git clone https://github.com/<your-github-username>/afi_loadboad_api.git
    ```

2. **Navigate to the Project Directory**
    ```bash
    cd afi_loadboad_api
    ```

## Running the apis
Make sure you have `appsettings.Development.json` file on the root directory and use below example as your local setup.
```bash
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=loadboard;Username=;Password=;"
    },
    "ApiSettings": {
        "JwtOptions": {
            "Issuer": "afroinnovate.com",
            "Audience": "app.loadboard.afroinnovate.com",
            "SecretKey": "secret-key-to-be-changed",
            "ValidFor": "1:00:00"
        }
    }
}
```

NB: to be able to run the api locally, please make sure you uncomment the below line and comment out the line that comes next, unless you want to run using a docker image.
Go to `Infrastructure > DataExtension.cs
```
// var connString = Environment.GetEnvironmentVariable("DefaultConnection"); // to retrieve connection from docker container environment variable   
        var connString = configuration.GetConnectionString("DefaultConnection"); // to retrieve connection from configuration file like appsettings.json
        Console.WriteLine($"DefaultConnection: {connString}");
```
1. Run ```dotnet restore```
2. Run ```dotnet watch run``` To keep loading the apis on any changes.


## Creating Migration for the first time
1. Create migration: 
    ```dotnet ef migrations add <MigrationName> --output-dir ./Infrastructure/Data/Migrations```
2. If you're running the app locally go to Infrastructure folder -> DataExtensions and then comment out this lines and comment the second line.
- comment out ```var connString = Environment.GetEnvironmentVariable("DefaultConnection");```
- uncomment For non docker testing
    ```
       var connString = configuration.GetConnectionString("DefaultConnection"); // to retrieve connection from configuration file like appsettings.json
        Console.WriteLine($"DefaultConnection: {connString}"); 
    ```
2. Update the database
    ```dotnet run``` on freight.api directory



## License

© 2023 AfroInnovate LoadBoard. All rights reserved.
