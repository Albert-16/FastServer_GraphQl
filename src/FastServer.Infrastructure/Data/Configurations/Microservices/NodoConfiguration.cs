using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para Nodo (tabla intermedia Method-Cluster)
/// </summary>
public class NodoConfiguration : IEntityTypeConfiguration<Nodo>
{
    public void Configure(EntityTypeBuilder<Nodo> builder)
    {
        builder.ToTable("FastServer_Nodo");

        builder.HasKey(e => e.NodoId);

        builder.Property(e => e.NodoId)
            .HasColumnName("fastserver_nodo_id");

        builder.Property(e => e.MicroserviceMethodId)
            .HasColumnName("fastserver_microservice_method_id");

        builder.Property(e => e.MicroservicesClusterId)
            .HasColumnName("fastserver_microservices_cluster_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        // Relaciones
        builder.HasOne(e => e.MicroserviceMethod)
            .WithMany(m => m.Nodos)
            .HasForeignKey(e => e.MicroserviceMethodId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.MicroservicesCluster)
            .WithMany(c => c.Nodos)
            .HasForeignKey(e => e.MicroservicesClusterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice único compuesto para evitar duplicados
        builder.HasIndex(e => new { e.MicroserviceMethodId, e.MicroservicesClusterId })
            .IsUnique()
            .HasDatabaseName("UX_FastServer_Nodo_Method_Cluster");

        builder.HasIndex(e => e.MicroservicesClusterId)
            .HasDatabaseName("IX_FastServer_Nodo_ClusterId");
    }
}
