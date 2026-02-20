using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlLogs
{
    /// <inheritdoc />
    public partial class AddLogServicesContentIdPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogServices_Content_Historico",
                table: "FastServer_LogServices_Content_Historico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogServices_Content",
                table: "FastServer_LogServices_Content");

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content",
                keyColumn: "fastserver_log_id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content",
                keyColumn: "fastserver_log_id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 2L);

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_logservices_state",
                table: "FastServer_LogServices_Content_Historico",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            // Convertir fastserver_logservices_date de varchar a timestamp con USING para datos existentes
            migrationBuilder.Sql(
                @"ALTER TABLE ""FastServer_LogServices_Content_Historico""
                  ALTER COLUMN fastserver_logservices_date TYPE timestamp with time zone
                  USING CASE
                    WHEN fastserver_logservices_date IS NULL OR fastserver_logservices_date = '' THEN NULL
                    WHEN fastserver_logservices_date LIKE '%Z' OR fastserver_logservices_date LIKE '%+%'
                    THEN fastserver_logservices_date::timestamp with time zone
                    ELSE (fastserver_logservices_date || ' UTC')::timestamp with time zone
                  END;");

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogServices_Content_Historico",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "fastserver_logservices_content_id",
                table: "FastServer_LogServices_Content_Historico",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "fastserver_event_name",
                table: "FastServer_LogServices_Content_Historico",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            // Backfill UUIDs para registros existentes en Historico
            migrationBuilder.Sql(
                @"UPDATE ""FastServer_LogServices_Content_Historico""
                  SET fastserver_logservices_content_id = gen_random_uuid()
                  WHERE fastserver_logservices_content_id = '00000000-0000-0000-0000-000000000000';");

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_logservices_state",
                table: "FastServer_LogServices_Content",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            // Convertir fastserver_logservices_date de varchar a timestamp con USING para datos existentes
            migrationBuilder.Sql(
                @"ALTER TABLE ""FastServer_LogServices_Content""
                  ALTER COLUMN fastserver_logservices_date TYPE timestamp with time zone
                  USING CASE
                    WHEN fastserver_logservices_date IS NULL OR fastserver_logservices_date = '' THEN NULL
                    WHEN fastserver_logservices_date LIKE '%Z' OR fastserver_logservices_date LIKE '%+%'
                    THEN fastserver_logservices_date::timestamp with time zone
                    ELSE (fastserver_logservices_date || ' UTC')::timestamp with time zone
                  END;");

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogServices_Content",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "fastserver_logservices_content_id",
                table: "FastServer_LogServices_Content",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "fastserver_event_name",
                table: "FastServer_LogServices_Content",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            // Backfill UUIDs para registros existentes en Content
            migrationBuilder.Sql(
                @"UPDATE ""FastServer_LogServices_Content""
                  SET fastserver_logservices_content_id = gen_random_uuid()
                  WHERE fastserver_logservices_content_id = '00000000-0000-0000-0000-000000000000';");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogServices_Content_Historico",
                table: "FastServer_LogServices_Content_Historico",
                column: "fastserver_logservices_content_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogServices_Content",
                table: "FastServer_LogServices_Content",
                column: "fastserver_logservices_content_id");

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Content",
                columns: new[] { "fastserver_logservices_content_id", "fastserver_event_name", "fastserver_log_id", "fastserver_logservices_content_text", "fastserver_logservices_date", "fastserver_logservices_log_level", "fastserver_logservices_state" },
                values: new object[,]
                {
                    { new Guid("01965b00-0000-7000-8000-000000000001"), "AuthenticateUser", 1L, "{\"username\": \"admin\", \"timestamp\": \"2025-01-01T10:00:00Z\", \"ip\": \"192.168.1.100\"}", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "INFO", "SUCCESS" },
                    { new Guid("01965b00-0000-7000-8000-000000000002"), "SearchProducts", 2L, "{\"searchTerm\": \"laptop gaming\", \"resultsCount\": 15, \"executionTime\": \"320ms\"}", new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), "INFO", "SUCCESS" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Content_Historico",
                columns: new[] { "fastserver_logservices_content_id", "fastserver_event_name", "fastserver_log_id", "fastserver_logservices_content_text", "fastserver_logservices_date", "fastserver_logservices_log_level", "fastserver_logservices_state" },
                values: new object[,]
                {
                    { new Guid("01965b00-0000-7000-8000-000000000003"), "AuthenticateUser", 1L, "{\"username\": \"admin\", \"timestamp\": \"2024-12-15T10:00:00Z\", \"ip\": \"192.168.1.50\", \"archived\": true}", new DateTime(2024, 12, 15, 10, 0, 0, 0, DateTimeKind.Utc), "INFO", "SUCCESS" },
                    { new Guid("01965b00-0000-7000-8000-000000000004"), "SearchProducts", 2L, "{\"searchTerm\": \"gaming keyboard\", \"resultsCount\": 12, \"executionTime\": \"280ms\", \"archived\": true}", new DateTime(2024, 12, 15, 10, 10, 0, 0, DateTimeKind.Utc), "INFO", "SUCCESS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Content_Historico_LogId",
                table: "FastServer_LogServices_Content_Historico",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Content_LogId",
                table: "FastServer_LogServices_Content",
                column: "fastserver_log_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogServices_Content_Historico",
                table: "FastServer_LogServices_Content_Historico");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogServices_Content_Historico_LogId",
                table: "FastServer_LogServices_Content_Historico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogServices_Content",
                table: "FastServer_LogServices_Content");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogServices_Content_LogId",
                table: "FastServer_LogServices_Content");

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content",
                keyColumn: "fastserver_logservices_content_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965b00-0000-7000-8000-000000000001"));

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content",
                keyColumn: "fastserver_logservices_content_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965b00-0000-7000-8000-000000000002"));

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content_Historico",
                keyColumn: "fastserver_logservices_content_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965b00-0000-7000-8000-000000000003"));

            migrationBuilder.DeleteData(
                table: "FastServer_LogServices_Content_Historico",
                keyColumn: "fastserver_logservices_content_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965b00-0000-7000-8000-000000000004"));

            migrationBuilder.DropColumn(
                name: "fastserver_logservices_content_id",
                table: "FastServer_LogServices_Content_Historico");

            migrationBuilder.DropColumn(
                name: "fastserver_event_name",
                table: "FastServer_LogServices_Content_Historico");

            migrationBuilder.DropColumn(
                name: "fastserver_logservices_content_id",
                table: "FastServer_LogServices_Content");

            migrationBuilder.DropColumn(
                name: "fastserver_event_name",
                table: "FastServer_LogServices_Content");

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_logservices_state",
                table: "FastServer_LogServices_Content_Historico",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_logservices_date",
                table: "FastServer_LogServices_Content_Historico",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogServices_Content_Historico",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_logservices_state",
                table: "FastServer_LogServices_Content",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_logservices_date",
                table: "FastServer_LogServices_Content",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogServices_Content",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogServices_Content_Historico",
                table: "FastServer_LogServices_Content_Historico",
                column: "fastserver_log_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogServices_Content",
                table: "FastServer_LogServices_Content",
                column: "fastserver_log_id");

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Content",
                columns: new[] { "fastserver_log_id", "fastserver_logservices_content_text", "fastserver_logservices_date", "fastserver_logservices_log_level", "fastserver_logservices_state" },
                values: new object[,]
                {
                    { 1L, "{\"username\": \"admin\", \"timestamp\": \"2025-01-01T10:00:00Z\", \"ip\": \"192.168.1.100\"}", "2025-01-01 10:00:00", "INFO", "SUCCESS" },
                    { 2L, "{\"searchTerm\": \"laptop gaming\", \"resultsCount\": 15, \"executionTime\": \"320ms\"}", "2025-01-01 10:05:00", "INFO", "SUCCESS" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Content_Historico",
                columns: new[] { "fastserver_log_id", "fastserver_logservices_content_text", "fastserver_logservices_date", "fastserver_logservices_log_level", "fastserver_logservices_state" },
                values: new object[,]
                {
                    { 1L, "{\"username\": \"admin\", \"timestamp\": \"2024-12-15T10:00:00Z\", \"ip\": \"192.168.1.50\", \"archived\": true}", "2024-12-15 10:00:00", "INFO", "SUCCESS" },
                    { 2L, "{\"searchTerm\": \"gaming keyboard\", \"resultsCount\": 12, \"executionTime\": \"280ms\", \"archived\": true}", "2024-12-15 10:10:00", "INFO", "SUCCESS" }
                });
        }
    }
}
