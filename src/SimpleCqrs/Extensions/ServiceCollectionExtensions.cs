using Microsoft.Extensions.DependencyInjection;
using SimpleCqrs.Contracts;
using SimpleCqrs.Dispatchers;

namespace SimpleCqrs.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleCqrs(this IServiceCollection serviceCollection)
        {
            //Add all command, query, and event handlers found in the hosting assembly
            serviceCollection.Scan(
                s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                    .AsImplementedInterfaces()
                    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            //Add dispatchers
            serviceCollection.AddScoped<ICommandDispatcher, CommandDispatcher>();
            serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();
            serviceCollection.AddScoped<IEventDispatcher, EventDispatcher>();

            return serviceCollection;
        }
    }
}