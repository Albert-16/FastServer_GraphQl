using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración EF Core para LogServicesHeader (SQL Server)
/// </summary>
public class LogServicesHeaderSqlServerConfiguration : IEntityTypeConfiguration<LogServicesHeader>
{
    public void Configure(EntityTypeBuilder<LogServicesHeader> builder)
    {
        builder.ToTable("FastServer_LogServices_Header");

        builder.HasKey(e => e.LogId);

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.LogDateIn)
            .HasColumnName("fastserver_log_date_in")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.LogDateOut)
            .HasColumnName("fastserver_log_date_out")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.LogState)
            .HasColumnName("fastserver_log_state")
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.LogMethodUrl)
            .HasColumnName("fastserver_log_method_url")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.LogMethodName)
            .HasColumnName("fastserver_log_method_name")
            .HasMaxLength(255);

        builder.Property(e => e.LogFsId)
            .HasColumnName("fastserver_log_fs_id");

        builder.Property(e => e.MethodDescription)
            .HasColumnName("fastserver_method_description")
            .HasMaxLength(1000);

        builder.Property(e => e.TciIpPort)
            .HasColumnName("fastserver_tci_ip_port")
            .HasMaxLength(100);

        builder.Property(e => e.ErrorCode)
            .HasColumnName("fastserver_error_code")
            .HasMaxLength(50);

        builder.Property(e => e.ErrorDescription)
            .HasColumnName("fastserver_error_description")
            .HasMaxLength(2000);

        builder.Property(e => e.IpFs)
            .HasColumnName("fastserver_ip_fs")
            .HasMaxLength(50);

        builder.Property(e => e.TypeProcess)
            .HasColumnName("fastserver_type_process")
            .HasMaxLength(100);

        builder.Property(e => e.LogNodo)
            .HasColumnName("fastserver_log_nodo")
            .HasMaxLength(100);

        builder.Property(e => e.HttpMethod)
            .HasColumnName("fastserver_http_method")
            .HasMaxLength(20);

        builder.Property(e => e.MicroserviceName)
            .HasColumnName("fastserver_microservice_name")
            .HasMaxLength(255);

        builder.Property(e => e.RequestDuration)
            .HasColumnName("fastserver_request_duration");

        builder.Property(e => e.TransactionId)
            .HasColumnName("fastserver_transaction_id")
            .HasMaxLength(100);

        builder.Property(e => e.UserId)
            .HasColumnName("fastserver_user_id")
            .HasMaxLength(100);

        builder.Property(e => e.SessionId)
            .HasColumnName("fastserver_session_id")
            .HasMaxLength(100);

        builder.Property(e => e.RequestId)
            .HasColumnName("fastserver_request_id");

        builder.HasMany(e => e.LogMicroservices)
            .WithOne(e => e.LogServicesHeader)
            .HasForeignKey(e => e.LogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.LogServicesContents)
            .WithOne(e => e.LogServicesHeader)
            .HasForeignKey(e => e.LogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.LogDateIn);
        builder.HasIndex(e => e.LogState);
        builder.HasIndex(e => e.MicroserviceName);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.TransactionId);
    }
}

/// <summary>
/// Configuración EF Core para LogMicroservice (SQL Server)
/// </summary>
public class LogMicroserviceSqlServerConfiguration : IEntityTypeConfiguration<LogMicroservice>
{
    public void Configure(EntityTypeBuilder<LogMicroservice> builder)
    {
        builder.ToTable("FastServer_LogMicroservice");

        builder.HasKey(e => e.LogId);

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogMicroserviceText)
            .HasColumnName("fastserver_logmicroservice_text")
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(e => e.LogId);
    }
}

/// <summary>
/// Configuración EF Core para LogServicesContent (SQL Server)
/// </summary>
public class LogServicesContentSqlServerConfiguration : IEntityTypeConfiguration<LogServicesContent>
{
    public void Configure(EntityTypeBuilder<LogServicesContent> builder)
    {
        builder.ToTable("FastServer_LogServices_Content");

        builder.HasKey(e => new { e.LogId, e.ContentNo });

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogServicesContentText)
            .HasColumnName("fastserver_logservices_content_text")
            .HasColumnType("nvarchar(max)");

        builder.Property(e => e.ContentNo)
            .HasColumnName("fastserver_no")
            .HasMaxLength(50);

        builder.HasIndex(e => e.LogId);
    }
}

