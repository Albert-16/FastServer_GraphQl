using Microsoft.Data.SqlClient;

var connectionString = "Server=DESKTOP-9C0B00C\SQLEXPRESS;Database=FastServerLogs_Dev;Integrated Security=True;TrustServerCertificate=True";

using var conn = new SqlConnection(connectionString);
await conn.OpenAsync();

// Check MicroservicesClusters
using var cmd1 = new SqlCommand("SELECT COUNT(*) FROM microservices_clusters", conn);
var clusterCount = await cmd1.ExecuteScalarAsync();
Console.WriteLine($"MicroservicesClusters: {clusterCount} registros");

// Check MicroserviceRegisters
using var cmd2 = new SqlCommand("SELECT COUNT(*) FROM microservice_registers", conn);
var registerCount = await cmd2.ExecuteScalarAsync();
Console.WriteLine($"MicroserviceRegisters: {registerCount} registros");

// Check MicroserviceMethods
using var cmd3 = new SqlCommand("SELECT COUNT(*) FROM microservice_methods", conn);
var methodCount = await cmd3.ExecuteScalarAsync();
Console.WriteLine($"MicroserviceMethods: {methodCount} registros");

// Show sample data
using var cmd4 = new SqlCommand(@"
SELECT mr.microservice_id, mr.microservice_name, mm.microservice_method_name, mm.microservice_method_url
FROM microservice_registers mr
LEFT JOIN microservice_methods mm ON mr.microservice_id = mm.microservice_id
ORDER BY mr.microservice_id", conn);
using var reader = await cmd4.ExecuteReaderAsync();

Console.WriteLine("\nRegistros de Microservicios y sus métodos:");
while (await reader.ReadAsync())
{
    Console.WriteLine($"  ID: {reader.GetInt64(0)}, Name: {reader.GetValue(1)}, Method: {reader.GetValue(2)}, URL: {reader.GetValue(3)}");
}
