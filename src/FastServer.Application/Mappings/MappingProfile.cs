using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Domain.Entities;

namespace FastServer.Application.Mappings;

/// <summary>
/// Perfil de mapeo AutoMapper
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // LogServicesHeader mappings
        CreateMap<LogServicesHeader, LogServicesHeaderDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));

        CreateMap<CreateLogServicesHeaderDto, LogServicesHeader>()
            .ForMember(dest => dest.LogId, opt => opt.Ignore());

        CreateMap<LogServicesHeaderHistorico, LogServicesHeaderDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));

        // LogMicroservice mappings
        CreateMap<LogMicroservice, LogMicroserviceDto>();

        CreateMap<CreateLogMicroserviceDto, LogMicroservice>()
            .ForMember(dest => dest.LogMicroserviceId, opt => opt.Ignore());

        CreateMap<LogMicroserviceHistorico, LogMicroserviceDto>();

        // LogServicesContent mappings
        CreateMap<LogServicesContent, LogServicesContentDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));

        CreateMap<CreateLogServicesContentDto, LogServicesContent>();

        CreateMap<LogServicesContentHistorico, LogServicesContentDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));
    }
}
