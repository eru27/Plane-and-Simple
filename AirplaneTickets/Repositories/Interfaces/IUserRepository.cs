using System.Threading.Tasks;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;

namespace AirplaneTickets.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateAsync(UserAccount user);

        Task<UserAccount> GetByEmail(string email);

        Task<UserAccount> GetByIdAsync(Guid id);

        Task<IEnumerable<UserAccount>> FetchAllAccountsAsync();
    }
}
