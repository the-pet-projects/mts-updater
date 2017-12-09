namespace IntegrationTests
{
    using System;
    using PetProjects.Framework.Kafka.Configurations.Producer;
    using PetProjects.Framework.Kafka.Producer;
    using PetProjects.MicroTransactions.Events.Transactions.V1;

    internal static class EventProducer
    {
        private static readonly Lazy<IProducer<TransactionEvent>> LazyProducer = BuildProducer();

        public static IProducer<TransactionEvent> Producer => LazyProducer.Value;

        private static Lazy<IProducer<TransactionEvent>> BuildProducer()
        {
            return new Lazy<IProducer<TransactionEvent>>(() => new Producer<TransactionEvent>(
                new TransactionEventsTopic(),
                new ProducerConfiguration("mts-updater-integrationtests", AppSettings.Current.KafkaBrokers)));
        }
    }
}