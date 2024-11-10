#!/bin/bash

# Define the database file
DB_FILE=${1:-"MagikarpMayhemContext.db"}

# Run the SQL scripts# Run the SQL scripts
for sql_file in $(ls SQLScaffold/*.sql | sort); do
  sqlite3 $DB_FILE < "$sql_file"
  echo "[$sql_file] Done"
done

echo "Database state reset"