using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para MicroserviceRegister
/// </summary>
public class MicroserviceRegisterConfiguration : IEntityTypeConfiguration<MicroserviceRegister>
{
    public void Configure(EntityTypeBuilder<MicroserviceRegister> builder)
    {
        builder.ToTable("microservice_registers");

        builder.HasKey(e => e.MicroserviceId);

        builder.Property(e => e.MicroserviceId)
            .HasColumnName("microservice_id");

        builder.Property(e => e.MicroserviceName)
            .HasColumnName("microservice_name")
            .HasMaxLength(255);

        builder.Property(e => e.MicroserviceActive)
            .HasColumnName("microservice_active");

        builder.Property(e => e.MicroserviceDeleted)
            .HasColumnName("microservice_deleted");

        builder.Property(e => e.MicroserviceCoreConnection)
            .HasColumnName("microservice_core_connection");

        builder.Property(e => e.SoapBase)
            .HasColumnName("soap_base")
            .HasMaxLength(250);

        builder.Property(e => e.MicroserviceTypeId)
            .HasColumnName("microservice_type_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        builder.Property(e => e.DeleteAt)
            .HasColumnName("delete_at");

        // Relaciones
        builder.HasOne(e => e.MicroserviceType)
            .WithMany(t => t.MicroserviceRegisters)
            .HasForeignKey(e => e.MicroserviceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.MicroserviceCoreConnectors)
            .WithOne(c => c.MicroserviceRegister)
            .HasForeignKey(c => c.MicroserviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.MicroserviceMethods)
            .WithOne(m => m.MicroserviceRegister)
            .HasForeignKey(m => m.MicroserviceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(e => e.MicroserviceName);
        builder.HasIndex(e => e.MicroserviceTypeId);
    }
}
