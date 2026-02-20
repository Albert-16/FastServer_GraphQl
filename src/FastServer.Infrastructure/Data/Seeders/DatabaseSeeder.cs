using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Data.Seeders;

/// <summary>
/// Clase para poblar la base de datos PostgreSQL con datos de prueba
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Aplica el seeding de datos de prueba para PostgreSQL (Logs)
    /// </summary>
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedLogServicesHeaders(modelBuilder);
        SeedLogMicroservices(modelBuilder);
        SeedLogServicesContents(modelBuilder);
        SeedLogServicesHeadersHistorico(modelBuilder);
        SeedLogMicroservicesHistorico(modelBuilder);
        SeedLogServicesContentsHistorico(modelBuilder);
    }

    private static void SeedLogServicesHeaders(ModelBuilder modelBuilder)
    {
        var baseDate = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<LogServicesHeader>().HasData(
            new LogServicesHeader
            {
                LogId = 1,
                LogDateIn = baseDate,
                LogDateOut = baseDate.AddMilliseconds(150),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/users/authenticate",
                LogMethodName = "AuthenticateUser",
                HttpMethod = "POST",
                MicroserviceName = "AuthService",
                RequestDuration = "150.00 ms",
                TransactionId = "TRX-001-2025",
                UserId = "admin",
                SessionId = "SES-001",
                IpFs = "192.168.1.100",
                TypeProcess = "Authentication"
            },
            new LogServicesHeader
            {
                LogId = 2,
                LogDateIn = baseDate.AddMinutes(5),
                LogDateOut = baseDate.AddMinutes(5).AddMilliseconds(320),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/products/search",
                LogMethodName = "SearchProducts",
                HttpMethod = "GET",
                MicroserviceName = "ProductService",
                RequestDuration = "320.00 ms",
                TransactionId = "TRX-002-2025",
                UserId = "user001",
                SessionId = "SES-002",
                IpFs = "192.168.1.101",
                TypeProcess = "Query"
            }
        );
    }

    private static void SeedLogMicroservices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogMicroservice>().HasData(
            new LogMicroservice
            {
                LogMicroserviceId = Guid.Parse("01965a00-0000-7000-8000-000000000001"),
                LogId = 1,
                EventName = "AuthenticateUser",
                LogDate = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                LogLevel = "INFO",
                LogMicroserviceText = "[AuthService] Inicio de autenticación para usuario: admin"
            },
            new LogMicroservice
            {
                LogMicroserviceId = Guid.Parse("01965a00-0000-7000-8000-000000000002"),
                LogId = 2,
                EventName = "SearchProducts",
                LogDate = new DateTime(2025, 1, 1, 10, 5, 0, DateTimeKind.Utc),
                LogLevel = "INFO",
                LogMicroserviceText = "[ProductService] Búsqueda ejecutada: 'laptop gaming' - 15 resultados encontrados"
            }
        );
    }

    private static void SeedLogServicesContents(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogServicesContent>().HasData(
            new LogServicesContent
            {
                LogServicesContentId = Guid.Parse("01965b00-0000-7000-8000-000000000001"),
                LogId = 1,
                EventName = "AuthenticateUser",
                LogServicesDate = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                LogServicesLogLevel = "INFO",
                LogServicesState = "SUCCESS",
                LogServicesContentText = "{\"username\": \"admin\", \"timestamp\": \"2025-01-01T10:00:00Z\", \"ip\": \"192.168.1.100\"}"
            },
            new LogServicesContent
            {
                LogServicesContentId = Guid.Parse("01965b00-0000-7000-8000-000000000002"),
                LogId = 2,
                EventName = "SearchProducts",
                LogServicesDate = new DateTime(2025, 1, 1, 10, 5, 0, DateTimeKind.Utc),
                LogServicesLogLevel = "INFO",
                LogServicesState = "SUCCESS",
                LogServicesContentText = "{\"searchTerm\": \"laptop gaming\", \"resultsCount\": 15, \"executionTime\": \"320ms\"}"
            }
        );
    }

    private static void SeedLogServicesHeadersHistorico(ModelBuilder modelBuilder)
    {
        var baseDate = new DateTime(2024, 12, 15, 10, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<LogServicesHeaderHistorico>().HasData(
            new LogServicesHeaderHistorico
            {
                LogId = 1,
                LogDateIn = baseDate,
                LogDateOut = baseDate.AddMilliseconds(145),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/users/authenticate",
                LogMethodName = "AuthenticateUser",
                HttpMethod = "POST",
                MicroserviceName = "AuthService",
                RequestDuration = "145.00 ms",
                TransactionId = "TRX-HIST-001-2024",
                UserId = "admin",
                SessionId = "SES-HIST-001",
                IpFs = "192.168.1.50",
                TypeProcess = "Authentication"
            },
            new LogServicesHeaderHistorico
            {
                LogId = 2,
                LogDateIn = baseDate.AddMinutes(10),
                LogDateOut = baseDate.AddMinutes(10).AddMilliseconds(280),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/products/search",
                LogMethodName = "SearchProducts",
                HttpMethod = "GET",
                MicroserviceName = "ProductService",
                RequestDuration = "280.00 ms",
                TransactionId = "TRX-HIST-002-2024",
                UserId = "user001",
                SessionId = "SES-HIST-002",
                IpFs = "192.168.1.51",
                TypeProcess = "Query"
            }
        );
    }

    private static void SeedLogMicroservicesHistorico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogMicroserviceHistorico>().HasData(
            new LogMicroserviceHistorico
            {
                LogMicroserviceId = Guid.Parse("01965a00-0000-7000-8000-000000000003"),
                LogId = 1,
                EventName = "AuthenticateUser",
                LogDate = new DateTime(2024, 12, 15, 10, 0, 0, DateTimeKind.Utc),
                LogLevel = "INFO",
                LogMicroserviceText = "[AuthService] Autenticación histórica exitosa para usuario: admin"
            },
            new LogMicroserviceHistorico
            {
                LogMicroserviceId = Guid.Parse("01965a00-0000-7000-8000-000000000004"),
                LogId = 2,
                EventName = "SearchProducts",
                LogDate = new DateTime(2024, 12, 15, 10, 10, 0, DateTimeKind.Utc),
                LogLevel = "INFO",
                LogMicroserviceText = "[ProductService] Búsqueda histórica ejecutada: 'gaming keyboard' - 12 resultados"
            }
        );
    }

    private static void SeedLogServicesContentsHistorico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogServicesContentHistorico>().HasData(
            new LogServicesContentHistorico
            {
                LogServicesContentId = Guid.Parse("01965b00-0000-7000-8000-000000000003"),
                LogId = 1,
                EventName = "AuthenticateUser",
                LogServicesDate = new DateTime(2024, 12, 15, 10, 0, 0, DateTimeKind.Utc),
                LogServicesLogLevel = "INFO",
                LogServicesState = "SUCCESS",
                LogServicesContentText = "{\"username\": \"admin\", \"timestamp\": \"2024-12-15T10:00:00Z\", \"ip\": \"192.168.1.50\", \"archived\": true}"
            },
            new LogServicesContentHistorico
            {
                LogId = 2,
                LogServicesContentId = Guid.Parse("01965b00-0000-7000-8000-000000000004"),
                EventName = "SearchProducts",
                LogServicesDate = new DateTime(2024, 12, 15, 10, 10, 0, DateTimeKind.Utc),
                LogServicesLogLevel = "INFO",
                LogServicesState = "SUCCESS",
                LogServicesContentText = "{\"searchTerm\": \"gaming keyboard\", \"resultsCount\": 12, \"executionTime\": \"280ms\", \"archived\": true}"
            }
        );
    }
}
