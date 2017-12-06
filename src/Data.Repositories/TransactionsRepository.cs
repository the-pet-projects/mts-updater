namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using System.Threading.Tasks;
    using Cassandra.Mapping;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IConnection connection;
        private readonly CassandraSettings settings;

        public TransactionsRepository(IConnection connection, CassandraSettings settings)
        {
            this.connection = connection;
            this.settings = settings;
        }

        public Task AddAsync(Transaction transaction)
        {
            return this.connection.Mapper.InsertAsync(
                transaction, 
                false, 
                null, 
                CqlQueryOptions.New().SetConsistencyLevel(this.settings.TransactionsWriteConsistencyLevel));
        }
    }
}
