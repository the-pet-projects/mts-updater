namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Tests.Transactions
{
    using Microsoft.Extensions.Logging;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Wrapper;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Application.Consumers.Transactions;
    using PetProjects.MicroTransactionsUpdater.Data.Repositories;

    public class TransactionEventConsumerProxy : TransactionEventConsumer
    {
        public TransactionEventConsumerProxy(IConsumerConfiguration configuration, ILogger<TransactionEventConsumerProxy> logger, ITransactionsRepository repo) : base(configuration, logger, repo)
        {
        }

        public void Handle(TransactionCreated ev)
        {
            this.CallHandler(new MessageWrapper { Message = ev, MessageType = ev.GetType().AssemblyQualifiedName });
        }
    }
}