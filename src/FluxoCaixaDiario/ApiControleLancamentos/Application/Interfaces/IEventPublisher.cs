namespace ApiControleLancamentos.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(object @event); 
    }
}
