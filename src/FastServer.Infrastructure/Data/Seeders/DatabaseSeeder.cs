using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Infrastructure.Data.Seeders;

/// <summary>
/// Clase para poblar la base de datos con datos de prueba
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Aplica el seeding de datos de prueba
    /// </summary>
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedLogServicesHeaders(modelBuilder);
        SeedLogMicroservices(modelBuilder);
        SeedLogServicesContents(modelBuilder);
    }

    private static void SeedLogServicesHeaders(ModelBuilder modelBuilder)
    {
        var baseDate = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<LogServicesHeader>().HasData(
            new LogServicesHeader
            {
                LogId = 1,
                LogDateIn = baseDate,
                LogDateOut = baseDate.AddMilliseconds(150),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/users/authenticate",
                LogMethodName = "AuthenticateUser",
                HttpMethod = "POST",
                MicroserviceName = "AuthService",
                RequestDuration = 150,
                TransactionId = "TRX-001-2025",
                UserId = "admin",
                SessionId = "SES-001",
                IpFs = "192.168.1.100",
                TypeProcess = "Authentication"
            },
            new LogServicesHeader
            {
                LogId = 2,
                LogDateIn = baseDate.AddMinutes(5),
                LogDateOut = baseDate.AddMinutes(5).AddMilliseconds(85),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/customers/list",
                LogMethodName = "GetCustomerList",
                HttpMethod = "GET",
                MicroserviceName = "CustomerService",
                RequestDuration = 85,
                TransactionId = "TRX-002-2025",
                UserId = "user001",
                SessionId = "SES-002",
                IpFs = "192.168.1.101",
                TypeProcess = "Query"
            },
            new LogServicesHeader
            {
                LogId = 3,
                LogDateIn = baseDate.AddMinutes(10),
                LogDateOut = baseDate.AddMinutes(10).AddMilliseconds(5200),
                LogState = LogState.Failed,
                LogMethodUrl = "/api/orders/create",
                LogMethodName = "CreateOrder",
                HttpMethod = "POST",
                MicroserviceName = "OrderService",
                RequestDuration = 5200,
                TransactionId = "TRX-003-2025",
                UserId = "user002",
                SessionId = "SES-003",
                IpFs = "192.168.1.102",
                TypeProcess = "Transaction",
                ErrorCode = "ORD-500",
                ErrorDescription = "Error de conexión con el servicio de inventario"
            },
            new LogServicesHeader
            {
                LogId = 4,
                LogDateIn = baseDate.AddMinutes(15),
                LogDateOut = baseDate.AddMinutes(15).AddMilliseconds(320),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/products/search",
                LogMethodName = "SearchProducts",
                HttpMethod = "GET",
                MicroserviceName = "ProductService",
                RequestDuration = 320,
                TransactionId = "TRX-004-2025",
                UserId = "user003",
                SessionId = "SES-004",
                IpFs = "192.168.1.103",
                TypeProcess = "Query"
            },
            new LogServicesHeader
            {
                LogId = 5,
                LogDateIn = baseDate.AddMinutes(20),
                LogDateOut = baseDate.AddMinutes(20).AddMilliseconds(30500),
                LogState = LogState.Timeout,
                LogMethodUrl = "/api/reports/generate",
                LogMethodName = "GenerateReport",
                HttpMethod = "POST",
                MicroserviceName = "ReportService",
                RequestDuration = 30500,
                TransactionId = "TRX-005-2025",
                UserId = "admin",
                SessionId = "SES-005",
                IpFs = "192.168.1.100",
                TypeProcess = "Report",
                ErrorCode = "RPT-408",
                ErrorDescription = "Tiempo de espera agotado generando reporte mensual"
            },
            new LogServicesHeader
            {
                LogId = 6,
                LogDateIn = baseDate.AddMinutes(25),
                LogDateOut = baseDate.AddMinutes(25).AddMilliseconds(45),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/health",
                LogMethodName = "HealthCheck",
                HttpMethod = "GET",
                MicroserviceName = "HealthService",
                RequestDuration = 45,
                TransactionId = "TRX-006-2025",
                IpFs = "192.168.1.200",
                TypeProcess = "Health"
            },
            new LogServicesHeader
            {
                LogId = 7,
                LogDateIn = baseDate.AddMinutes(30),
                LogDateOut = baseDate.AddMinutes(30).AddMilliseconds(1850),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/payments/process",
                LogMethodName = "ProcessPayment",
                HttpMethod = "POST",
                MicroserviceName = "PaymentService",
                RequestDuration = 1850,
                TransactionId = "TRX-007-2025",
                UserId = "user004",
                SessionId = "SES-006",
                IpFs = "192.168.1.104",
                TypeProcess = "Transaction"
            },
            new LogServicesHeader
            {
                LogId = 8,
                LogDateIn = baseDate.AddMinutes(35),
                LogDateOut = baseDate.AddMinutes(35).AddMilliseconds(125),
                LogState = LogState.Cancelled,
                LogMethodUrl = "/api/notifications/send",
                LogMethodName = "SendNotification",
                HttpMethod = "POST",
                MicroserviceName = "NotificationService",
                RequestDuration = 125,
                TransactionId = "TRX-008-2025",
                UserId = "system",
                IpFs = "192.168.1.50",
                TypeProcess = "Notification",
                ErrorCode = "NTF-499",
                ErrorDescription = "Operación cancelada por el usuario"
            },
            new LogServicesHeader
            {
                LogId = 9,
                LogDateIn = baseDate.AddMinutes(40),
                LogDateOut = baseDate.AddMinutes(40).AddMilliseconds(2100),
                LogState = LogState.Completed,
                LogMethodUrl = "/api/inventory/update",
                LogMethodName = "UpdateInventory",
                HttpMethod = "PUT",
                MicroserviceName = "InventoryService",
                RequestDuration = 2100,
                TransactionId = "TRX-009-2025",
                UserId = "warehouse001",
                SessionId = "SES-007",
                IpFs = "192.168.1.105",
                TypeProcess = "Update"
            },
            new LogServicesHeader
            {
                LogId = 10,
                LogDateIn = baseDate.AddMinutes(45),
                LogDateOut = baseDate.AddMinutes(45).AddMilliseconds(95),
                LogState = LogState.InProgress,
                LogMethodUrl = "/api/batch/process",
                LogMethodName = "ProcessBatch",
                HttpMethod = "POST",
                MicroserviceName = "BatchService",
                RequestDuration = 95,
                TransactionId = "TRX-010-2025",
                UserId = "scheduler",
                IpFs = "192.168.1.60",
                TypeProcess = "Batch"
            }
        );
    }

    private static void SeedLogMicroservices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogMicroservice>().HasData(
            new LogMicroservice
            {
                LogId = 1,
                LogMicroserviceText = "[AuthService] Inicio de autenticación para usuario: admin"
            },
            new LogMicroservice
            {
                LogId = 2,
                LogMicroserviceText = "[CustomerService] Consultando lista de clientes con filtro activo"
            },
            new LogMicroservice
            {
                LogId = 3,
                LogMicroserviceText = "[OrderService] Error: No se pudo conectar con InventoryService después de 3 reintentos"
            },
            new LogMicroservice
            {
                LogId = 4,
                LogMicroserviceText = "[ProductService] Búsqueda ejecutada: 'laptop gaming' - 15 resultados encontrados"
            },
            new LogMicroservice
            {
                LogId = 5,
                LogMicroserviceText = "[ReportService] Generando reporte mensual de ventas - Timeout alcanzado en agregación de datos"
            },
            new LogMicroservice
            {
                LogId = 7,
                LogMicroserviceText = "[PaymentService] Pago procesado exitosamente - Monto: $1,250.00 - Método: Tarjeta de crédito"
            },
            new LogMicroservice
            {
                LogId = 8,
                LogMicroserviceText = "[NotificationService] Envío de notificación cancelado por solicitud del usuario"
            },
            new LogMicroservice
            {
                LogId = 9,
                LogMicroserviceText = "[InventoryService] Actualización de stock completada - 50 productos modificados"
            },
            new LogMicroservice
            {
                LogId = 10,
                LogMicroserviceText = "[BatchService] Procesamiento por lotes iniciado - 1000 registros pendientes"
            }
        );
    }

    private static void SeedLogServicesContents(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogServicesContent>().HasData(
            new LogServicesContent
            {
                LogId = 1,
                LogServicesContentText = "{\"username\": \"admin\", \"timestamp\": \"2025-01-01T10:00:00Z\", \"ip\": \"192.168.1.100\"}",
                ContentNo = "001"
            },
            new LogServicesContent
            {
                LogId = 3,
                LogServicesContentText = "{\"orderId\": null, \"error\": \"ConnectionRefused\", \"service\": \"InventoryService\", \"retries\": 3}",
                ContentNo = "001"
            },
            new LogServicesContent
            {
                LogId = 5,
                LogServicesContentText = "{\"reportType\": \"MonthlySales\", \"month\": \"December\", \"year\": 2024, \"status\": \"timeout\"}",
                ContentNo = "001"
            },
            new LogServicesContent
            {
                LogId = 7,
                LogServicesContentText = "{\"transactionId\": \"PAY-12345\", \"amount\": 1250.00, \"currency\": \"USD\", \"method\": \"credit_card\"}",
                ContentNo = "001"
            },
            new LogServicesContent
            {
                LogId = 9,
                LogServicesContentText = "{\"warehouseId\": \"WH-001\", \"productsUpdated\": 50, \"operation\": \"stock_adjustment\"}",
                ContentNo = "001"
            }
        );
    }
}
