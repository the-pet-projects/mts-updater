namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Tests.Mappings
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Application.Consumers.Mappings;

    [TestClass]
    public class TransactionMappingsTests
    {
        private readonly TransactionCreated target;

        public TransactionMappingsTests()
        {
            this.target = new TransactionCreated
            {
                ItemId = Guid.NewGuid(),
                Quantity = 1,
                TransactionId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
        }

        [TestMethod]
        public void ToDomainModel_ValidModel_MapsPropertiesCorrectly()
        {
            // Arrange
            // Act
            var act = this.target.ToDomainModel();

            // Assert
            Assert.AreEqual(this.target.TransactionId, act.Id);
            Assert.AreEqual(this.target.Timestamp.UnixTimeEpochTimestamp, act.EpochTimestamp);
            Assert.AreEqual(this.target.UserId, act.UserId);
            Assert.AreEqual(this.target.Quantity, act.Quantity);
            Assert.AreEqual(this.target.ItemId, act.ItemId);
        }
    }
}