using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración EF Core para LogMicroserviceHistorico (PostgreSQL)
/// </summary>
public class LogMicroserviceHistoricoConfiguration : IEntityTypeConfiguration<LogMicroserviceHistorico>
{
    public void Configure(EntityTypeBuilder<LogMicroserviceHistorico> builder)
    {
        builder.ToTable("FastServer_LogMicroservice_Historico");

        builder.HasKey(e => e.LogMicroserviceId);

        builder.Property(e => e.LogMicroserviceId)
            .HasColumnName("fastserver_log_microservice_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id")
            .IsRequired();

        builder.Property(e => e.RequestId)
            .HasColumnName("fastserver_request_id")
            .IsRequired();

        builder.Property(e => e.EventName)
            .HasColumnName("fastserver_event_name")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(e => e.LogDate)
            .HasColumnName("fastserver_log_date")
            .HasColumnType("timestamp with time zone");

        builder.Property(e => e.LogLevel)
            .HasColumnName("fastserver_log_level")
            .HasMaxLength(50);

        builder.Property(e => e.LogMicroserviceText)
            .HasColumnName("fastserver_logmicroservice_text")
            .HasColumnType("text")
            .IsRequired();

        builder.HasIndex(e => e.LogId)
            .HasDatabaseName("IX_FastServer_LogMicroservice_Historico_LogId");
    }
}
