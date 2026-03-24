using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlMicroservices
{
    /// <inheritdoc />
    public partial class PostgreSqlMicroservicesInitialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FastServer_Cluster",
                columns: table => new
                {
                    fastserver_cluster_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_cluster_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    fastserver_cluster_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_cluster_version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_cluster_server_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    fastserver_cluster_server_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_cluster_active = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_cluster_delete = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_create_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    fastserver_delete_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_Cluster", x => x.fastserver_cluster_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_CoreConnector_Credential",
                columns: table => new
                {
                    fastserver_core_connector_credential_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_core_connector_credential_user = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_core_connector_credential_pass = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_core_connector_credential_key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_CoreConnector_Credential", x => x.fastserver_core_connector_credential_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_EventType",
                columns: table => new
                {
                    fastserver_event_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_event_type_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_EventType", x => x.fastserver_event_type_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_Microservices_Cluster",
                columns: table => new
                {
                    fastserver_microservices_cluster_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_microservices_cluster_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_microservices_cluster_server_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_microservices_cluster_server_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_microservices_cluster_protocol = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    fastserver_microservices_cluster_active = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_microservices_cluster_deleted = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_delete_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_Microservices_Cluster", x => x.fastserver_microservices_cluster_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_Microservices_RegisterType",
                columns: table => new
                {
                    fastserver_microservices_register_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_microservices_register_type_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_microservices_register_type_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_Microservices_RegisterType", x => x.fastserver_microservices_register_type_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_User",
                columns: table => new
                {
                    fastserver_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_user_peoplesoft = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fastserver_user_active = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_user_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_user_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_password_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_password_changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_email_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fastserver_refresh_token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_refresh_token_expiry_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_User", x => x.fastserver_user_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_Microservice_Register",
                columns: table => new
                {
                    fastserver_microservice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_microservice_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_microservice_active = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_microservice_deleted = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_microservice_core_connection = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_soap_base = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    fastserver_microservice_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    fastserver_delete_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_Microservice_Register", x => x.fastserver_microservice_id);
                    table.ForeignKey(
                        name: "FK_FastServer_Microservice_Register_FastServer_Microservices_R~",
                        column: x => x.fastserver_microservice_type_id,
                        principalTable: "FastServer_Microservices_RegisterType",
                        principalColumn: "fastserver_microservices_register_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_ActivityLog",
                columns: table => new
                {
                    fastserver_activity_log_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_event_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    fastserver_activity_log_entity_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_activity_log_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    fastserver_activity_log_description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    fastserver_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_ActivityLog", x => x.fastserver_activity_log_id);
                    table.ForeignKey(
                        name: "FK_FastServer_ActivityLog_FastServer_EventType_fastserver_even~",
                        column: x => x.fastserver_event_type_id,
                        principalTable: "FastServer_EventType",
                        principalColumn: "fastserver_event_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FastServer_ActivityLog_FastServer_User_fastserver_user_id",
                        column: x => x.fastserver_user_id,
                        principalTable: "FastServer_User",
                        principalColumn: "fastserver_user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_Microservice_CoreConnector",
                columns: table => new
                {
                    fastserver_microservice_core_connector_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_core_connector_credential_id = table.Column<Guid>(type: "uuid", nullable: true),
                    fastserver_microservice_id = table.Column<Guid>(type: "uuid", nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_Microservice_CoreConnector", x => x.fastserver_microservice_core_connector_id);
                    table.ForeignKey(
                        name: "FK_FastServer_Microservice_CoreConnector_FastServer_CoreConnec~",
                        column: x => x.fastserver_core_connector_credential_id,
                        principalTable: "FastServer_CoreConnector_Credential",
                        principalColumn: "fastserver_core_connector_credential_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FastServer_Microservice_CoreConnector_FastServer_Microservi~",
                        column: x => x.fastserver_microservice_id,
                        principalTable: "FastServer_Microservice_Register",
                        principalColumn: "fastserver_microservice_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_Microservice_Method",
                columns: table => new
                {
                    fastserver_microservice_method_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_microservice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_microservice_method_delete = table.Column<bool>(type: "boolean", nullable: true),
                    fastserver_microservice_method_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fastserver_microservice_method_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fastserver_http_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_Microservice_Method", x => x.fastserver_microservice_method_id);
                    table.ForeignKey(
                        name: "FK_FastServer_Microservice_Method_FastServer_Microservice_Regi~",
                        column: x => x.fastserver_microservice_id,
                        principalTable: "FastServer_Microservice_Register",
                        principalColumn: "fastserver_microservice_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_Nodo",
                columns: table => new
                {
                    fastserver_nodo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_microservice_method_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_microservices_cluster_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fastserver_create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fastserver_modify_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_Nodo", x => x.fastserver_nodo_id);
                    table.ForeignKey(
                        name: "FK_FastServer_Nodo_FastServer_Microservice_Method_fastserver_m~",
                        column: x => x.fastserver_microservice_method_id,
                        principalTable: "FastServer_Microservice_Method",
                        principalColumn: "fastserver_microservice_method_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FastServer_Nodo_FastServer_Microservices_Cluster_fastserver~",
                        column: x => x.fastserver_microservices_cluster_id,
                        principalTable: "FastServer_Microservices_Cluster",
                        principalColumn: "fastserver_microservices_cluster_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FastServer_Cluster",
                columns: new[] { "fastserver_cluster_id", "fastserver_create_at", "fastserver_delete_at", "fastserver_cluster_active", "fastserver_cluster_delete", "fastserver_cluster_name", "fastserver_cluster_server_ip", "fastserver_cluster_server_name", "fastserver_cluster_url", "fastserver_cluster_version", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, "FS Production Cluster", "10.10.1.100", "fs-prod-node-01", "https://fs-prod.davivienda.hn:8443", "1.0.0", new DateTimeOffset(new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new DateTimeOffset(new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, "FS Development Cluster", "10.10.2.100", "fs-dev-node-01", "https://fs-dev.davivienda.hn:8443", "1.0.0-dev", new DateTimeOffset(new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "FastServer_CoreConnector_Credential",
                columns: new[] { "fastserver_core_connector_credential_id", "fastserver_core_connector_credential_key", "fastserver_core_connector_credential_pass", "fastserver_core_connector_credential_user", "fastserver_create_at", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("ff000000-0000-0000-0000-000000000001"), "api_key_001", "encrypted_pass_001", "auth_service_user", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ff000000-0000-0000-0000-000000000002"), "api_key_002", "encrypted_pass_002", "product_service_user", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "FastServer_EventType",
                columns: new[] { "fastserver_event_type_id", "fastserver_create_at", "fastserver_event_type_description", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("ee000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Microservice Registration", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ee000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Configuration Change", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "FastServer_Microservices_Cluster",
                columns: new[] { "fastserver_microservices_cluster_id", "fastserver_create_at", "fastserver_delete_at", "fastserver_microservices_cluster_active", "fastserver_microservices_cluster_deleted", "fastserver_microservices_cluster_name", "fastserver_microservices_cluster_protocol", "fastserver_microservices_cluster_server_ip", "fastserver_microservices_cluster_server_name", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("bb000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Production Cluster", "HTTPS", "10.0.1.100", "prod-cluster-01", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bb000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Development Cluster", "HTTP", "10.0.2.100", "dev-cluster-01", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "FastServer_Microservices_RegisterType",
                columns: new[] { "fastserver_microservices_register_type_id", "fastserver_create_at", "fastserver_microservices_register_type_description", "fastserver_microservices_register_type_name", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("aa000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Microservicio basado en API REST", "REST", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("aa000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Microservicio basado en SOAP/XML", "SOAP", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "FastServer_User",
                columns: new[] { "fastserver_user_id", "fastserver_create_at", "fastserver_email_confirmed", "fastserver_last_login", "fastserver_modify_at", "fastserver_password_changed_at", "fastserver_password_hash", "fastserver_refresh_token", "fastserver_refresh_token_expiry_time", "fastserver_user_active", "fastserver_user_email", "fastserver_user_name", "fastserver_user_peoplesoft" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, null, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy", null, null, true, "admin@fastserver.com", "Admin User", "PS001" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, null, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, "$2a$11$VRjNO0ZRwK7x1Z.XfJcKAOKs7ggzwhPB3QVpLp2PF3cxyMq7R5rHu", null, null, true, "developer@fastserver.com", "Developer User", "PS002" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_ActivityLog",
                columns: new[] { "fastserver_activity_log_id", "fastserver_activity_log_description", "fastserver_activity_log_entity_id", "fastserver_activity_log_entity_name", "fastserver_create_at", "fastserver_event_type_id", "fastserver_modify_at", "fastserver_user_id" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "AuthService registered successfully", new Guid("20000000-0000-0000-0000-000000000001"), "MicroserviceRegister", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("ee000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "ProductService configuration updated", new Guid("20000000-0000-0000-0000-000000000002"), "MicroserviceRegister", new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), new Guid("ee000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "FastServer_Microservice_Register",
                columns: new[] { "fastserver_microservice_id", "fastserver_create_at", "fastserver_delete_at", "fastserver_microservice_active", "fastserver_microservice_core_connection", "fastserver_microservice_deleted", "fastserver_microservice_name", "fastserver_microservice_type_id", "fastserver_modify_at", "fastserver_soap_base" },
                values: new object[,]
                {
                    { new Guid("cc000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, true, false, "AuthService", new Guid("aa000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("cc000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, true, true, false, "ProductService", new Guid("aa000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "https://core.davivienda.hn/soap/products" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_Microservice_CoreConnector",
                columns: new[] { "fastserver_microservice_core_connector_id", "fastserver_core_connector_credential_id", "fastserver_create_at", "fastserver_microservice_id", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("11000000-0000-0000-0000-000000000001"), new Guid("ff000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("cc000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11000000-0000-0000-0000-000000000002"), new Guid("ff000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("cc000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "FastServer_Microservice_Method",
                columns: new[] { "fastserver_microservice_method_id", "fastserver_create_at", "fastserver_http_method", "fastserver_microservice_id", "fastserver_microservice_method_delete", "fastserver_microservice_method_name", "fastserver_microservice_method_url", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("dd000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "POST", new Guid("cc000000-0000-0000-0000-000000000001"), false, "AuthenticateUser", "/api/users/authenticate", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("dd000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "GET", new Guid("cc000000-0000-0000-0000-000000000002"), false, "SearchProducts", "/api/products/search", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "FastServer_Nodo",
                columns: new[] { "fastserver_nodo_id", "fastserver_create_at", "fastserver_microservice_method_id", "fastserver_microservices_cluster_id", "fastserver_modify_at" },
                values: new object[,]
                {
                    { new Guid("22000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("dd000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22000000-0000-0000-0000-000000000002"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("dd000000-0000-0000-0000-000000000002"), new Guid("bb000000-0000-0000-0000-000000000001"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_ActivityLog_CreateAt",
                table: "FastServer_ActivityLog",
                column: "fastserver_create_at");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_ActivityLog_EntityName",
                table: "FastServer_ActivityLog",
                column: "fastserver_activity_log_entity_name");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_ActivityLog_EventTypeId",
                table: "FastServer_ActivityLog",
                column: "fastserver_event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_ActivityLog_UserId",
                table: "FastServer_ActivityLog",
                column: "fastserver_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_ActivityLog_UserId_CreateAt_Desc",
                table: "FastServer_ActivityLog",
                columns: new[] { "fastserver_user_id", "fastserver_create_at" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Cluster_Name",
                table: "FastServer_Cluster",
                column: "fastserver_cluster_name");

            migrationBuilder.CreateIndex(
                name: "UX_FastServer_CoreConnector_Credential_User",
                table: "FastServer_CoreConnector_Credential",
                column: "fastserver_core_connector_credential_user",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_EventType_Description",
                table: "FastServer_EventType",
                column: "fastserver_event_type_description");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservice_CoreConnector_CredentialId",
                table: "FastServer_Microservice_CoreConnector",
                column: "fastserver_core_connector_credential_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservice_CoreConnector_MicroserviceId",
                table: "FastServer_Microservice_CoreConnector",
                column: "fastserver_microservice_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservice_Method_MicroserviceId",
                table: "FastServer_Microservice_Method",
                column: "fastserver_microservice_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservice_Method_Name",
                table: "FastServer_Microservice_Method",
                column: "fastserver_microservice_method_name");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservice_Register_Name",
                table: "FastServer_Microservice_Register",
                column: "fastserver_microservice_name");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservice_Register_TypeId",
                table: "FastServer_Microservice_Register",
                column: "fastserver_microservice_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservices_Cluster_Name",
                table: "FastServer_Microservices_Cluster",
                column: "fastserver_microservices_cluster_name");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservices_RegisterType_Name",
                table: "FastServer_Microservices_RegisterType",
                column: "fastserver_microservices_register_type_name");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Nodo_ClusterId",
                table: "FastServer_Nodo",
                column: "fastserver_microservices_cluster_id");

            migrationBuilder.CreateIndex(
                name: "UX_FastServer_Nodo_Method_Cluster",
                table: "FastServer_Nodo",
                columns: new[] { "fastserver_microservice_method_id", "fastserver_microservices_cluster_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_User_Peoplesoft",
                table: "FastServer_User",
                column: "fastserver_user_peoplesoft");

            migrationBuilder.CreateIndex(
                name: "UX_FastServer_User_Email",
                table: "FastServer_User",
                column: "fastserver_user_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FastServer_ActivityLog");

            migrationBuilder.DropTable(
                name: "FastServer_Cluster");

            migrationBuilder.DropTable(
                name: "FastServer_Microservice_CoreConnector");

            migrationBuilder.DropTable(
                name: "FastServer_Nodo");

            migrationBuilder.DropTable(
                name: "FastServer_EventType");

            migrationBuilder.DropTable(
                name: "FastServer_User");

            migrationBuilder.DropTable(
                name: "FastServer_CoreConnector_Credential");

            migrationBuilder.DropTable(
                name: "FastServer_Microservice_Method");

            migrationBuilder.DropTable(
                name: "FastServer_Microservices_Cluster");

            migrationBuilder.DropTable(
                name: "FastServer_Microservice_Register");

            migrationBuilder.DropTable(
                name: "FastServer_Microservices_RegisterType");
        }
    }
}
