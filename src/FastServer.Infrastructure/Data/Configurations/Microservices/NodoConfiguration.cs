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
        builder.ToTable("nodos");

        builder.HasKey(e => e.NodoId);

        builder.Property(e => e.NodoId)
            .HasColumnName("nodo_id");

        builder.Property(e => e.MicroserviceMethodId)
            .HasColumnName("microservice_method_id");

        builder.Property(e => e.MicroservicesClusterId)
            .HasColumnName("microservices_cluster_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

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
            .HasDatabaseName("UX_Nodo_Method_Cluster");

        builder.HasIndex(e => e.MicroservicesClusterId);
    }
}
