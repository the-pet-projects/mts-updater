namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Mappings
{
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public static class TransactionMappings
    {
        public static Transaction ToDomainModel(this TransactionCreatedEvent transaction)
        {
            return new Transaction
            {
                Id = transaction.TransactionId,
                UserId = transaction.UserId,
                EpochTimestamp = transaction.Timestamp.UnixTimeEpochTimestamp,
                ItemId = transaction.ItemId,
                Quantity = transaction.Quantity
            };
        }
    }
}