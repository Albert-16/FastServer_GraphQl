using FastServer.Domain.Entities;
using FastServer.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Data.Contexts;

/// <summary>
/// Contexto de base de datos PostgreSQL
/// </summary>
public class PostgreSqlDbContext : DbContext
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options)
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

        // Aplicar configuraciones
        modelBuilder.ApplyConfiguration(new LogServicesHeaderConfiguration());
        modelBuilder.ApplyConfiguration(new LogMicroserviceConfiguration());
        modelBuilder.ApplyConfiguration(new LogServicesContentConfiguration());
        modelBuilder.ApplyConfiguration(new LogServicesHeaderHistoricoConfiguration());
        modelBuilder.ApplyConfiguration(new LogMicroserviceHistoricoConfiguration());
        modelBuilder.ApplyConfiguration(new LogServicesContentHistoricoConfiguration());
    }
}
