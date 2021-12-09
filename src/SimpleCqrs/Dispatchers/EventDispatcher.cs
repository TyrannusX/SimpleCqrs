using System;
using System.Threading.Tasks;
using Dawn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleCqrs.Contracts;

namespace SimpleCqrs.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EventDispatcher> _logger;

        public EventDispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<EventDispatcher> logger)
        {
            _serviceScopeFactory = Guard.Argument(serviceScopeFactory, nameof(serviceScopeFactory)).NotNull().Value;
            _logger = Guard.Argument(logger, nameof(logger)).NotNull().Value;
        }

        public async Task PublishAsync<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            IEnumerable<IEventHandler<TEvent>> matchingEventHandlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
            if(matchingEventHandlers.Count() == 0)
            {
                string message = $"Could not resolve at least one event handler for event {evnt.GetType()}";
                _logger.LogError(message, evnt.GetType());    
                throw new InvalidOperationException(message);
            }
            
            foreach(IEventHandler<TEvent> eventHandler in matchingEventHandlers)
            {
                await eventHandler.HandleAsync(evnt);
            }
        }
    }
}