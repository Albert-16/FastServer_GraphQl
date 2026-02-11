using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using FastServer.Infrastructure.Data.Configurations.Microservices;
using FastServer.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Data.Contexts;

/// <summary>
/// Contexto exclusivo para gestión de microservicios (BD: FastServer).
/// NO contiene entidades de logs - ver PostgreSqlLogsDbContext.
/// </summary>
public class PostgreSqlMicroservicesDbContext : DbContext, IMicroservicesDbContext
{
    public PostgreSqlMicroservicesDbContext(DbContextOptions<PostgreSqlMicroservicesDbContext> options) : base(options)
    {
    }

    public DbSet<EventType> EventTypes => Set<EventType>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<MicroserviceRegister> MicroserviceRegisters => Set<MicroserviceRegister>();
    public DbSet<MicroservicesCluster> MicroservicesClusters => Set<MicroservicesCluster>();
    public DbSet<MicroserviceCoreConnector> MicroserviceCoreConnectors => Set<MicroserviceCoreConnector>();
    public DbSet<CoreConnectorCredential> CoreConnectorCredentials => Set<CoreConnectorCredential>();
    public DbSet<MicroserviceMethod> MicroserviceMethods => Set<MicroserviceMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones para microservicios
        modelBuilder.ApplyConfiguration(new EventTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityLogConfiguration());
        modelBuilder.ApplyConfiguration(new MicroserviceRegisterConfiguration());
        modelBuilder.ApplyConfiguration(new MicroservicesClusterConfiguration());
        modelBuilder.ApplyConfiguration(new MicroserviceCoreConnectorConfiguration());
        modelBuilder.ApplyConfiguration(new CoreConnectorCredentialConfiguration());
        modelBuilder.ApplyConfiguration(new MicroserviceMethodConfiguration());

        // Aplicar datos de prueba (siempre para migraciones)
        MicroservicesSeeder.Seed(modelBuilder);
    }
}
