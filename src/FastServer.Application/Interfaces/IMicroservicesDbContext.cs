using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Interfaz para el contexto de base de datos de microservicios (PostgreSQL: FastServer).
/// Permite desacoplar la capa de aplicación de la implementación específica del contexto.
/// </summary>
public interface IMicroservicesDbContext
{
    DbSet<EventType> EventTypes { get; }
    DbSet<User> Users { get; }
    DbSet<ActivityLog> ActivityLogs { get; }
    DbSet<MicroserviceRegister> MicroserviceRegisters { get; }
    DbSet<MicroservicesCluster> MicroservicesClusters { get; }
    DbSet<MicroserviceCoreConnector> MicroserviceCoreConnectors { get; }
    DbSet<CoreConnectorCredential> CoreConnectorCredentials { get; }
    DbSet<MicroserviceMethod> MicroserviceMethods { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
