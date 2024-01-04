#!/bin/bash

# Apply migrations
dotnet ef database update

# Start the application
exec dotnet Auth.Min.API.dll