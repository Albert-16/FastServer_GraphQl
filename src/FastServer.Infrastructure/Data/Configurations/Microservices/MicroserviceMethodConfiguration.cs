using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para MicroserviceMethod
/// </summary>
public class MicroserviceMethodConfiguration : IEntityTypeConfiguration<MicroserviceMethod>
{
    public void Configure(EntityTypeBuilder<MicroserviceMethod> builder)
    {
        builder.ToTable("microservice_methods");

        builder.HasKey(e => e.MicroserviceMethodId);

        builder.Property(e => e.MicroserviceMethodId)
            .HasColumnName("microservice_method_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.MicroserviceId)
            .HasColumnName("microservice_id");

        builder.Property(e => e.MicroserviceMethodDelete)
            .HasColumnName("microservice_method_delete");

        builder.Property(e => e.MicroserviceMethodName)
            .HasColumnName("microservice_method_name")
            .HasMaxLength(255);

        builder.Property(e => e.MicroserviceMethodUrl)
            .HasColumnName("microservice_method_url")
            .HasMaxLength(500);

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        // Relaciones
        builder.HasOne(e => e.MicroserviceRegister)
            .WithMany(r => r.MicroserviceMethods)
            .HasForeignKey(e => e.MicroserviceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(e => e.MicroserviceId);
        builder.HasIndex(e => e.MicroserviceMethodName);
        builder.HasIndex(e => e.MicroserviceMethodDelete);
    }
}
