# AFI_TMS_API

Welcome to the AFI Transportation Management Service API. This project is built using .NET Core with C# 8, leverages a microservices architecture, and utilizes PostgreSQL as its primary database, interfaced with the Dapper ORM.

## Table of Contents

- [Dependencies](#dependencies)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Contributing](#contributing)
- [License](#license)

## Dependencies

This API has the following core dependencies:

- `.NET Core SDK`: The software development kit for .NET Core.
- `Dapper`: A micro ORM for .NET. It simplifies data access and is highly performant.
- `Npgsql`: .NET data provider for PostgreSQL.
- ... _(Include other specific packages or libraries as necessary)_

## Getting Started

1. **Clone the Repository**
    ```bash
    git clone https://github.com/<your-github-username>/afi_tms_api.git
    ```

2. **Navigate to the Project Directory**
    ```bash
    cd afi_tms_api
    ```

3. **Restore the .NET Packages**
    ```bash
    dotnet restore
    ```

4. **Run the API**
    ```bash
    dotnet run
    ```

The API should now be running at `http://localhost:5000`.

## Database Setup

This API uses PostgreSQL. To set it up:

1. **Install PostgreSQL** if not already installed.
   
2. **Create a Database** named 'afi_tms'.
   
3. **Update Connection String** in `appsettings.json` to point to your local PostgreSQL instance.

## Contributing

We value and appreciate your contributions. To contribute:

1. **Fork** the repository.
   
2. **Clone** your forked repository:
    ```bash
    git clone https://github.com/<your-github-username>/afi_tms_api.git
    ```

3. **Create a New Branch** off the `develop` branch:
    ```bash
    git checkout -b feature/my-new-feature develop
    ```

4. **Make Your Changes** and commit them.
   
5. **Push** the changes to your forked repository on GitHub.
   
6. **Create a Pull Request** targeting the `develop` branch of the original repository.

Please ensure your code adheres to the project's coding standards.

## License

[alright reserved by afroinnovate@2023](alright reserved by afroinnovate@2023)
