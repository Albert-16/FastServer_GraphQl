using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using FastServer.Infrastructure.Data.Configurations;
using FastServer.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Data.Contexts;

/// <summary>
/// Contexto exclusivo para logs de servicios (BD: FastServer_Logs).
/// NO contiene entidades de microservicios - ver PostgreSqlMicroservicesDbContext.
/// </summary>
public class PostgreSqlLogsDbContext : DbContext, ILogsDbContext
{

    public PostgreSqlLogsDbContext(DbContextOptions<PostgreSqlLogsDbContext> options) : base(options)
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

        // Aplicar datos de prueba (siempre para migraciones)
        DatabaseSeeder.Seed(modelBuilder);
    }
}
