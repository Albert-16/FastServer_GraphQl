using Npgsql;

var connectionString = "Host=localhost;Port=5432;Database=FastServerLogs;Username=postgres;Password=Souma";

using var conn = new NpgsqlConnection(connectionString);
await conn.OpenAsync();

// Check LogServicesHeader
using var cmd1 = new NpgsqlCommand("SELECT COUNT(*) FROM \"FastServer_LogServices_Header\"", conn);
var headerCount = await cmd1.ExecuteScalarAsync();
Console.WriteLine($"LogServicesHeader: {headerCount} registros");

// Check LogMicroservice  
using var cmd2 = new NpgsqlCommand("SELECT COUNT(*) FROM \"FastServer_LogMicroservice\"", conn);
var microCount = await cmd2.ExecuteScalarAsync();
Console.WriteLine($"LogMicroservice: {microCount} registros");

// Check LogServicesContent
using var cmd3 = new NpgsqlCommand("SELECT COUNT(*) FROM \"FastServer_LogServices_Content\"", conn);
var contentCount = await cmd3.ExecuteScalarAsync();
Console.WriteLine($"LogServicesContent: {contentCount} registros");

// Show sample data
using var cmd4 = new NpgsqlCommand("SELECT \"fastserver_log_id\", \"fastserver_microservice_name\", \"fastserver_log_method_name\" FROM \"FastServer_LogServices_Header\" LIMIT 5", conn);
using var reader = await cmd4.ExecuteReaderAsync();

Console.WriteLine("\nPrimeros 5 registros de Headers:");
while (await reader.ReadAsync())
{
    Console.WriteLine($"  ID: {reader.GetInt64(0)}, Microservice: {reader.GetValue(1)}, Method: {reader.GetValue(2)}");
}
