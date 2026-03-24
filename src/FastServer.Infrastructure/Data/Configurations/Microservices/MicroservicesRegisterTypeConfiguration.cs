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
        builder.ToTable("microservices_register_types");

        builder.HasKey(e => e.MicroservicesRegisterTypeId);

        builder.Property(e => e.MicroservicesRegisterTypeId)
            .HasColumnName("microservices_register_type_id");

        builder.Property(e => e.MicroservicesRegisterTypeName)
            .HasColumnName("microservices_register_type_name")
            .HasMaxLength(100);

        builder.Property(e => e.MicroservicesRegisterTypeDescription)
            .HasColumnName("microservices_register_type_description")
            .HasMaxLength(500);

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        // Relaciones
        builder.HasMany(e => e.MicroserviceRegisters)
            .WithOne(r => r.MicroserviceType)
            .HasForeignKey(r => r.MicroserviceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.MicroservicesRegisterTypeName);
    }
}
