namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Transactions
{
    using Microsoft.Extensions.Logging;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Application.Consumers.Mappings;
    using PetProjects.MicroTransactionsUpdater.Data.Repositories;

    public class TransactionEventConsumer : Consumer<TransactionEvent>
    {
        private readonly ITransactionsRepository repo;

        public TransactionEventConsumer(IConsumerConfiguration configuration, ILogger<TransactionEventConsumer> logger, ITransactionsRepository repo) : base(new TransactionEventsTopic(), configuration, logger, true)
        {
            this.repo = repo;
            this.Receive<TransactionCreated>(this.Handle);
        }

        private void Handle(TransactionCreated evt)
        {
            this.repo.AddAsync(evt.ToDomainModel()).Wait();
        }
    }
}