/// <summary>
/// Configuración EF Core para LogServicesHeaderHistorico (SQL Server)
/// </summary>
public class LogServicesHeaderHistoricoSqlServerConfiguration : IEntityTypeConfiguration<LogServicesHeaderHistorico>
{
    public void Configure(EntityTypeBuilder<LogServicesHeaderHistorico> builder)
    {
        builder.ToTable("FastServer_LogServices_Header_Historico");

        builder.HasKey(e => e.LogId);

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogDateIn)
            .HasColumnName("fastserver_log_date_in")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.LogDateOut)
            .HasColumnName("fastserver_log_date_out")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.LogState)
            .HasColumnName("fastserver_log_state")
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.LogMethodUrl)
            .HasColumnName("fastserver_log_method_url")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.LogMethodName)
            .HasColumnName("fastserver_log_method_name")
            .HasMaxLength(255);

        builder.Property(e => e.LogFsId)
            .HasColumnName("fastserver_log_fs_id");

        builder.Property(e => e.MethodDescription)
            .HasColumnName("fastserver_method_description")
            .HasMaxLength(1000);

        builder.Property(e => e.TciIpPort)
            .HasColumnName("fastserver_tci_ip_port")
            .HasMaxLength(100);

        builder.Property(e => e.ErrorCode)
            .HasColumnName("fastserver_error_code")
            .HasMaxLength(50);

        builder.Property(e => e.ErrorDescription)
            .HasColumnName("fastserver_error_description")
            .HasMaxLength(2000);

        builder.Property(e => e.IpFs)
            .HasColumnName("fastserver_ip_fs")
            .HasMaxLength(50);

        builder.Property(e => e.TypeProcess)
            .HasColumnName("fastserver_type_process")
            .HasMaxLength(100);

        builder.Property(e => e.LogNodo)
            .HasColumnName("fastserver_log_nodo")
            .HasMaxLength(100);

        builder.Property(e => e.HttpMethod)
            .HasColumnName("fastserver_http_method")
            .HasMaxLength(20);

        builder.Property(e => e.MicroserviceName)
            .HasColumnName("fastserver_microservice_name")
            .HasMaxLength(255);

        builder.Property(e => e.RequestDuration)
            .HasColumnName("fastserver_request_duration");

        builder.Property(e => e.TransactionId)
            .HasColumnName("fastserver_transaction_id")
            .HasMaxLength(100);

        builder.Property(e => e.UserId)
            .HasColumnName("fastserver_user_id")
            .HasMaxLength(100);

        builder.Property(e => e.SessionId)
            .HasColumnName("fastserver_session_id")
            .HasMaxLength(100);

        builder.Property(e => e.RequestId)
            .HasColumnName("fastserver_request_id");

        builder.HasMany(e => e.LogMicroservices)
            .WithOne(e => e.LogServicesHeader)
            .HasForeignKey(e => e.LogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.LogServicesContents)
            .WithOne(e => e.LogServicesHeader)
            .HasForeignKey(e => e.LogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.LogDateIn);
        builder.HasIndex(e => e.LogState);
    }
}

/// <summary>
/// Configuración EF Core para LogMicroserviceHistorico (SQL Server)
/// </summary>
public class LogMicroserviceHistoricoSqlServerConfiguration : IEntityTypeConfiguration<LogMicroserviceHistorico>
{
    public void Configure(EntityTypeBuilder<LogMicroserviceHistorico> builder)
    {
        builder.ToTable("FastServer_LogMicroservice_Historico");

        builder.HasKey(e => e.LogId);

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogMicroserviceText)
            .HasColumnName("fastserver_logmicroservice_text")
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.HasIndex(e => e.LogId);
    }
}

/// <summary>
/// Configuración EF Core para LogServicesContentHistorico (SQL Server)
/// </summary>
public class LogServicesContentHistoricoSqlServerConfiguration : IEntityTypeConfiguration<LogServicesContentHistorico>
{
    public void Configure(EntityTypeBuilder<LogServicesContentHistorico> builder)
    {
        builder.ToTable("FastServer_LogServices_Content_Historico");

        builder.HasKey(e => new { e.LogId, e.ContentNo });

        builder.Property(e => e.LogId)
            .HasColumnName("fastserver_log_id");

        builder.Property(e => e.LogServicesContentText)
            .HasColumnName("fastserver_logservices_content_text")
            .HasColumnType("nvarchar(max)");

        builder.Property(e => e.ContentNo)
            .HasColumnName("fastserver_no")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.LogId);
    }
}
