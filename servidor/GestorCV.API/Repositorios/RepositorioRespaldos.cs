using GestorCV.API.Infraestructura;
using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioRespaldos : IRepositorio
    {
        public List<Respaldo> ObtenerTodos();

        public void Agregar(string tipoRespaldo);

        public void Modificar(DateTime fecha, string tipoRespaldo);

        public void Eliminar(DateTime fecha, string tipoRespaldo);
    }

    /// <summary>
    /// Repositorio de empleos.
    /// </summary>
    public sealed class RepositorioRespaldos : RepositorioBase, IRepositorioRespaldos
    {
        public const string TipoRespaldoCompleto = "Completo";
        public const string Diferencial = "Diferencial";

        private const string FormatoFecha = "yyyyMMddHHmmss";
        /// <summary>
        /// Obtiene los respaldos realizados de la base de datos guardados en el disco.
        /// </summary>
        /// <returns>Respaldos guardados en el disco rígido.</returns>
        public List<Respaldo> ObtenerTodos()
        {
            // Obtengo los archivos del directorio de respaldos
            var directorio = new DirectoryInfo(AppConfiguration.RutaRespaldos);
            var archivos = directorio.GetFiles("*.bak");

            var resultado = archivos.Select(archivo => 
            {
                // El formato esperado de los backups es '{NombreBaseDeDatos}_yyyyMMddHHmmss_{TipoRespaldo}.bak'
                var nombreArchivoSplit = archivo.Name.Split(".")[0].Split('_');
                var fecha = DateTime.ParseExact(nombreArchivoSplit[1], FormatoFecha, CultureInfo.InvariantCulture);
                var tipoRespaldo = nombreArchivoSplit[2] == "Full" ? "Completo" : "Diferencial";

                return new Respaldo(fecha, tipoRespaldo);
            }).ToList();

            return resultado;
        }

        /// <summary>
        /// Crea un respaldo de la base de datos.
        /// </summary>
        /// <param name="tipoRespaldo">Tipo de respaldo a realizar (Completo/Diferencial).</param>
        public void Agregar(string tipoRespaldo)
        {
            ValidarExistenciaStoreCrearRespaldo();

            var instanciaServidor = _contexto.Database.GetDbConnection().DataSource;
            var nombreBaseDeDatos = _contexto.Database.GetDbConnection().Database;
            var rutaRespaldos = AppConfiguration.RutaRespaldos;

            tipoRespaldo = ConvertirTipoRespaldoValorBaseDatos(tipoRespaldo);

            _contexto.ExecuteRawSql("EXEC [dbo].[BackupDatabase] @ServerInstance, @DatabaseName, @BackupPath, @BackupType", instanciaServidor, nombreBaseDeDatos, rutaRespaldos, tipoRespaldo);
        }

        /// <summary>
        /// Restaura un respaldo de la base de datos.
        /// </summary>
        /// <param name="fecha">Fecha del respaldo a restaurar.</param>
        /// <param name="tipoRespaldo">Tipo de respaldo a restaurar (Completo/Diferencial).</param>
        public void Modificar(DateTime fecha, string tipoRespaldo)
        {
            var nombreBaseDeDatos = _contexto.Database.GetDbConnection().Database;
        }

        /// <summary>
        /// Elimina un respaldo guardado en el disco
        /// </summary>
        /// <param name="fecha">Fecha del respaldo a eliminar.</param>
        /// <param name="tipoRespaldo">Tipo de respaldo a eliminar (Completo/Diferencial).</param>
        public void Eliminar(DateTime fecha, string tipoRespaldo)
        {
            ValidarExistenciaStoreEliminarRespaldo();
            
            var nombreBaseDeDatos = _contexto.Database.GetDbConnection().Database;
            var rutaRespaldos = AppConfiguration.RutaRespaldos;

            tipoRespaldo = ConvertirTipoRespaldoValorBaseDatos(tipoRespaldo);

            _contexto.ExecuteRawSql("EXEC [dbo].[DeleteBackup] @DatabaseName, @BackupDate, @BackupPath, @BackupType", nombreBaseDeDatos, fecha, rutaRespaldos, tipoRespaldo);
        }

        /// <summary>
        /// Método para crear store procedure de creación de respaldo si no existe
        /// </summary>
        private void ValidarExistenciaStoreCrearRespaldo()
        {
            _contexto.ExecuteRawSql(
            @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.BackupDatabase') AND type IN (N'P', N'PC'))
                BEGIN
                    EXEC('
                    CREATE PROCEDURE dbo.BackupDatabase
                        @ServerInstance NVARCHAR(255),
                        @DatabaseName NVARCHAR(255),
                        @BackupPath NVARCHAR(255),
                        @BackupType NVARCHAR(50) = ''Full'' -- Parámetro para especificar el tipo de respaldo (Full o Differential)
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        DECLARE @ConnectionString NVARCHAR(1000);
                        DECLARE @BackupFileName NVARCHAR(255);
                        DECLARE @BackupFilePath NVARCHAR(255);
                        DECLARE @Query NVARCHAR(MAX);

                        -- Genera la cadena de conexión
                        SET @ConnectionString = ''Server='' + @ServerInstance + '';Database='' + @DatabaseName + '';Integrated Security=True;'';

                        -- Nombre del archivo de backup
                        DECLARE @BackupTypeSuffix NVARCHAR(10) = CASE WHEN UPPER(@BackupType) = ''FULL'' THEN ''Full'' ELSE ''Diff'' END;
                        SET @BackupFileName = @DatabaseName + ''_'' + CONVERT(NVARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(NVARCHAR(20), GETDATE(), 108), '':'', '''') + ''_'' + @BackupTypeSuffix + ''.bak'';
                        SET @BackupFilePath = @BackupPath + ''\'' + @BackupFileName;

                        -- Verifica si el backup ya existe
                        IF EXISTS (SELECT 1 FROM msdb.dbo.backupset WHERE database_name = @DatabaseName AND type = ''D'' AND backup_set_id = (SELECT MAX(backup_set_id) FROM msdb.dbo.backupset WHERE database_name = @DatabaseName))
                        BEGIN
                            PRINT ''El backup diferencial ya existe. No se realizará una copia de seguridad adicional.'';
                        END
                        ELSE IF EXISTS (SELECT 1 FROM msdb.dbo.backupset WHERE database_name = @DatabaseName AND type = ''D'' AND @BackupType = ''Diff'')
                        BEGIN
                            PRINT ''No se puede realizar un backup diferencial porque no hay un backup completo previo.'';
                        END
                        ELSE
                        BEGIN
                            -- Query para realizar el backup
                            SET @Query = ''BACKUP DATABASE ['' + @DatabaseName + ''] TO DISK = '''''' + @BackupFilePath + '''''' WITH FORMAT, INIT, NAME = '''''' + @DatabaseName + ''-'' + @BackupTypeSuffix + '' Database Backup'''''';

                            -- Ejecuta la consulta
                            EXEC sp_executesql @Query;

                            PRINT ''Backup '' + @BackupType + '' de la base de datos '' + @DatabaseName + '' creado en '' + @BackupFilePath;
                        END;
                    END;
                    ');
                END;
            ");
        }

        /// <summary>
        /// Método para crear store procedure de creación de respaldo si no existe
        /// </summary>
        private void ValidarExistenciaStoreEliminarRespaldo()
        {
            _contexto.ExecuteRawSql(
            @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.DeleteBackup') AND type IN (N'P', N'PC'))
                BEGIN
                    EXEC('
                    CREATE PROCEDURE dbo.DeleteBackup
                        @DatabaseName NVARCHAR(255),
                        @BackupDate DATETIME,
                        @BackupPath NVARCHAR(255),
                        @BackupType NVARCHAR(50) = ''Full'' -- Parámetro para especificar el tipo de respaldo (Full o Differential)
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        DECLARE @BackupFilePath NVARCHAR(255);

        
                        -- Genera la ruta del archivo de backup
                        SET @BackupFilePath = @BackupPath + ''\'' + @DatabaseName + ''_'' + CONVERT(NVARCHAR(14), @BackupDate, 112) + REPLACE(CONVERT(NVARCHAR(12), @BackupDate, 108), '':'', '''') + ''_'' + @BackupType + ''.bak'';

        
                        -- Verifica si el archivo existe antes de intentar eliminarlo
                        DECLARE @FileExists INT
                        EXEC master.dbo.xp_fileexist @BackupFilePath, @FileExists OUTPUT
                        IF @FileExists = 1
                        BEGIN
                            -- Elimina el archivo físico
			                EXEC sp_dropdevice @BackupFilePath, ''DELFILE'';
                            PRINT ''Archivo físico del respaldo eliminado: '' + @BackupFilePath;
                        END
                        ELSE
                        BEGIN
                            PRINT ''El archivo físico del respaldo no existe: '' + @BackupFilePath;
                        END

                        PRINT ''Registros del respaldo eliminados para la base de datos '' + @DatabaseName + '' ('' + @BackupType + '').'';
                    END;
                    ');
                END;
            ");
        }

        /// <summary>
        /// Convierte el valor de tipo de respaldo de la interfaz al valor de la base de datos.
        /// </summary>
        /// <param name="tipoRespaldo">Tipo de respaldo mostrado en la interfaz.</param>
        /// <returns>Tipo de respaldo como se espera en la base de datos.</returns>
        /// <exception cref="ArgumentException">Si el tipo de respaldo enviado no corresponde a los valores esperados se retorna una excepción.</exception>
        private static string ConvertirTipoRespaldoValorBaseDatos(string tipoRespaldo)
        {
            switch (tipoRespaldo)
            {
                case "Completo":
                    tipoRespaldo = "Full";
                    break;
                case "Diferencial":
                    tipoRespaldo = "Diff";
                    break;
                default:
                    throw new ArgumentException("tipoRespaldo incorrecto (Completo|Diferencial)");
            }

            return tipoRespaldo;
        }
    }
}
