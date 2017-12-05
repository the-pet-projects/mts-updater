namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using Cassandra;
    using Cassandra.Data.Linq;
    using Cassandra.Mapping;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public static class ConnectionBuilder
    {
        public static IConnection BuildConnection(CassandraSettings settings)
        {
            var cluster = Cluster.Builder()
                .WithDefaultKeyspace(settings.Keyspace)
                .AddContactPoints(settings.ContactPoints.Split(','))
                .Build();

            var mapConfig = new MappingConfiguration();
            mapConfig.Define<TransactionMappings>();

            var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists(settings.ReplicationParameters);

            var table = new Table<Transaction>(session, mapConfig);
            table.CreateIfNotExists();

            return new Connection(cluster, session, mapConfig);
        }
    }
}