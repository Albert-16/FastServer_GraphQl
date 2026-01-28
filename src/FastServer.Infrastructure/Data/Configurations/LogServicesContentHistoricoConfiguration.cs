using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración EF Core para LogServicesContentHistorico (PostgreSQL)
/// </summary>
public class LogServicesContentHistoricoConfiguration : IEntityTypeConfiguration<LogServicesContentHistorico>
{
    public void Configure(EntityTypeBuilder<LogServicesContentHistorico> builder)
    {
        builder.ToTable("FastServer_LogServices_Content_Historico");

        builder.HasKey(e => e.LogId);

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogServicesDate)
            .HasColumnName("fastserver_logservices_date")
            .HasMaxLength(255);

        builder.Property(e => e.LogServicesLogLevel)
            .HasColumnName("fastserver_logservices_log_level")
            .HasMaxLength(50);

        builder.Property(e => e.LogServicesState)
            .HasColumnName("fastserver_logservices_state")
            .HasMaxLength(50);

        builder.Property(e => e.LogServicesContentText)
            .HasColumnName("fastserver_logservices_content_text")
            .HasColumnType("text");

        builder.HasIndex(e => e.LogId);
    }
}
