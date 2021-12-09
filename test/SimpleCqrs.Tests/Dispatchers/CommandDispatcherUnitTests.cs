using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SimpleCqrs.Contracts;
using SimpleCqrs.Dispatchers;
using SimpleCqrs.Tests.Models;

namespace SimpleCqrs.Tests.Dispatchers
{
    [TestFixture]
    public class CommandDispatcherUnitTests
    {
        [Test]
        [Category("Unit")]
        public async Task SendAsync_ThrowsInvalidOperationException_WhenNoHandlersRegistered()
        {
            //Arrange
            Mock<IServiceScope> serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(scope => scope.ServiceProvider).Returns(Mock.Of<IServiceProvider>());

            Mock<IServiceScopeFactory> serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(factory => factory.CreateScope()).Returns(serviceScope.Object);

            Mock<ILogger<CommandDispatcher>> logger = new Mock<ILogger<CommandDispatcher>>();
            CommandDispatcher dispatcher = new CommandDispatcher(serviceScopeFactory.Object, logger.Object);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => dispatcher.SendAsync(Mock.Of<ICommand>()));
        }

        [Test]
        [Category("Unit")]
        public async Task SendAsync_RunsSuccessfully()
        {
            //Arrange
            IServiceCollection collection = new ServiceCollection();
            collection.AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>();

            Mock<IServiceScope> serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(scope => scope.ServiceProvider).Returns(collection.BuildServiceProvider());

            Mock<IServiceScopeFactory> serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(factory => factory.CreateScope()).Returns(serviceScope.Object);

            Mock<ILogger<CommandDispatcher>> logger = new Mock<ILogger<CommandDispatcher>>();
            CommandDispatcher dispatcher = new CommandDispatcher(serviceScopeFactory.Object, logger.Object);

            //Act
            await dispatcher.SendAsync(new TestCommand());

            //Assert
            serviceScope.Verify(scope => scope.ServiceProvider, Times.Once);
            serviceScopeFactory.Verify(factory => factory.CreateScope(), Times.Once);
        }

        [Test]
        [Category("Unit")]
        public async Task SendAsyncTResult_ThrowsInvalidOperationException_WhenNoHandlersRegistered()
        {
            //Arrange
            Mock<IServiceScope> serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(scope => scope.ServiceProvider).Returns(Mock.Of<IServiceProvider>());

            Mock<IServiceScopeFactory> serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(factory => factory.CreateScope()).Returns(serviceScope.Object);

            Mock<ILogger<CommandDispatcher>> logger = new Mock<ILogger<CommandDispatcher>>();
            CommandDispatcher dispatcher = new CommandDispatcher(serviceScopeFactory.Object, logger.Object);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => dispatcher.SendAsync<TestCommandWithReturnValue, string>(new TestCommandWithReturnValue()));
        }

        [Test]
        [Category("Unit")]
        public async Task SendAsyncTResult_RunsSuccessfully_AndReturnsString()
        {
            //Arrange
            IServiceCollection collection = new ServiceCollection();
            collection.AddScoped<ICommandHandler<TestCommandWithReturnValue, string>, TestCommandHandlerWithReturnValue>();

            Mock<IServiceScope> serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(scope => scope.ServiceProvider).Returns(collection.BuildServiceProvider());

            Mock<IServiceScopeFactory> serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(factory => factory.CreateScope()).Returns(serviceScope.Object);

            Mock<ILogger<CommandDispatcher>> logger = new Mock<ILogger<CommandDispatcher>>();
            CommandDispatcher dispatcher = new CommandDispatcher(serviceScopeFactory.Object, logger.Object);

            //Act
            string result = await dispatcher.SendAsync<TestCommandWithReturnValue, string>(new TestCommandWithReturnValue());

            //Assert
            result.Should().Be("hello world");
            serviceScope.Verify(scope => scope.ServiceProvider, Times.Once);
            serviceScopeFactory.Verify(factory => factory.CreateScope(), Times.Once);
        }
    }
}