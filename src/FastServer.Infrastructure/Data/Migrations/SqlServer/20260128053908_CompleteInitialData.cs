using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastServer.Infrastructure.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class CompleteInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "core_connector_credentials",
                columns: table => new
                {
                    core_connector_credential_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    core_connector_credential_user = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    core_connector_credential_pass = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    core_connector_credential_key = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_connector_credentials", x => x.core_connector_credential_id);
                });

            migrationBuilder.CreateTable(
                name: "event_types",
                columns: table => new
                {
                    event_type_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    event_type_description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_types", x => x.event_type_id);
                });

            migrationBuilder.CreateTable(
                name: "microservices_clusters",
                columns: table => new
                {
                    microservices_cluster_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    microservices_cluster_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    microservices_cluster_server_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    microservices_cluster_server_ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    microservices_cluster_active = table.Column<bool>(type: "bit", nullable: true),
                    microservices_cluster_deleted = table.Column<bool>(type: "bit", nullable: true),
                    delete_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_microservices_clusters", x => x.microservices_cluster_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_peoplesoft = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    user_active = table.Column<bool>(type: "bit", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    user_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "microservice_registers",
                columns: table => new
                {
                    microservice_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    microservice_cluster_id = table.Column<long>(type: "bigint", nullable: true),
                    microservice_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    microservice_active = table.Column<bool>(type: "bit", nullable: true),
                    microservice_deleted = table.Column<bool>(type: "bit", nullable: true),
                    microservice_core_connection = table.Column<bool>(type: "bit", nullable: true),
                    delete_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_microservice_registers", x => x.microservice_id);
                    table.ForeignKey(
                        name: "FK_microservice_registers_microservices_clusters_microservice_cluster_id",
                        column: x => x.microservice_cluster_id,
                        principalTable: "microservices_clusters",
                        principalColumn: "microservices_cluster_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activity_logs",
                columns: table => new
                {
                    activity_log_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_type_id = table.Column<long>(type: "bigint", nullable: true),
                    activity_log_entity_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    activity_log_entity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    activity_log_description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_logs", x => x.activity_log_id);
                    table.ForeignKey(
                        name: "FK_activity_logs_event_types_event_type_id",
                        column: x => x.event_type_id,
                        principalTable: "event_types",
                        principalColumn: "event_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_activity_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "microservice_core_connector",
                columns: table => new
                {
                    microservice_core_connector_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    core_connector_credential_id = table.Column<long>(type: "bigint", nullable: true),
                    microservice_id = table.Column<long>(type: "bigint", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_microservice_core_connector", x => x.microservice_core_connector_id);
                    table.ForeignKey(
                        name: "FK_microservice_core_connector_core_connector_credentials_core_connector_credential_id",
                        column: x => x.core_connector_credential_id,
                        principalTable: "core_connector_credentials",
                        principalColumn: "core_connector_credential_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_microservice_core_connector_microservice_registers_microservice_id",
                        column: x => x.microservice_id,
                        principalTable: "microservice_registers",
                        principalColumn: "microservice_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "microservice_methods",
                columns: table => new
                {
                    microservice_method_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    microservice_id = table.Column<long>(type: "bigint", nullable: false),
                    microservice_method_delete = table.Column<bool>(type: "bit", nullable: true),
                    microservice_method_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    microservice_method_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modify_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_microservice_methods", x => x.microservice_method_id);
                    table.ForeignKey(
                        name: "FK_microservice_methods_microservice_registers_microservice_id",
                        column: x => x.microservice_id,
                        principalTable: "microservice_registers",
                        principalColumn: "microservice_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "core_connector_credentials",
                columns: new[] { "core_connector_credential_id", "core_connector_credential_key", "core_connector_credential_pass", "core_connector_credential_user", "create_at", "modify_at" },
                values: new object[,]
                {
                    { 1L, "api_key_001", "encrypted_pass_001", "auth_service_user", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, "api_key_002", "encrypted_pass_002", "product_service_user", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "event_types",
                columns: new[] { "event_type_id", "create_at", "event_type_description", "modify_at" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Microservice Registration", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Configuration Change", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "microservices_clusters",
                columns: new[] { "microservices_cluster_id", "create_at", "delete_at", "microservices_cluster_active", "microservices_cluster_deleted", "microservices_cluster_name", "microservices_cluster_server_ip", "microservices_cluster_server_name", "modify_at" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Production Cluster", "10.0.1.100", "prod-cluster-01", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Development Cluster", "10.0.2.100", "dev-cluster-01", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "create_at", "modify_at", "user_active", "user_email", "user_name", "user_peoplesoft" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "admin@fastserver.com", "Admin User", "PS001" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "developer@fastserver.com", "Developer User", "PS002" }
                });

            migrationBuilder.InsertData(
                table: "activity_logs",
                columns: new[] { "activity_log_id", "activity_log_description", "activity_log_entity_id", "activity_log_entity_name", "create_at", "event_type_id", "modify_at", "user_id" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "AuthService registered successfully", new Guid("20000000-0000-0000-0000-000000000001"), "MicroserviceRegister", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "ProductService configuration updated", new Guid("20000000-0000-0000-0000-000000000002"), "MicroserviceRegister", new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), 2L, new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "microservice_registers",
                columns: new[] { "microservice_id", "create_at", "delete_at", "microservice_active", "microservice_cluster_id", "microservice_core_connection", "microservice_deleted", "microservice_name", "modify_at" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, 1L, true, false, "AuthService", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, 1L, true, false, "ProductService", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "microservice_core_connector",
                columns: new[] { "microservice_core_connector_id", "core_connector_credential_id", "create_at", "microservice_id", "modify_at" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, 2L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "microservice_methods",
                columns: new[] { "microservice_method_id", "create_at", "microservice_id", "microservice_method_delete", "microservice_method_name", "microservice_method_url", "modify_at" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1L, false, "AuthenticateUser", "/api/users/authenticate", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2L, false, "SearchProducts", "/api/products/search", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_activity_logs_activity_log_entity_name",
                table: "activity_logs",
                column: "activity_log_entity_name");

            migrationBuilder.CreateIndex(
                name: "IX_activity_logs_create_at",
                table: "activity_logs",
                column: "create_at");

            migrationBuilder.CreateIndex(
                name: "IX_activity_logs_event_type_id",
                table: "activity_logs",
                column: "event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_activity_logs_user_id",
                table: "activity_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_core_connector_credentials_core_connector_credential_user",
                table: "core_connector_credentials",
                column: "core_connector_credential_user");

            migrationBuilder.CreateIndex(
                name: "IX_event_types_event_type_description",
                table: "event_types",
                column: "event_type_description");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_core_connector_core_connector_credential_id",
                table: "microservice_core_connector",
                column: "core_connector_credential_id");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_core_connector_microservice_id",
                table: "microservice_core_connector",
                column: "microservice_id");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_methods_microservice_id",
                table: "microservice_methods",
                column: "microservice_id");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_methods_microservice_method_delete",
                table: "microservice_methods",
                column: "microservice_method_delete");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_methods_microservice_method_name",
                table: "microservice_methods",
                column: "microservice_method_name");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_registers_microservice_active",
                table: "microservice_registers",
                column: "microservice_active");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_registers_microservice_cluster_id",
                table: "microservice_registers",
                column: "microservice_cluster_id");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_registers_microservice_deleted",
                table: "microservice_registers",
                column: "microservice_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_registers_microservice_name",
                table: "microservice_registers",
                column: "microservice_name");

            migrationBuilder.CreateIndex(
                name: "IX_microservices_clusters_microservices_cluster_active",
                table: "microservices_clusters",
                column: "microservices_cluster_active");

            migrationBuilder.CreateIndex(
                name: "IX_microservices_clusters_microservices_cluster_deleted",
                table: "microservices_clusters",
                column: "microservices_cluster_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_microservices_clusters_microservices_cluster_name",
                table: "microservices_clusters",
                column: "microservices_cluster_name");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_active",
                table: "users",
                column: "user_active");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_email",
                table: "users",
                column: "user_email",
                unique: true,
                filter: "[user_email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_peoplesoft",
                table: "users",
                column: "user_peoplesoft");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_logs");

            migrationBuilder.DropTable(
                name: "microservice_core_connector");

            migrationBuilder.DropTable(
                name: "microservice_methods");

            migrationBuilder.DropTable(
                name: "event_types");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "core_connector_credentials");

            migrationBuilder.DropTable(
                name: "microservice_registers");

            migrationBuilder.DropTable(
                name: "microservices_clusters");
        }
    }
}
