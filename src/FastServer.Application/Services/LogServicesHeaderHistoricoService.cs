using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación de solo lectura para el histórico de cabeceras de logs
/// (FastServer_LogServices_Header_Historico).
/// </summary>
/// <remarks>
/// - Acceso directo a PostgreSQL mediante ILogsDbContext
/// - Mapeo automático entre entidades y DTOs usando AutoMapper
/// - Uso de AsNoTracking() en todas las consultas (solo lectura)
/// - No publica eventos: la tabla histórica es inmutable desde el API
/// </remarks>
public class LogServicesHeaderHistoricoService : ILogServicesHeaderHistoricoService
{
    private readonly ILogsDbContext _context;
    private readonly IMapper _mapper;

    public LogServicesHeaderHistoricoService(ILogsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LogServicesHeaderDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        LogServicesHeaderHistorico? entity = await _context.LogServicesHeadersHistorico
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<LogServicesHeaderDto?> GetWithDetailsAsync(long id, CancellationToken cancellationToken = default)
    {
        LogServicesHeaderHistorico? entity = await _context.LogServicesHeadersHistorico
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<LogServicesHeaderHistorico> query = _context.LogServicesHeadersHistorico
            .AsNoTracking()
            .Where(x => x.ErrorCode != null);

        if (fromDate.HasValue)
            query = query.Where(x => x.LogDateIn >= fromDate.Value);

        List<LogServicesHeaderHistorico> entities = await query
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities);
    }
}
