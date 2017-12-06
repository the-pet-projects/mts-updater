namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Mappings
{
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public static class TransactionMappings
    {
        public static Transaction ToDomainModel(this TransactionCreated transaction)
        {
            return new Transaction
            {
                Id = transaction.TransactionId,
                UserId = transaction.UserId,
                EpochTimestamp = transaction.Timestamp.UnixTimeEpochTimestamp
            };
        }
    }
}