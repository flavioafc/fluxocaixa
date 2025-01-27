using ApiControleLancamentos.Application.Interfaces;
using MassTransit;

namespace ApiControleLancamentos.Infra.Messaging
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            await _publishEndpoint.Publish(@event);
        }
    }
}
