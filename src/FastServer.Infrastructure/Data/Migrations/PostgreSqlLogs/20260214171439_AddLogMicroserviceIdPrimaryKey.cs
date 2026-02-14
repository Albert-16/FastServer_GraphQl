using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlLogs
{
    /// <inheritdoc />
    public partial class AddLogMicroserviceIdPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogMicroservice_Historico",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogMicroservice",
                table: "FastServer_LogMicroservice");

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice",
                keyColumn: "fastserver_log_id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice",
                keyColumn: "fastserver_log_id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 2L);

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogMicroservice_Historico",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "fastserver_log_microservice_id",
                table: "FastServer_LogMicroservice_Historico",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "fastserver_event_name",
                table: "FastServer_LogMicroservice_Historico",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice_Historico",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogMicroservice",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "fastserver_log_microservice_id",
                table: "FastServer_LogMicroservice",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "fastserver_event_name",
                table: "FastServer_LogMicroservice",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            // Generar UUIDs únicos para registros existentes antes de crear la PK
            migrationBuilder.Sql(
                "UPDATE \"FastServer_LogMicroservice\" SET fastserver_log_microservice_id = gen_random_uuid() WHERE fastserver_log_microservice_id = '00000000-0000-0000-0000-000000000000';");
            migrationBuilder.Sql(
                "UPDATE \"FastServer_LogMicroservice_Historico\" SET fastserver_log_microservice_id = gen_random_uuid() WHERE fastserver_log_microservice_id = '00000000-0000-0000-0000-000000000000';");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogMicroservice_Historico",
                table: "FastServer_LogMicroservice_Historico",
                column: "fastserver_log_microservice_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogMicroservice",
                table: "FastServer_LogMicroservice",
                column: "fastserver_log_microservice_id");

            migrationBuilder.InsertData(
                table: "FastServer_LogMicroservice",
                columns: new[] { "fastserver_log_microservice_id", "fastserver_event_name", "fastserver_log_date", "fastserver_log_id", "fastserver_log_level", "fastserver_logmicroservice_text", "fastserver_request_id" },
                values: new object[,]
                {
                    { new Guid("01965a00-0000-7000-8000-000000000001"), "AuthenticateUser", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "INFO", "[AuthService] Inicio de autenticación para usuario: admin", 1001L },
                    { new Guid("01965a00-0000-7000-8000-000000000002"), "SearchProducts", new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), 2L, "INFO", "[ProductService] Búsqueda ejecutada: 'laptop gaming' - 15 resultados encontrados", 1002L }
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogMicroservice_Historico",
                columns: new[] { "fastserver_log_microservice_id", "fastserver_event_name", "fastserver_log_date", "fastserver_log_id", "fastserver_log_level", "fastserver_logmicroservice_text", "fastserver_request_id" },
                values: new object[,]
                {
                    { new Guid("01965a00-0000-7000-8000-000000000003"), "AuthenticateUser", new DateTime(2024, 12, 15, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "INFO", "[AuthService] Autenticación histórica exitosa para usuario: admin", 2001L },
                    { new Guid("01965a00-0000-7000-8000-000000000004"), "SearchProducts", new DateTime(2024, 12, 15, 10, 10, 0, 0, DateTimeKind.Utc), 2L, "INFO", "[ProductService] Búsqueda histórica ejecutada: 'gaming keyboard' - 12 resultados", 2002L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogMicroservice_Historico_LogId",
                table: "FastServer_LogMicroservice_Historico",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogMicroservice_LogId",
                table: "FastServer_LogMicroservice",
                column: "fastserver_log_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogMicroservice_Historico",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogMicroservice_Historico_LogId",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FastServer_LogMicroservice",
                table: "FastServer_LogMicroservice");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogMicroservice_LogId",
                table: "FastServer_LogMicroservice");

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice",
                keyColumn: "fastserver_log_microservice_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000001"));

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice",
                keyColumn: "fastserver_log_microservice_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000002"));

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice_Historico",
                keyColumn: "fastserver_log_microservice_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000003"));

            migrationBuilder.DeleteData(
                table: "FastServer_LogMicroservice_Historico",
                keyColumn: "fastserver_log_microservice_id",
                keyColumnType: "uuid",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000004"));

            migrationBuilder.DropColumn(
                name: "fastserver_log_microservice_id",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropColumn(
                name: "fastserver_event_name",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropColumn(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropColumn(
                name: "fastserver_log_microservice_id",
                table: "FastServer_LogMicroservice");

            migrationBuilder.DropColumn(
                name: "fastserver_event_name",
                table: "FastServer_LogMicroservice");

            migrationBuilder.DropColumn(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice");

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogMicroservice_Historico",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_id",
                table: "FastServer_LogMicroservice",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogMicroservice_Historico",
                table: "FastServer_LogMicroservice_Historico",
                column: "fastserver_log_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastServer_LogMicroservice",
                table: "FastServer_LogMicroservice",
                column: "fastserver_log_id");

            migrationBuilder.InsertData(
                table: "FastServer_LogMicroservice",
                columns: new[] { "fastserver_log_id", "fastserver_log_date", "fastserver_log_level", "fastserver_logmicroservice_text" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "INFO", "[AuthService] Inicio de autenticación para usuario: admin" },
                    { 2L, new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), "INFO", "[ProductService] Búsqueda ejecutada: 'laptop gaming' - 15 resultados encontrados" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogMicroservice_Historico",
                columns: new[] { "fastserver_log_id", "fastserver_log_date", "fastserver_log_level", "fastserver_logmicroservice_text" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 12, 15, 10, 0, 0, 0, DateTimeKind.Utc), "INFO", "[AuthService] Autenticación histórica exitosa para usuario: admin" },
                    { 2L, new DateTime(2024, 12, 15, 10, 10, 0, 0, DateTimeKind.Utc), "INFO", "[ProductService] Búsqueda histórica ejecutada: 'gaming keyboard' - 12 resultados" }
                });
        }
    }
}
