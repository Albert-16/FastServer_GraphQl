using FastServer.Application.Events.UserEvents;
using HotChocolate.Subscriptions;

namespace FastServer.Application.EventPublishers;

/// <summary>
/// Implementación del publisher de eventos de usuarios usando HotChocolate
/// </summary>
public class UserEventPublisher : IUserEventPublisher
{
    private readonly ITopicEventSender _eventSender;

    public UserEventPublisher(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public async Task PublishUserCreatedAsync(UserCreatedEvent userEvent)
    {
        await _eventSender.SendAsync("UserCreated", userEvent);
    }

    public async Task PublishUserUpdatedAsync(UserUpdatedEvent userEvent)
    {
        await _eventSender.SendAsync("UserUpdated", userEvent);
    }

    public async Task PublishUserDeletedAsync(UserDeletedEvent userEvent)
    {
        await _eventSender.SendAsync("UserDeleted", userEvent);
    }
}
