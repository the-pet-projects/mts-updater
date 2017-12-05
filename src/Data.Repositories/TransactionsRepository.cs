namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using System.Threading.Tasks;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public class TransactionsRepository : ITransactionsRepository
    {
        public Task AddAsync(Transaction transaction)
        {
            throw new System.NotImplementedException();
        }
    }
}
