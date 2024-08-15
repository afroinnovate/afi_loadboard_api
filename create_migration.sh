#!/bin/bash

# Create Migration given migration dir as input and migration name
# Usage: ./create_migration.sh <migration_dir> <migration_name>
# Example: ./create_migration.sh migrations create_users_table

# Check if migration dir is provided
if [ -z "$1" ]; then
  echo "Migration dir is required"
  exit 1
fi

# Check if migration name is provided
if [ -z "$2" ]; then
  echo "Migration name is required"
  exit 1
fi

# Create migration file
timestamp=$(date +"%Y%m%d%H%M%S")
migration_file="$1/$timestamp_$2.sql"

# use entity framework to create migration
dotnet ef migrations add $2 -o $1