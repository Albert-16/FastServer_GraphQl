using FastServer.Application.Events.UserEvents;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Publisher de eventos de usuarios para suscripciones GraphQL
/// </summary>
public interface IUserEventPublisher
{
    /// <summary>
    /// Publica un evento de creación de usuario
    /// </summary>
    Task PublishUserCreatedAsync(UserCreatedEvent userEvent);

    /// <summary>
    /// Publica un evento de actualización de usuario
    /// </summary>
    Task PublishUserUpdatedAsync(UserUpdatedEvent userEvent);

    /// <summary>
    /// Publica un evento de eliminación de usuario
    /// </summary>
    Task PublishUserDeletedAsync(UserDeletedEvent userEvent);
}
