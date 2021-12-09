namespace SimpleCqrs.Contracts
{
    public interface IQueryDispatcher
    {
        Task<TResult> SendAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}