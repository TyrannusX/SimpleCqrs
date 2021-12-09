using System;
using System.Threading.Tasks;
using Dawn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleCqrs.Contracts;

namespace SimpleCqrs.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<QueryDispatcher> _logger;

        public QueryDispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<QueryDispatcher> logger)
        {
            _serviceScopeFactory = Guard.Argument(serviceScopeFactory, nameof(serviceScopeFactory)).NotNull().Value;
            _logger = Guard.Argument(logger, nameof(logger)).NotNull().Value;
        }

        public async Task<TResult> SendAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            
            try
            {
                IQueryHandler<TQuery, TResult> matchingQueryHandler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
                return await matchingQueryHandler.HandleAsync(query);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogError("Could not resolve a query handler for query {query}: {ex}", query.GetType(), ex);
                throw;
            }
        }
    }
}