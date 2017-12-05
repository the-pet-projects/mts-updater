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
                .Column(t => t.Id)
                .Column(t => t.EpochTimestamp)
                .Column(t => t.UserId)
                .ExplicitColumns();
        }
    }
}