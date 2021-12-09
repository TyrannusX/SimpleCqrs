using System;
using System.Threading.Tasks;
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
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(provider => provider.GetRequiredService(It.IsAny<Type>())).Returns(new TestCommandHandler());

            Mock<IServiceScope> serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(scope => scope.ServiceProvider).Returns(serviceProvider.Object);

            Mock<IServiceScopeFactory> serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(factory => factory.CreateScope()).Returns(serviceScope.Object);

            Mock<ILogger<CommandDispatcher>> logger = new Mock<ILogger<CommandDispatcher>>();
            CommandDispatcher dispatcher = new CommandDispatcher(serviceScopeFactory.Object, logger.Object);

            //Act
            await dispatcher.SendAsync(Mock.Of<ICommand>());

            //Assert
            serviceProvider.Verify(provider => provider.GetRequiredService(It.IsAny<Type>()), Times.Once);
            serviceScope.Verify(scope => scope.ServiceProvider, Times.Once);
            serviceScopeFactory.Verify(factory => factory.CreateScope(), Times.Once);
        }
    }
}