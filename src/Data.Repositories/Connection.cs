namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using Cassandra;
    using Cassandra.Mapping;

    public class Connection : IConnection
    {
        private readonly ICluster cluster;

        public Connection(ICluster cluster, ISession session, MappingConfiguration mapConfig)
        {
            this.cluster = cluster;
            this.Session = session;
            this.Mapper = new Mapper(this.Session, mapConfig);
        }

        public ISession Session { get; }

        public IMapper Mapper { get; }

        public void Dispose()
        {
            this.Session?.Dispose();
            this.cluster?.Dispose();
        }
    }
}