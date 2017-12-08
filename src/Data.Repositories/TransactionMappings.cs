namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using Cassandra.Mapping;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public class TransactionMappings : Mappings
    {
        public TransactionMappings()
        {
            this.For<Transaction>()
                .TableName("transactions")
                .PartitionKey(t => t.Id)
                .Column(t => t.Id, cfg => cfg.WithName("transaction_id"))
                .Column(t => t.EpochTimestamp, cfg => cfg.WithName("timestamp"))
                .Column(t => t.UserId, cfg => cfg.WithName("user_id"))
                .ExplicitColumns();
        }
    }
}