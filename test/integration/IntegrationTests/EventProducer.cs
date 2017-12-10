namespace IntegrationTests
{
    using System;
    using PetProjects.Framework.Kafka.Configurations.Producer;
    using PetProjects.Framework.Kafka.Producer;
    using PetProjects.MicroTransactions.Events.Transactions.V1;

    internal static class EventProducer
    {
        private static readonly Lazy<IProducer<TransactionEventV1>> LazyProducer = BuildProducer();

        public static IProducer<TransactionEventV1> Producer => LazyProducer.Value;

        private static Lazy<IProducer<TransactionEventV1>> BuildProducer()
        {
            return new Lazy<IProducer<TransactionEventV1>>(() => new Producer<TransactionEventV1>(
                new TransactionEventsTopicV1(AppSettings.Current.KafkaTopicEnvironment),
                new ProducerConfiguration("mts-updater-integrationtests", AppSettings.Current.KafkaBrokers)));
        }
    }
}