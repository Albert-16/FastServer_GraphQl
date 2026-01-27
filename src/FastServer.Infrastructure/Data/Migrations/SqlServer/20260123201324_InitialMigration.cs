using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastServer.Infrastructure.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FastServer_LogServices_Header",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fastserver_log_date_in = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fastserver_log_date_out = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fastserver_log_state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fastserver_log_method_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fastserver_log_method_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fastserver_log_fs_id = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_method_description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    fastserver_tci_ip_port = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_error_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fastserver_error_description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    fastserver_ip_fs = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fastserver_type_process = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_log_nodo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_http_method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fastserver_microservice_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fastserver_request_duration = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_transaction_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_user_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_session_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fastserver_log_date_in = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fastserver_log_date_out = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fastserver_log_state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fastserver_log_method_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fastserver_log_method_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fastserver_log_fs_id = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_method_description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    fastserver_tci_ip_port = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_error_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fastserver_error_description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    fastserver_ip_fs = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fastserver_type_process = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_log_nodo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_http_method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fastserver_microservice_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fastserver_request_duration = table.Column<long>(type: "bigint", nullable: true),
                    fastserver_transaction_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_user_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_session_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fastserver_request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogServices_Header_Historico", x => x.fastserver_log_id);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogMicroservice",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false),
                    fastserver_logmicroservice_text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogMicroservice", x => x.fastserver_log_id);
                    table.ForeignKey(
                        name: "FK_FastServer_LogMicroservice_FastServer_LogServices_Header_fastserver_log_id",
                        column: x => x.fastserver_log_id,
                        principalTable: "FastServer_LogServices_Header",
                        principalColumn: "fastserver_log_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogServices_Content",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false),
                    fastserver_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fastserver_logservices_content_text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogServices_Content", x => new { x.fastserver_log_id, x.fastserver_no });
                    table.ForeignKey(
                        name: "FK_FastServer_LogServices_Content_FastServer_LogServices_Header_fastserver_log_id",
                        column: x => x.fastserver_log_id,
                        principalTable: "FastServer_LogServices_Header",
                        principalColumn: "fastserver_log_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogMicroservice_Historico",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false),
                    fastserver_logmicroservice_text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogMicroservice_Historico", x => x.fastserver_log_id);
                    table.ForeignKey(
                        name: "FK_FastServer_LogMicroservice_Historico_FastServer_LogServices_Header_Historico_fastserver_log_id",
                        column: x => x.fastserver_log_id,
                        principalTable: "FastServer_LogServices_Header_Historico",
                        principalColumn: "fastserver_log_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FastServer_LogServices_Content_Historico",
                columns: table => new
                {
                    fastserver_log_id = table.Column<long>(type: "bigint", nullable: false),
                    fastserver_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fastserver_logservices_content_text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastServer_LogServices_Content_Historico", x => new { x.fastserver_log_id, x.fastserver_no });
                    table.ForeignKey(
                        name: "FK_FastServer_LogServices_Content_Historico_FastServer_LogServices_Header_Historico_fastserver_log_id",
                        column: x => x.fastserver_log_id,
                        principalTable: "FastServer_LogServices_Header_Historico",
                        principalColumn: "fastserver_log_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Header",
                columns: new[] { "fastserver_log_id", "fastserver_error_code", "fastserver_error_description", "fastserver_http_method", "fastserver_ip_fs", "fastserver_log_date_in", "fastserver_log_date_out", "fastserver_log_fs_id", "fastserver_log_method_name", "fastserver_log_method_url", "fastserver_log_nodo", "fastserver_log_state", "fastserver_method_description", "fastserver_microservice_name", "fastserver_request_duration", "fastserver_request_id", "fastserver_session_id", "fastserver_tci_ip_port", "fastserver_transaction_id", "fastserver_type_process", "fastserver_user_id" },
                values: new object[,]
                {
                    { 1L, null, null, "POST", "192.168.1.100", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, 150, DateTimeKind.Utc), null, "AuthenticateUser", "/api/users/authenticate", null, "Completed", null, "AuthService", 150L, null, "SES-001", null, "TRX-001-2025", "Authentication", "admin" },
                    { 2L, null, null, "GET", "192.168.1.101", new DateTime(2025, 1, 1, 10, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 5, 0, 85, DateTimeKind.Utc), null, "GetCustomerList", "/api/customers/list", null, "Completed", null, "CustomerService", 85L, null, "SES-002", null, "TRX-002-2025", "Query", "user001" },
                    { 3L, "ORD-500", "Error de conexión con el servicio de inventario", "POST", "192.168.1.102", new DateTime(2025, 1, 1, 10, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 10, 5, 200, DateTimeKind.Utc), null, "CreateOrder", "/api/orders/create", null, "Failed", null, "OrderService", 5200L, null, "SES-003", null, "TRX-003-2025", "Transaction", "user002" },
                    { 4L, null, null, "GET", "192.168.1.103", new DateTime(2025, 1, 1, 10, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 15, 0, 320, DateTimeKind.Utc), null, "SearchProducts", "/api/products/search", null, "Completed", null, "ProductService", 320L, null, "SES-004", null, "TRX-004-2025", "Query", "user003" },
                    { 5L, "RPT-408", "Tiempo de espera agotado generando reporte mensual", "POST", "192.168.1.100", new DateTime(2025, 1, 1, 10, 20, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 20, 30, 500, DateTimeKind.Utc), null, "GenerateReport", "/api/reports/generate", null, "Timeout", null, "ReportService", 30500L, null, "SES-005", null, "TRX-005-2025", "Report", "admin" },
                    { 6L, null, null, "GET", "192.168.1.200", new DateTime(2025, 1, 1, 10, 25, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 25, 0, 45, DateTimeKind.Utc), null, "HealthCheck", "/api/health", null, "Completed", null, "HealthService", 45L, null, null, null, "TRX-006-2025", "Health", null },
                    { 7L, null, null, "POST", "192.168.1.104", new DateTime(2025, 1, 1, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 30, 1, 850, DateTimeKind.Utc), null, "ProcessPayment", "/api/payments/process", null, "Completed", null, "PaymentService", 1850L, null, "SES-006", null, "TRX-007-2025", "Transaction", "user004" },
                    { 8L, "NTF-499", "Operación cancelada por el usuario", "POST", "192.168.1.50", new DateTime(2025, 1, 1, 10, 35, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 35, 0, 125, DateTimeKind.Utc), null, "SendNotification", "/api/notifications/send", null, "Cancelled", null, "NotificationService", 125L, null, null, null, "TRX-008-2025", "Notification", "system" },
                    { 9L, null, null, "PUT", "192.168.1.105", new DateTime(2025, 1, 1, 10, 40, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 40, 2, 100, DateTimeKind.Utc), null, "UpdateInventory", "/api/inventory/update", null, "Completed", null, "InventoryService", 2100L, null, "SES-007", null, "TRX-009-2025", "Update", "warehouse001" },
                    { 10L, null, null, "POST", "192.168.1.60", new DateTime(2025, 1, 1, 10, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 45, 0, 95, DateTimeKind.Utc), null, "ProcessBatch", "/api/batch/process", null, "InProgress", null, "BatchService", 95L, null, null, null, "TRX-010-2025", "Batch", "scheduler" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogMicroservice",
                columns: new[] { "fastserver_log_id", "fastserver_logmicroservice_text" },
                values: new object[,]
                {
                    { 1L, "[AuthService] Inicio de autenticación para usuario: admin" },
                    { 2L, "[CustomerService] Consultando lista de clientes con filtro activo" },
                    { 3L, "[OrderService] Error: No se pudo conectar con InventoryService después de 3 reintentos" },
                    { 4L, "[ProductService] Búsqueda ejecutada: 'laptop gaming' - 15 resultados encontrados" },
                    { 5L, "[ReportService] Generando reporte mensual de ventas - Timeout alcanzado en agregación de datos" },
                    { 7L, "[PaymentService] Pago procesado exitosamente - Monto: $1,250.00 - Método: Tarjeta de crédito" },
                    { 8L, "[NotificationService] Envío de notificación cancelado por solicitud del usuario" },
                    { 9L, "[InventoryService] Actualización de stock completada - 50 productos modificados" },
                    { 10L, "[BatchService] Procesamiento por lotes iniciado - 1000 registros pendientes" }
                });

            migrationBuilder.InsertData(
                table: "FastServer_LogServices_Content",
                columns: new[] { "fastserver_no", "fastserver_log_id", "fastserver_logservices_content_text" },
                values: new object[,]
                {
                    { "001", 1L, "{\"username\": \"admin\", \"timestamp\": \"2025-01-01T10:00:00Z\", \"ip\": \"192.168.1.100\"}" },
                    { "001", 3L, "{\"orderId\": null, \"error\": \"ConnectionRefused\", \"service\": \"InventoryService\", \"retries\": 3}" },
                    { "001", 5L, "{\"reportType\": \"MonthlySales\", \"month\": \"December\", \"year\": 2024, \"status\": \"timeout\"}" },
                    { "001", 7L, "{\"transactionId\": \"PAY-12345\", \"amount\": 1250.00, \"currency\": \"USD\", \"method\": \"credit_card\"}" },
                    { "001", 9L, "{\"warehouseId\": \"WH-001\", \"productsUpdated\": 50, \"operation\": \"stock_adjustment\"}" }
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
                name: "IX_FastServer_LogServices_Header_Historico_fastserver_log_date_in",
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
