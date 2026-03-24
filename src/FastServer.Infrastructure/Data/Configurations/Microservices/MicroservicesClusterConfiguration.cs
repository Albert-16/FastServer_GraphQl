using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para MicroservicesCluster
/// </summary>
public class MicroservicesClusterConfiguration : IEntityTypeConfiguration<MicroservicesCluster>
{
    public void Configure(EntityTypeBuilder<MicroservicesCluster> builder)
    {
        builder.ToTable("FastServer_Microservices_Cluster");

        builder.HasKey(e => e.MicroservicesClusterId);

        builder.Property(e => e.MicroservicesClusterId)
            .HasColumnName("fastserver_microservices_cluster_id");

        builder.Property(e => e.MicroservicesClusterName)
            .HasColumnName("fastserver_microservices_cluster_name")
            .HasMaxLength(255);

        builder.Property(e => e.MicroservicesClusterServerName)
            .HasColumnName("fastserver_microservices_cluster_server_name")
            .HasMaxLength(255);

        builder.Property(e => e.MicroservicesClusterServerIp)
            .HasColumnName("fastserver_microservices_cluster_server_ip")
            .HasMaxLength(50);

        builder.Property(e => e.MicroservicesClusterProtocol)
            .HasColumnName("fastserver_microservices_cluster_protocol")
            .HasMaxLength(250);

        builder.Property(e => e.MicroservicesClusterActive)
            .HasColumnName("fastserver_microservices_cluster_active");

        builder.Property(e => e.MicroservicesClusterDeleted)
            .HasColumnName("fastserver_microservices_cluster_deleted");

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        builder.Property(e => e.DeleteAt)
            .HasColumnName("fastserver_delete_at");

        // Relaciones
        builder.HasMany(e => e.Nodos)
            .WithOne(n => n.MicroservicesCluster)
            .HasForeignKey(n => n.MicroservicesClusterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(e => e.MicroservicesClusterName)
            .HasDatabaseName("IX_FastServer_Microservices_Cluster_Name");
    }
}
