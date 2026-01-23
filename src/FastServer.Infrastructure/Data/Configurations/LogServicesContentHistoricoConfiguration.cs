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

        builder.HasKey(e => new { e.LogId, e.ContentNo });

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogServicesContentText)
            .HasColumnName("fastserver_logservices_content_text")
            .HasColumnType("text");

        builder.Property(e => e.ContentNo)
            .HasColumnName("fastserver_no")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.LogId);
    }
}
