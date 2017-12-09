namespace PetProjects.MicroTransactionsUpdater.Domain.Model
{
    using System;

    public class Transaction
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public long EpochTimestamp { get; set; }

        public int Quantity { get; set; }

        public Guid ItemId { get; set; }
    }
}