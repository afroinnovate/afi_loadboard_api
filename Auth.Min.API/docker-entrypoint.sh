#!/bin/bash

# Apply migrations
dotnet ef database update 1

# Start the application
exec dotnet Auth.Min.API.dll