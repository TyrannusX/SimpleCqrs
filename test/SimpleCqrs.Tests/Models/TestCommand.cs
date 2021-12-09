using SimpleCqrs.Contracts;

namespace SimpleCqrs.Tests.Models
{
    public class TestCommand : ICommand
    {

    }

    public class TestCommandWithReturnValue : ICommand<string>
    {

    }
}