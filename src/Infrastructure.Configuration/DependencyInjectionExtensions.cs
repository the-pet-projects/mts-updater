namespace PetProjects.MicroTransactionsUpdater.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using Cassandra;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using PetProjects.Framework.Consul.Store;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Application.Consumers.Transactions;
    using PetProjects.MicroTransactionsUpdater.Data.Repositories;
    using PetProjects.MicroTransactionsUpdater.Infrastructure.CrossCutting;

    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection SetupDependencies(this IServiceCollection services)
        {
            services.AddSingleton<EnvironmentContext>(sp =>
            {
                var store = sp.GetRequiredService<IStringKeyValueStore>();
                return new EnvironmentContext
                {
                    Environment = store.GetAndConvertValue<string>("ApplicationKafkaConfiguration/Environment")
                };
            });

            services.AddTransient<CassandraSettings>(sp =>
            {
                var store = sp.GetRequiredService<IStringKeyValueStore>();
                return new CassandraSettings
                {
                    TransactionsWriteConsistencyLevel = (ConsistencyLevel) Enum.Parse(typeof(ConsistencyLevel), store.GetAndConvertValue<string>("CassandraConfiguration/TransactionsWriteConsistencyLevel"))
                };
            });

            services.AddSingleton<CassandraConfiguration>(sp =>
            {
                var store = sp.GetRequiredService<IStringKeyValueStore>();
                return new CassandraConfiguration
                {
                    ContactPoints = store.GetAndConvertValue<string>("CassandraConfiguration/ContactPoints").Split(','),
                    Keyspace = store.GetAndConvertValue<string>("CassandraConfiguration/Keyspace"),
                    ReplicationParameters = JsonConvert.DeserializeObject<Dictionary<string, String>>(store.GetAndConvertValue<string>("CassandraConfiguration/ReplicationParameters"))
                };
            });

            services.AddSingleton<IConsumerConfiguration>(sp =>
            {
                var store = sp.GetRequiredService<IStringKeyValueStore>();
                return new ConsumerConfiguration(
                    store.GetAndConvertValue<string>("ApplicationKafkaConfiguration/ConsumerGroupId"),
                    store.GetAndConvertValue<string>("ApplicationKafkaConfiguration/ClientId"),
                    store.GetAndConvertValue<string>("ApplicationKafkaConfiguration/Brokers").Split(','));
            });

            services.AddSingleton<IConnection>(sp => ConnectionBuilder.BuildConnection(sp.GetRequiredService<CassandraConfiguration>()));
            services.AddTransient<ITransactionsRepository, TransactionsRepository>();
            services.AddTransient<IConsumer<TransactionEventV1>, TransactionEventConsumer>();

            return services;
        }
    }
}
