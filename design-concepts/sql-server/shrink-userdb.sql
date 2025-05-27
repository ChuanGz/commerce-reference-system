DECLARE @DbName NVARCHAR(255);
DECLARE @SQL NVARCHAR(MAX);

DECLARE db_cursor CURSOR FOR
SELECT name
FROM sys.databases
WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');

OPEN db_cursor;
FETCH NEXT FROM db_cursor INTO @DbName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = '
    USE [' + @DbName + '];
    DECLARE @LogFile NVARCHAR(255);
    SELECT TOP 1 @LogFile = name FROM sys.database_files WHERE type_desc = ''LOG'';
    ALTER DATABASE [' + @DbName + '] SET RECOVERY SIMPLE;
    DBCC SHRINKFILE (@LogFile, 50);';
    
    EXEC sp_executesql @SQL;
    FETCH NEXT FROM db_cursor INTO @DbName;
END

CLOSE db_cursor;
DEALLOCATE db_cursor;