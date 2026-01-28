using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastServer.Infrastructure.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class CompleteInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FastServer_LogMicroservice",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fastserver_log_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_log_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_logmicroservice_text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogMicroservice", x => x.fastserver_log_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogMicroservice_Historico",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fastserver_log_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_log_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_logmicroservice_text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogMicroservice_Historico", x => x.fastserver_log_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogServices_Content",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fastserver_logservices_date = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_logservices_log_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_logservices_state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_logservices_content_text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogServices_Content", x => x.fastserver_log_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogServices_Content_Historico",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fastserver_logservices_date = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_logservices_log_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_logservices_state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_logservices_content_text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogServices_Content_Historico", x => x.fastserver_log_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogServices_Header",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fastserver_log_date_in = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fastserver_log_date_out = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fastserver_log_state = table.Column<string>(type: "text", nullable: false),
                    fastserver_log_method_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    fastserver_log_method_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_log_fs_id = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_method_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    fastserver_tci_ip_port = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_error_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_error_description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    fastserver_ip_fs = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_type_process = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_log_nodo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_http_method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fastserver_microservice_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_request_duration = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_request_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogServices_Header", x => x.fastserver_log_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogServices_Header_Historico",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fastserver_log_date_in = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fastserver_log_date_out = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fastserver_log_state = table.Column<string>(type: "text", nullable: false),
                    fastserver_log_method_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    fastserver_log_method_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_log_fs_id = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_method_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    fastserver_tci_ip_port = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_error_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_error_description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    fastserver_ip_fs = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_type_process = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_log_nodo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_http_method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fastserver_microservice_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_request_duration = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_request_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogServices_Header_Historico", x => x.fastserver_log_id);
                });

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

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Header",
                columns: new[] { "fastserver_log_id", "fastserver_error_code", "fastserver_error_description", "fastserver_http_method", "fastserver_ip_fs", "fastserver_log_date_in", "fastserver_log_date_out", "fastserver_log_fs_id", "fastserver_log_method_name", "fastserver_log_method_url", "fastserver_log_nodo", "fastserver_log_state", "fastserver_method_description", "fastserver_microservice_name", "fastserver_request_duration", "fastserver_request_id", "fastserver_session_id", "fastserver_tci_ip_port", "fastserver_transaction_id", "fastserver_type_process", "fastserver_user_id" },
                values: new object[,]
                {
                    { 1L, null, null, "POST", "192.168.1.100", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 150, DateTimeKind.Utc), null, "AuthenticateUser", "/api/users/authenticate", null, "Completed", null, "AuthService", 150L, null, "SES-001", null, "TRX-001-2025", "Authentication", "admin" },
                    { 2L, null, null, "GET", "192.168.1.101", new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 5, 0, 320, DateTimeKind.Utc), null, "SearchProducts", "/api/products/search", null, "Completed", null, "ProductService", 320L, null, "SES-002", null, "TRX-002-2025", "Query", "user001" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Header_Historico",
                columns: new[] { "fastserver_log_id", "fastserver_error_code", "fastserver_error_description", "fastserver_http_method", "fastserver_ip_fs", "fastserver_log_date_in", "fastserver_log_date_out", "fastserver_log_fs_id", "fastserver_log_method_name", "fastserver_log_method_url", "fastserver_log_nodo", "fastserver_log_state", "fastserver_method_description", "fastserver_microservice_name", "fastserver_request_duration", "fastserver_request_id", "fastserver_session_id", "fastserver_tci_ip_port", "fastserver_transaction_id", "fastserver_type_process", "fastserver_user_id" },
                values: new object[,]
                {
                    { 1L, null, null, "POST", "192.168.1.50", new DateTime(2024, 12, 15, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 15, 10, 0, 0, 145, DateTimeKind.Utc), null, "AuthenticateUser", "/api/users/authenticate", null, "Completed", null, "AuthService", 145L, null, "SES-HIST-001", null, "TRX-HIST-001-2024", "Authentication", "admin" },
                    { 2L, null, null, "GET", "192.168.1.51", new DateTime(2024, 12, 15, 10, 10, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 15, 10, 10, 0, 280, DateTimeKind.Utc), null, "SearchProducts", "/api/products/search", null, "Completed", null, "ProductService", 280L, null, "SES-HIST-002", null, "TRX-HIST-002-2024", "Query", "user001" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogMicroservice_fastserver_log_id",
                table: "FastServer_LogMicroservice",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogMicroservice_Historico_fastserver_log_id",
                table: "FastServer_LogMicroservice_Historico",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Content_fastserver_log_id",
                table: "FastServer_LogServices_Content",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Content_Historico_fastserver_log_id",
                table: "FastServer_LogServices_Content_Historico",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_fastserver_log_date_in",
                table: "FastServer_LogServices_Header",
                column: "fastserver_log_date_in");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_fastserver_log_state",
                table: "FastServer_LogServices_Header",
                column: "fastserver_log_state");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_fastserver_microservice_name",
                table: "FastServer_LogServices_Header",
                column: "fastserver_microservice_name");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_fastserver_transaction_id",
                table: "FastServer_LogServices_Header",
                column: "fastserver_transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_fastserver_user_id",
                table: "FastServer_LogServices_Header",
                column: "fastserver_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_Historico_fastserver_log_date~",
                table: "FastServer_LogServices_Header_Historico",
                column: "fastserver_log_date_in");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_Historico_fastserver_log_state",
                table: "FastServer_LogServices_Header_Historico",
                column: "fastserver_log_state");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FastServer_LogMicroservice");

            migrationBuilder.DropTable(
                name: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropTable(
                name: "FastServer_LogServices_Content");

            migrationBuilder.DropTable(
                name: "FastServer_LogServices_Content_Historico");

            migrationBuilder.DropTable(
                name: "FastServer_LogServices_Header");

            migrationBuilder.DropTable(
                name: "FastServer_LogServices_Header_Historico");
        }
    }
}
