using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Data.Seeders;

/// <summary>
/// Clase para poblar la base de datos SQL Server con datos de prueba de microservicios
/// </summary>
public static class MicroservicesSeeder
{
    // Fecha fija para evitar cambios en cada compilación
    private static readonly DateTime BaseDate = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Aplica el seeding de datos de prueba para SQL Server (Microservices)
    /// </summary>
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedMicroservicesClusters(modelBuilder);
        SeedMicroserviceRegisters(modelBuilder);
        SeedMicroserviceMethods(modelBuilder);
        SeedEventTypes(modelBuilder);
        SeedUsers(modelBuilder);
        SeedCoreConnectorCredentials(modelBuilder);
        SeedMicroserviceCoreConnectors(modelBuilder);
        SeedActivityLogs(modelBuilder);
    }

    private static void SeedMicroservicesClusters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MicroservicesCluster>().HasData(
            new MicroservicesCluster
            {
                MicroservicesClusterId = 1,
                MicroservicesClusterName = "Production Cluster",
                MicroservicesClusterServerName = "prod-cluster-01",
                MicroservicesClusterServerIp = "10.0.1.100",
                MicroservicesClusterActive = true,
                MicroservicesClusterDeleted = false,
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new MicroservicesCluster
            {
                MicroservicesClusterId = 2,
                MicroservicesClusterName = "Development Cluster",
                MicroservicesClusterServerName = "dev-cluster-01",
                MicroservicesClusterServerIp = "10.0.2.100",
                MicroservicesClusterActive = true,
                MicroservicesClusterDeleted = false,
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            }
        );
    }

    private static void SeedMicroserviceRegisters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MicroserviceRegister>().HasData(
            new MicroserviceRegister
            {
                MicroserviceId = 1,
                MicroserviceClusterId = 1,
                MicroserviceName = "AuthService",
                MicroserviceActive = true,
                MicroserviceDeleted = false,
                MicroserviceCoreConnection = true,
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new MicroserviceRegister
            {
                MicroserviceId = 2,
                MicroserviceClusterId = 1,
                MicroserviceName = "ProductService",
                MicroserviceActive = true,
                MicroserviceDeleted = false,
                MicroserviceCoreConnection = true,
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            }
        );
    }

    private static void SeedMicroserviceMethods(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MicroserviceMethod>().HasData(
            new MicroserviceMethod
            {
                MicroserviceMethodId = 1,
                MicroserviceId = 1,
                MicroserviceMethodDelete = false,
                MicroserviceMethodName = "AuthenticateUser",
                MicroserviceMethodUrl = "/api/users/authenticate",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new MicroserviceMethod
            {
                MicroserviceMethodId = 2,
                MicroserviceId = 2,
                MicroserviceMethodDelete = false,
                MicroserviceMethodName = "SearchProducts",
                MicroserviceMethodUrl = "/api/products/search",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            }
        );
    }

    private static void SeedEventTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventType>().HasData(
            new EventType
            {
                EventTypeId = 1,
                EventTypeDescription = "Microservice Registration",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new EventType
            {
                EventTypeId = 2,
                EventTypeDescription = "Configuration Change",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            }
        );
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UserPeoplesoft = "PS001",
                UserActive = true,
                UserName = "Admin User",
                UserEmail = "admin@fastserver.com",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new User
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                UserPeoplesoft = "PS002",
                UserActive = true,
                UserName = "Developer User",
                UserEmail = "developer@fastserver.com",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            }
        );
    }

    private static void SeedCoreConnectorCredentials(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoreConnectorCredential>().HasData(
            new CoreConnectorCredential
            {
                CoreConnectorCredentialId = 1,
                CoreConnectorCredentialUser = "auth_service_user",
                CoreConnectorCredentialPass = "encrypted_pass_001",
                CoreConnectorCredentialKey = "api_key_001",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new CoreConnectorCredential
            {
                CoreConnectorCredentialId = 2,
                CoreConnectorCredentialUser = "product_service_user",
                CoreConnectorCredentialPass = "encrypted_pass_002",
                CoreConnectorCredentialKey = "api_key_002",
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            }
        );
    }

    private static void SeedMicroserviceCoreConnectors(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MicroserviceCoreConnector>().HasData(
            new MicroserviceCoreConnector
            {
                MicroserviceCoreConnectorId = 1,
                MicroserviceId = 1,
                CoreConnectorCredentialId = 1,
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new MicroserviceCoreConnector
            {
                MicroserviceCoreConnectorId = 2,
                MicroserviceId = 2,
                CoreConnectorCredentialId = 2,
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            }
        );
    }

    private static void SeedActivityLogs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>().HasData(
            new ActivityLog
            {
                ActivityLogId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                EventTypeId = 1,
                ActivityLogEntityName = "MicroserviceRegister",
                ActivityLogEntityId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                ActivityLogDescription = "AuthService registered successfully",
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CreateAt = BaseDate,
                ModifyAt = BaseDate
            },
            new ActivityLog
            {
                ActivityLogId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                EventTypeId = 2,
                ActivityLogEntityName = "MicroserviceRegister",
                ActivityLogEntityId = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                ActivityLogDescription = "ProductService configuration updated",
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CreateAt = BaseDate.AddMinutes(5),
                ModifyAt = BaseDate.AddMinutes(5)
            }
        );
    }
}
