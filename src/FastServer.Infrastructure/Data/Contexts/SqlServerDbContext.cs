using FastServer.Domain.Entities;
using FastServer.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Data.Contexts;

/// <summary>
/// Contexto de base de datos SQL Server
/// </summary>
public class SqlServerDbContext : DbContext
{
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
    {
    }

    public DbSet<LogServicesHeader> LogServicesHeaders => Set<LogServicesHeader>();
    public DbSet<LogMicroservice> LogMicroservices => Set<LogMicroservice>();
    public DbSet<LogServicesContent> LogServicesContents => Set<LogServicesContent>();
    public DbSet<LogServicesHeaderHistorico> LogServicesHeadersHistorico => Set<LogServicesHeaderHistorico>();
    public DbSet<LogMicroserviceHistorico> LogMicroservicesHistorico => Set<LogMicroserviceHistorico>();
    public DbSet<LogServicesContentHistorico> LogServicesContentsHistorico => Set<LogServicesContentHistorico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones para SQL Server
        modelBuilder.ApplyConfiguration(new LogServicesHeaderSqlServerConfiguration());
        modelBuilder.ApplyConfiguration(new LogMicroserviceSqlServerConfiguration());
        modelBuilder.ApplyConfiguration(new LogServicesContentSqlServerConfiguration());
        modelBuilder.ApplyConfiguration(new LogServicesHeaderHistoricoSqlServerConfiguration());
        modelBuilder.ApplyConfiguration(new LogMicroserviceHistoricoSqlServerConfiguration());
        modelBuilder.ApplyConfiguration(new LogServicesContentHistoricoSqlServerConfiguration());
    }
}
