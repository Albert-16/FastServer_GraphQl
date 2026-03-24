using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para FastServerCluster.
/// Tabla: FastServer_Cluster | PK: Guid v7 | Soft delete con DeleteAt
/// </summary>
public class FastServerClusterConfiguration : IEntityTypeConfiguration<FastServerCluster>
{
    public void Configure(EntityTypeBuilder<FastServerCluster> builder)
    {
        builder.ToTable("FastServer_Cluster");

        builder.HasKey(e => e.FastServerClusterId);

        builder.Property(e => e.FastServerClusterId)
            .HasColumnName("fastserver_cluster_id");

        builder.Property(e => e.FastServerClusterName)
            .HasColumnName("fastserver_cluster_name")
            .HasMaxLength(250);

        builder.Property(e => e.FastServerClusterUrl)
            .HasColumnName("fastserver_cluster_url")
            .HasMaxLength(500);

        builder.Property(e => e.FastServerClusterVersion)
            .HasColumnName("fastserver_cluster_version")
            .HasMaxLength(50);

        builder.Property(e => e.FastServerClusterServerName)
            .HasColumnName("fastserver_cluster_server_name")
            .HasMaxLength(250);

        builder.Property(e => e.FastServerClusterServerIp)
            .HasColumnName("fastserver_cluster_server_ip")
            .HasMaxLength(50);

        builder.Property(e => e.FastServerClusterActive)
            .HasColumnName("fastserver_cluster_active");

        builder.Property(e => e.FastServerClusterDelete)
            .HasColumnName("fastserver_cluster_delete");

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        builder.Property(e => e.DeleteAt)
            .HasColumnName("fastserver_delete_at");

        // Índices
        builder.HasIndex(e => e.FastServerClusterName)
            .HasDatabaseName("IX_FastServer_Cluster_Name");
    }
}
