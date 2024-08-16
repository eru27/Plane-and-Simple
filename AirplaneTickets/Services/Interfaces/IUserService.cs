using System.Security.Claims;
using System.Threading.Tasks;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Models.ViewModels;

namespace AirplaneTickets.Services.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult> CreateAccountAsync(CreateAccountViewModel model);

        Task<ClaimsIdentity> CreateIdentitiyAsync(LoginViewModel model);

        Task<IEnumerable<UserAccount>> FetchAllAccountsAsync();

        Task<UserAccount> GetAccountByIdAsync(Guid id);

        Task<UserAccount> GetAccountByEmailAsync(string email);
    }
}
