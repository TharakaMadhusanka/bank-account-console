namespace Domain.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Accounts>
    {
        Task<Accounts?> GetAccountByAccountNoAsync(string accountNo);
    }
}
