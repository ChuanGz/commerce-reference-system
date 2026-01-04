FROM mcr.microsoft.com/azure-sql-edge

# Switch to root to change file permissions
USER root

# Copy shrink script & watcher
COPY shrink-userdb.sql /shrink-userdb.sql
COPY watch-and-shrink.sh /watch-and-shrink.sh
RUN chmod +x /watch-and-shrink.sh

# Revert to default SQL Server user
USER 10001

# Start SQL Server + background watcher
CMD ["sh", "-c", "/opt/mssql/bin/sqlservr & /watch-and-shrink.sh"]
