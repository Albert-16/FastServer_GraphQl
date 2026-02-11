using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FastServer.Infrastructure.Data;

/// <summary>
/// Factory para crear el DbContext de PostgreSQL Logs en tiempo de diseño (migraciones)
/// BD: FastServer_Logs
/// </summary>
public class PostgreSqlLogsDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlLogsDbContext>
{
    public PostgreSqlLogsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSQLLogs")
            ?? "Host=localhost;Port=5432;Database=FastServer_Logs;Username=postgres;Password=Souma";

        var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlLogsDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "public");
        });

        return new PostgreSqlLogsDbContext(optionsBuilder.Options);
    }
}

/// <summary>
/// Factory para crear el DbContext de PostgreSQL Microservices en tiempo de diseño (migraciones)
/// BD: FastServer
/// </summary>
public class PostgreSqlMicroservicesDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlMicroservicesDbContext>
{
    public PostgreSqlMicroservicesDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSQLMicroservices")
            ?? "Host=localhost;Port=5432;Database=FastServer;Username=postgres;Password=Souma";

        var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlMicroservicesDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "public");
        });

        return new PostgreSqlMicroservicesDbContext(optionsBuilder.Options);
    }
}
