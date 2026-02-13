using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FastServer.Application.Interfaces;

/// <summary>
/// Interfaz para el contexto de base de datos de logs (PostgreSQL: FastServer_Logs).
/// Permite desacoplar la capa de aplicación de la implementación específica del contexto.
/// </summary>
public interface ILogsDbContext
{
    DbSet<LogServicesHeader> LogServicesHeaders { get; }
    DbSet<LogMicroservice> LogMicroservices { get; }
    DbSet<LogServicesContent> LogServicesContents { get; }
    DbSet<LogServicesHeaderHistorico> LogServicesHeadersHistorico { get; }
    DbSet<LogMicroserviceHistorico> LogMicroservicesHistorico { get; }
    DbSet<LogServicesContentHistorico> LogServicesContentsHistorico { get; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
