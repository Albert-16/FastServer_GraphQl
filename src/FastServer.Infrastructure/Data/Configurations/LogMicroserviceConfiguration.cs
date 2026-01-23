using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración EF Core para LogMicroservice (PostgreSQL)
/// </summary>
public class LogMicroserviceConfiguration : IEntityTypeConfiguration<LogMicroservice>
{
    public void Configure(EntityTypeBuilder<LogMicroservice> builder)
    {
        builder.ToTable("FastServer_LogMicroservice");

        builder.HasKey(e => e.LogId);

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogMicroserviceText)
            .HasColumnName("fastserver_logmicroservice_text")
            .HasColumnType("text");

        builder.HasIndex(e => e.LogId);
    }
}
