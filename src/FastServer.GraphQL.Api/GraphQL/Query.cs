namespace FastServer.GraphQL.Api.GraphQL;

/// <summary>
/// Query raíz de GraphQL
/// </summary>
public class Query
{
    /// <summary>
    /// Verifica que la API está funcionando
    /// </summary>
    [GraphQLDescription("Verifica que la API GraphQL está funcionando correctamente")]
    public string Health() => "FastServer GraphQL API is running";

    /// <summary>
    /// Obtiene la versión de la API
    /// </summary>
    [GraphQLDescription("Obtiene la versión de la API")]
    public string Version() => "1.0.0";
}
