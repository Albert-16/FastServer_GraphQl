using FastServer.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FastServer.Infrastructure.Data;

/// <summary>
/// Factory para crear el DbContext de PostgreSQL en tiempo de diseño (migraciones)
/// </summary>
public class PostgreSqlDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlDbContext>
{
    public PostgreSqlDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
           // .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSQL")
            ?? "Host=localhost;Database=FastServerLogs;Username=postgres;Password=Souma";

        var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "public");
        });

        return new PostgreSqlDbContext(optionsBuilder.Options);
    }
}

/// <summary>
/// Factory para crear el DbContext de SQL Server en tiempo de diseño (migraciones)
/// </summary>
public class SqlServerDbContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
{
    public SqlServerDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
          //  .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("SqlServer")
            ?? "Server=DESKTOP-9C0B00C\\SQLEXPRESS;Database=FastServerLogs_Dev;Integrated Security=True;TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
        });

        return new SqlServerDbContext(optionsBuilder.Options);
    }
}
