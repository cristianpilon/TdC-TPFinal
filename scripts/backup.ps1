# Parámetros
$serverInstance = "EN1810612\SQLEXPRESS"
$databaseName = "NombreDeLaBaseDeDatos"
$backupPath = "C:\Cristian\Backup"

# Genera la cadena de conexión
$connectionString = "Server=$serverInstance;Database=$databaseName;Integrated Security=True;"

# Nombre del archivo de backup
$backupFileName = "$databaseName-$(Get-Date -Format 'yyyyMMdd-HHmmss').bak"
$backupFilePath = Join-Path $backupPath $backupFileName

# Query para realizar el backup
$query = "BACKUP DATABASE [$databaseName] TO DISK = '$backupFilePath' WITH FORMAT, INIT, NAME = '$databaseName-Full Database Backup'"

# Ejecuta la consulta
Invoke-Sqlcmd -ServerInstance $serverInstance -Database $databaseName -Query $query

Write-Host "Backup completo de la base de datos $databaseName creado en $backupFilePath"
