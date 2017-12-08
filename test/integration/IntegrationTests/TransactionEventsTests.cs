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
            var transaction = new TransactionCreated(Utils.GenerateTransactionId(), Guid.NewGuid());

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

            var transactionId = result.GetValue<Guid>("transaction_id");
            var userId = result.GetValue<Guid>("user_id");
            var timestamp = result.GetValue<long>("timestamp");
            Assert.AreEqual(transaction.TransactionId, transactionId);
            Assert.AreEqual(transaction.Timestamp.UnixTimeEpochTimestamp, timestamp);
            Assert.AreEqual(transaction.UserId, userId);
        }
    }
}