using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Domain.Entities.Microservices;

namespace FastServer.Application.Mappings;

/// <summary>
/// Perfil de AutoMapper para entidades de microservicios
/// </summary>
public class MicroservicesMappingProfile : Profile
{
    public MicroservicesMappingProfile()
    {
        // EventType
        CreateMap<EventType, EventTypeDto>().ReverseMap();

        // User
        CreateMap<User, UserDto>().ReverseMap();

        // ActivityLog
        CreateMap<ActivityLog, ActivityLogDto>()
            .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        // MicroservicesCluster
        CreateMap<MicroservicesCluster, MicroservicesClusterDto>()
            .ForMember(dest => dest.Nodos, opt => opt.MapFrom(src => src.Nodos));

        // MicroserviceRegister
        CreateMap<MicroserviceRegister, MicroserviceRegisterDto>()
            .ForMember(dest => dest.CoreConnectors, opt => opt.MapFrom(src => src.MicroserviceCoreConnectors))
            .ForMember(dest => dest.MicroserviceType, opt => opt.MapFrom(src => src.MicroserviceType))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        // MicroserviceMethod
        CreateMap<MicroserviceMethod, MicroserviceMethodDto>()
            .ForMember(dest => dest.Microservice, opt => opt.MapFrom(src => src.MicroserviceRegister))
            .ForMember(dest => dest.Nodos, opt => opt.MapFrom(src => src.Nodos));

        // CoreConnectorCredential (Excluir password en el mapeo de lectura)
        CreateMap<CoreConnectorCredential, CoreConnectorCredentialDto>()
            .ForMember(dest => dest.CoreConnectorCredentialUser, opt => opt.MapFrom(src => src.CoreConnectorCredentialUser))
            .ForMember(dest => dest.CoreConnectorCredentialKey, opt => opt.MapFrom(src => src.CoreConnectorCredentialKey));

        // MicroserviceCoreConnector
        CreateMap<MicroserviceCoreConnector, MicroserviceCoreConnectorDto>()
            .ForMember(dest => dest.Credential, opt => opt.MapFrom(src => src.CoreConnectorCredential))
            .ForMember(dest => dest.Microservice, opt => opt.MapFrom(src => src.MicroserviceRegister));

        // FastServerCluster
        CreateMap<FastServerCluster, FastServerClusterDto>();

        // MicroservicesRegisterType
        CreateMap<MicroservicesRegisterType, MicroservicesRegisterTypeDto>();

        // Nodo
        CreateMap<Nodo, NodoDto>()
            .ForMember(dest => dest.MicroserviceMethod, opt => opt.MapFrom(src => src.MicroserviceMethod))
            .ForMember(dest => dest.MicroservicesCluster, opt => opt.MapFrom(src => src.MicroservicesCluster));
    }
}
