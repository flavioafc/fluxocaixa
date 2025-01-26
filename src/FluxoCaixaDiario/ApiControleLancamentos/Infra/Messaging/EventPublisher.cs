using ApiControleLancamentos.Application.Interfaces;

namespace ApiControleLancamentos.Infra.Messaging
{
    public class EventPublisher : IEventPublisher
    {
        public Task PublishAsync(object @event)
        {
            // Implementation of PublishAsync method
            return Task.CompletedTask;
        }
    }
}
