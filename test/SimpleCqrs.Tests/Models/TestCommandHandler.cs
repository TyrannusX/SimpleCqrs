using System.Threading.Tasks;
using SimpleCqrs.Contracts;

namespace SimpleCqrs.Tests.Models
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public async Task HandleAsync(TestCommand command)
        {

        }
    }

    public class TestCommandHandlerWithReturnValue : ICommandHandler<TestCommandWithReturnValue, string>
    {
        public async Task<string> HandleAsync(TestCommandWithReturnValue command)
        {
            return "hello world";
        }
    }
}