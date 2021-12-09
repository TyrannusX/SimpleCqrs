namespace SimpleCqrs.Contracts
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent evnt);
    }
}