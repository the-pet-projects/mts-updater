namespace PetProjects.MicroTransactionsUpdater.Application.Consumers.Tests.Transactions
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Contracts.Topics;
    using PetProjects.Framework.Kafka.Wrapper;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.MicroTransactionsUpdater.Application.Consumers.Transactions;
    using PetProjects.MicroTransactionsUpdater.Data.Repositories;

    internal class TransactionEventConsumerProxy : TransactionEventConsumer
    {
        public TransactionEventConsumerProxy(IConsumerConfiguration configuration, ILogger logger, ITransactionsRepository repo) : base(configuration, logger, repo)
        {
        }

        public void Handle(TransactionCreated ev)
        {
            var cenas = new Message<string, string>(string.Empty, 0, 0, string.Empty,
                JsonConvert.SerializeObject(new MessageWrapper { Message = ev, MessageType = ev.GetType().AssemblyQualifiedName }, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }),
                new Timestamp(Timestamp.DateTimeToUnixTimestampMs(Timestamp.UnixTimeEpoch), TimestampType.NotAvailable),
                new Error(ErrorCode.BrokerNotAvailable));
            this.HandleMessage(string.Empty, cenas);
        }
    }
}