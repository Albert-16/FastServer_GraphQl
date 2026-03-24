using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para MicroservicesRegisterType
/// </summary>
public class MicroservicesRegisterTypeConfiguration : IEntityTypeConfiguration<MicroservicesRegisterType>
{
    public void Configure(EntityTypeBuilder<MicroservicesRegisterType> builder)
    {
        builder.ToTable("FastServer_Microservices_RegisterType");

        builder.HasKey(e => e.MicroservicesRegisterTypeId);

        builder.Property(e => e.MicroservicesRegisterTypeId)
            .HasColumnName("fastserver_microservices_register_type_id");

        builder.Property(e => e.MicroservicesRegisterTypeName)
            .HasColumnName("fastserver_microservices_register_type_name")
            .HasMaxLength(100);

        builder.Property(e => e.MicroservicesRegisterTypeDescription)
            .HasColumnName("fastserver_microservices_register_type_description")
            .HasMaxLength(500);

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        // Relaciones
        builder.HasMany(e => e.MicroserviceRegisters)
            .WithOne(r => r.MicroserviceType)
            .HasForeignKey(r => r.MicroserviceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.MicroservicesRegisterTypeName)
            .HasDatabaseName("IX_FastServer_Microservices_RegisterType_Name");
    }
}
