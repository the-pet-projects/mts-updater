namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Tests.Transactions
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Data.Repositories;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    [TestClass]
    public class TransactionEventConsumerTests
    {
        private readonly Mock<IConsumerConfiguration> consumerConfigMock = new Mock<IConsumerConfiguration>();
        private readonly Mock<ILogger<TransactionEventConsumerProxy>> loggerMock = new Mock<ILogger<TransactionEventConsumerProxy>>();
        private readonly Mock<ITransactionsRepository> repoMock = new Mock<ITransactionsRepository>();

        private readonly TransactionEventConsumerProxy target;

        public TransactionEventConsumerTests()
        {
            this.repoMock.Setup(r => r.AddAsync(It.IsAny<Transaction>())).Returns(Task.CompletedTask);
            this.target = new TransactionEventConsumerProxy(this.consumerConfigMock.Object, this.loggerMock.Object, this.repoMock.Object);
        }

        [TestMethod]
        public void Handle_ValidEvent_CallsAddAsync()
        {
            // Arrange
            var transactionEvent = new TransactionCreated(Guid.Empty, Guid.Empty, DateTime.UtcNow);

            // Act
            this.target.Handle(transactionEvent);

            // Assert
            this.repoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }
    }
}