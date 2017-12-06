namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using Cassandra;
    using Cassandra.Data.Linq;
    using Cassandra.Mapping;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public static class ConnectionBuilder
    {
        public static IConnection BuildConnection(CassandraConfiguration config)
        {
            var cluster = Cluster.Builder()
                .WithDefaultKeyspace(config.Keyspace)
                .AddContactPoints(config.ContactPoints)
                .Build();

            var mapConfig = new MappingConfiguration();
            mapConfig.Define<TransactionMappings>();

            var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists(config.ReplicationParameters);

            var table = new Table<Transaction>(session, mapConfig);
            table.CreateIfNotExists();

            return new Connection(cluster, session, mapConfig);
        }
    }
}