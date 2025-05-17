Step 1: Pull the image
docker pull mcr.microsoft.com/azure-sql-edge

Step 2: Run a SQL Server Edge container
docker run -e 'ACCEPT_EULA=1' \
 -e 'SA_PASSWORD=SqlDev@2025!' \
 -p 1433:1433 \
 --name sqlserver \
 --restart unless-stopped \
 -d mcr.microsoft.com/azure-sql-edge

Step 3: Verify it’s running
docker ps

Step 4: Connect to it

1.  From your .NET app:
    "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=UserDb;User Id=sa;Password=SqlDev@2025!;"
    }
2.  SQL CLI from container:
    docker exec -it sqlserver /bin/bash
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P SqlDev@2025!
3.  GUI tools:
    Server: localhost
    Port: 1433
    Username: sa
    Password: SqlDev@2025!
