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
        builder.ToTable("FastServer_Microservice_Register");

        builder.HasKey(e => e.MicroserviceId);

        builder.Property(e => e.MicroserviceId)
            .HasColumnName("fastserver_microservice_id");

        builder.Property(e => e.MicroserviceName)
            .HasColumnName("fastserver_microservice_name")
            .HasMaxLength(255);

        builder.Property(e => e.MicroserviceActive)
            .HasColumnName("fastserver_microservice_active");

        builder.Property(e => e.MicroserviceDeleted)
            .HasColumnName("fastserver_microservice_deleted");

        builder.Property(e => e.MicroserviceCoreConnection)
            .HasColumnName("fastserver_microservice_core_connection");

        builder.Property(e => e.SoapBase)
            .HasColumnName("fastserver_soap_base")
            .HasMaxLength(250);

        builder.Property(e => e.MicroserviceTypeId)
            .HasColumnName("fastserver_microservice_type_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        builder.Property(e => e.DeleteAt)
            .HasColumnName("fastserver_delete_at");

        builder.Property(e => e.FastServerUserId)
            .HasColumnName("fastserver_user_id");

        // Relaciones
        builder.HasOne(e => e.User)
            .WithMany(u => u.MicroserviceRegisters)
            .HasForeignKey(e => e.FastServerUserId)
            .OnDelete(DeleteBehavior.Restrict);

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
        builder.HasIndex(e => e.MicroserviceName)
            .HasDatabaseName("IX_FastServer_Microservice_Register_Name");
        builder.HasIndex(e => e.MicroserviceTypeId)
            .HasDatabaseName("IX_FastServer_Microservice_Register_TypeId");
        builder.HasIndex(e => e.FastServerUserId)
            .HasDatabaseName("IX_FastServer_Microservice_Register_UserId");
    }
}
