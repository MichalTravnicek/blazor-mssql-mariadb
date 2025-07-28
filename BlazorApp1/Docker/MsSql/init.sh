#wait for the SQL Server to come up

sleep 15s

echo "running set up script"
#run the setup script to create the DB and the schema in the DB
#latest version of docker image MSSSQL2019 has /opt/msql-tools18/ instead /opt/mssql-tools/
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P ${SA_PASSWORD} -C -d master -i initdb.sql
