using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("===========================================");
Console.WriteLine("FastServer Data Migrator");
Console.WriteLine("SQL Server → PostgreSQL");
Console.WriteLine("===========================================");
Console.WriteLine();

// Cargar configuración
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var sqlServerConn = config.GetConnectionString("SqlServer");
var postgresConn = config.GetConnectionString("PostgreSQL");

if (string.IsNullOrEmpty(sqlServerConn))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: No se encontró la cadena de conexión de SQL Server.");
    Console.ResetColor();
    return;
}

if (string.IsNullOrEmpty(postgresConn))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: No se encontró la cadena de conexión de PostgreSQL.");
    Console.ResetColor();
    return;
}

// Crear contextos
var sqlServerOptions = new DbContextOptionsBuilder<SqlServerDbContext>()
    .UseSqlServer(sqlServerConn)
    .Options;

var postgresOptions = new DbContextOptionsBuilder<PostgreSqlDbContext>()
    .UseNpgsql(postgresConn)
    .Options;

Console.WriteLine($"Origen: SQL Server");
Console.WriteLine($"Destino: PostgreSQL");
Console.WriteLine();

try
{
    using var sourceDb = new SqlServerDbContext(sqlServerOptions);
    using var targetDb = new PostgreSqlDbContext(postgresOptions);

    Console.WriteLine("Verificando conexiones...");

    // Verificar conexión a SQL Server
    try
    {
        await sourceDb.Database.CanConnectAsync();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✓ Conexión a SQL Server exitosa");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ Error conectando a SQL Server: {ex.Message}");
        Console.ResetColor();
        return;
    }

    // Verificar conexión a PostgreSQL
    try
    {
        await targetDb.Database.CanConnectAsync();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✓ Conexión a PostgreSQL exitosa");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ Error conectando a PostgreSQL: {ex.Message}");
        Console.ResetColor();
        return;
    }

    Console.WriteLine();
    Console.WriteLine("Limpiando tablas de destino...");

    // Limpiar datos existentes
    await targetDb.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"FastServer_LogServices_Header\" CASCADE");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✓ Tablas limpiadas");
    Console.ResetColor();

    Console.WriteLine();
    Console.WriteLine("Iniciando migración de datos...");
    Console.WriteLine();

    // MIGRAR LogServicesHeader
    Console.WriteLine("Migrando LogServicesHeader...");
    var batchSize = 1000;
    var totalRecords = await sourceDb.LogServicesHeaders.CountAsync();
    Console.WriteLine($"Total de registros a migrar: {totalRecords}");

    if (totalRecords == 0)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("No hay registros para migrar en LogServicesHeader.");
        Console.ResetColor();
    }
    else
    {
        var processed = 0;
        var startTime = DateTime.Now;

        while (processed < totalRecords)
        {
            var batch = await sourceDb.LogServicesHeaders
                .OrderBy(x => x.LogId)
                .Skip(processed)
                .Take(batchSize)
                .AsNoTracking()
                .ToListAsync();

            if (!batch.Any()) break;

            // Convertir fechas a UTC para PostgreSQL
            foreach (var log in batch)
            {
                if (log.LogDateIn.Kind == DateTimeKind.Unspecified)
                    log.LogDateIn = DateTime.SpecifyKind(log.LogDateIn, DateTimeKind.Utc);

                if (log.LogDateOut.Kind == DateTimeKind.Unspecified)
                    log.LogDateOut = DateTime.SpecifyKind(log.LogDateOut, DateTimeKind.Utc);
            }

            await targetDb.LogServicesHeaders.AddRangeAsync(batch);
            await targetDb.SaveChangesAsync();

            processed += batch.Count;
            var percentage = (processed * 100.0) / totalRecords;
            var elapsed = DateTime.Now - startTime;
            var recordsPerSecond = processed / elapsed.TotalSeconds;

            Console.Write($"\rProgreso: {processed}/{totalRecords} ({percentage:F1}%) - {recordsPerSecond:F0} reg/s");

            foreach (var entity in batch)
            {
                targetDb.Entry(entity).State = EntityState.Detached;
            }
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ LogServicesHeader migrado ({processed} registros)");
        Console.ResetColor();
    }

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("===========================================");
    Console.WriteLine("✓ Migración completada exitosamente");
    Console.WriteLine("===========================================");
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("===========================================");
    Console.WriteLine("✗ Error durante la migración:");
    Console.WriteLine(ex.Message);
    if (ex.InnerException != null)
    {
        Console.WriteLine("Detalles: " + ex.InnerException.Message);
    }
    Console.WriteLine("===========================================");
    Console.ResetColor();
}
