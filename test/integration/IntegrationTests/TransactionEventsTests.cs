namespace IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cassandra;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PetProjects.MicroTransactions.Events.Transactions.V1;

    [TestClass]
    public class TransactionEventsTests
    {
        [TestMethod]
        public async Task ProduceTransactionCreated_ValidEvent_SelectFromReadModelReturnsTransaction()
        {
            // Arrange
            var transaction = new TransactionCreatedEvent
            {
                TransactionId = Utils.GenerateTransactionId(),
                ItemId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Quantity = 1
            };

            // Act
            await EventProducer.Producer.ProduceAsync(transaction).ConfigureAwait(false);

            // Assert
            var result = await Utils.AssertWithRetry<Row>(async () =>
            {
                var st = new SimpleStatement("SELECT * FROM transactions WHERE transaction_id = ?", transaction.TransactionId);
                var rs = await CassandraConnection.Session.ExecuteAsync(st).ConfigureAwait(false);
                var row = rs.FirstOrDefault();

                Assert.IsNotNull(row);
                return row;
            });
            
            Assert.AreEqual(transaction.TransactionId, result.GetValue<Guid>("transaction_id"));
            Assert.AreEqual(transaction.Timestamp.UnixTimeEpochTimestamp, result.GetValue<long>("timestamp"));
            Assert.AreEqual(transaction.UserId, result.GetValue<Guid>("user_id"));
            Assert.AreEqual(transaction.Quantity, result.GetValue<int>("quantity"));
            Assert.AreEqual(transaction.ItemId, result.GetValue<Guid>("item_id"));
        }
    }
}