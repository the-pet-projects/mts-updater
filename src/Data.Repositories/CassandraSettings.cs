namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using Cassandra;

    public class CassandraSettings
    {
        public ConsistencyLevel TransactionsWriteConsistencyLevel { get; set; }
    }
}