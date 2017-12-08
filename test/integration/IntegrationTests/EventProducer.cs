namespace IntegrationTests
{
    using PetProjects.Framework.Kafka.Configurations.Producer;
    using PetProjects.Framework.Kafka.Producer;
    using PetProjects.MicroTransactions.Events.Transactions.V1;

    internal static class EventProducer
    {
        public static IProducer<TransactionEvent> Producer { get; } = BuildProducer();

        private static IProducer<TransactionEvent> BuildProducer()
        {
            return new Producer<TransactionEvent>(
                new TransactionEventsTopic(),
                new ProducerConfiguration("mts-updater-integrationtests", AppSettings.Current.KafkaBrokers));
        }
    }
}