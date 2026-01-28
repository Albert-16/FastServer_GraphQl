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
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId))
            .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.RequestId != null ? (long?)src.RequestId.GetHashCode() : null));

        // LogMicroservice mappings
        CreateMap<LogMicroservice, LogMicroserviceDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));

        CreateMap<CreateLogMicroserviceDto, LogMicroservice>();

        CreateMap<LogMicroserviceHistorico, LogMicroserviceDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));

        // LogServicesContent mappings
        CreateMap<LogServicesContent, LogServicesContentDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));

        CreateMap<CreateLogServicesContentDto, LogServicesContent>();

        CreateMap<LogServicesContentHistorico, LogServicesContentDto>()
            .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId));
    }
}
