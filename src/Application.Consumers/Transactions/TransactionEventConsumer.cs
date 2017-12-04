namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Transactions
{
    using Confluent.Kafka;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.Framework.Kafka.Contracts.Topics;
    using PetProjects.MicroTransactions.Events.Transactions.V1;

    public class TransactionEventConsumer : Consumer<TransactionEvent>
    {
        public TransactionEventConsumer(ITopic<TransactionEvent> topic, IConsumerConfiguration configuration) : base(topic, configuration)
        {
        }

        protected override void HandleStatistics(object sender, string statistics)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleLogs(object sender, LogMessage logMessage)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleError(object sender, Error error)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleOnConsumerError(object sender, Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}