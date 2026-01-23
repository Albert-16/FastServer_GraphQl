namespace FastServer.GraphQL.Api.GraphQL;

/// <summary>
/// Mutation raíz de GraphQL
/// </summary>
public class Mutation
{
    /// <summary>
    /// Operación de prueba
    /// </summary>
    [GraphQLDescription("Operación de prueba que retorna un mensaje")]
    public string Ping() => "Pong";
}
