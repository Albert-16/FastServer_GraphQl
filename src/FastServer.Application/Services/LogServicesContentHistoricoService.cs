using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación de solo lectura para el histórico de contenidos de logs
/// (FastServer_LogServices_Content_Historico).
/// </summary>
public class LogServicesContentHistoricoService : ILogServicesContentHistoricoService
{
    private readonly ILogsDbContext _context;
    private readonly IMapper _mapper;

    public LogServicesContentHistoricoService(ILogsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LogServicesContentDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default)
    {
        List<LogServicesContentHistorico> entities = await _context.LogServicesContentsHistorico
            .AsNoTracking()
            .Where(x => x.LogId == logId)
            .OrderBy(x => x.LogServicesDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }

    public async Task<IEnumerable<LogServicesContentDto>> SearchByContentAsync(string searchText, CancellationToken cancellationToken = default)
    {
        List<LogServicesContentHistorico> entities = await _context.LogServicesContentsHistorico
            .AsNoTracking()
            .Where(x => x.LogServicesContentText != null && x.LogServicesContentText.Contains(searchText))
            .OrderByDescending(x => x.LogServicesDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }
}
