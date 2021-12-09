namespace SimpleCqrs.Contracts
{
    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent evnt) where TEvent : IEvent;
    }
}