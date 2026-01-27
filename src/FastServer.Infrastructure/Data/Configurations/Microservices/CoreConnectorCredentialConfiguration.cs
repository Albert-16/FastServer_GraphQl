using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para CoreConnectorCredential
/// </summary>
public class CoreConnectorCredentialConfiguration : IEntityTypeConfiguration<CoreConnectorCredential>
{
    public void Configure(EntityTypeBuilder<CoreConnectorCredential> builder)
    {
        builder.ToTable("core_connector_credentials");

        builder.HasKey(e => e.CoreConnectorCredentialId);

        builder.Property(e => e.CoreConnectorCredentialId)
            .HasColumnName("core_connector_credential_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CoreConnectorCredentialUser)
            .HasColumnName("core_connector_credential_user")
            .HasMaxLength(255);

        builder.Property(e => e.CoreConnectorCredentialPass)
            .HasColumnName("core_connector_credential_pass")
            .HasMaxLength(500);

        builder.Property(e => e.CoreConnectorCredentialKey)
            .HasColumnName("core_connector_credential_key")
            .HasMaxLength(500);

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        // Relaciones
        builder.HasMany(e => e.MicroserviceCoreConnectors)
            .WithOne(c => c.CoreConnectorCredential)
            .HasForeignKey(c => c.CoreConnectorCredentialId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.CoreConnectorCredentialUser);
    }
}
