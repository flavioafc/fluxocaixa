﻿namespace ApiControleLancamentos.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
    }
}
