namespace PetProjects.MicroTransactionsUpdater.Data.Repositories
{
    using System.Threading.Tasks;
    using PetProjects.MicroTransactionsUpdater.Domain.Model;

    public interface ITransactionsRepository
    {
        Task AddAsync(Transaction transaction);
    }
}