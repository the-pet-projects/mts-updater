namespace IntegrationTests
{
    using Cassandra;

    internal static class CassandraConnection
    {
        public static ISession Session { get; } = BuildSession();

        private static ISession BuildSession()
        {
            var cluster = Cluster.Builder()
                .AddContactPoints(AppSettings.Current.CassandraContactPoints)
                .WithDefaultKeyspace(AppSettings.Current.CassandraKeyspace)
                .Build();

            return cluster.Connect();
        }
    }
}