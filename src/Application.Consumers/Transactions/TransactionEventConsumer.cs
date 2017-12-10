namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Transactions
{
    using Microsoft.Extensions.Logging;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Application.Consumers.Mappings;
    using PetProjects.MicroTransactionsUpdater.Data.Repositories;
    using PetProjects.MicroTransactionsUpdater.Infrastructure.CrossCutting;

    public class TransactionEventConsumer : Consumer<TransactionEventV1>
    {
        private readonly ITransactionsRepository repo;

        public TransactionEventConsumer(EnvironmentContext environmentContext, IConsumerConfiguration configuration, ILogger<TransactionEventConsumer> logger, ITransactionsRepository repo) : base(new TransactionEventsTopicV1(environmentContext.Environment), configuration, logger, true)
        {
            this.repo = repo;
            this.Receive<TransactionCreatedEvent>(this.Handle);
        }

        private void Handle(TransactionCreatedEvent evt)
        {
            this.repo.AddAsync(evt.ToDomainModel()).Wait();
        }
    }
}