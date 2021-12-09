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
}