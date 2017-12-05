namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Transactions
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Logging;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.Framework.Kafka.Contracts.Topics;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Application.Consumers.Mappings;
    using PetProjects.MicroTransactionsUpdater.Data.Repositories;

    public class TransactionEventConsumer : Consumer<TransactionEvent>
    {
        private readonly ILogger logger;
        private readonly ITransactionsRepository repo;

        public TransactionEventConsumer(ITopic<TransactionEvent> topic, IConsumerConfiguration configuration, ILogger logger, ITransactionsRepository repo) : base(topic, configuration)
        {
            this.logger = logger;
            this.repo = repo;
            this.ConsumerHandlerFor<TransactionCreated>(this.Handle);
        }

        protected override void HandleStatistics(object sender, string statistics)
        {
            this.logger.LogDebug("TransactionEventConsumer statistics: {statistics}", statistics);
        }

        protected override void HandleLogs(object sender, LogMessage logMessage)
        {
            this.logger.LogInformation("TransactionEventConsumer log: {logMessage}", logMessage);
        }

        protected override void HandleError(object sender, Error error)
        {
            this.logger.LogError("TransactionEventConsumer error: {error}", error);
        }

        protected override void HandleOnConsumerError(object sender, Message message)
        {
        }

        private void Handle(TransactionCreated evt)
        {
            this.repo.AddAsync(evt.ToDomainModel()).Wait();
        }
    }
}