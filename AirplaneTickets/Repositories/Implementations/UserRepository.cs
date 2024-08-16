using System.Threading.Tasks;
using AirplaneTickets.Repositories.Interfaces;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AirplaneTickets.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> CreateAsync(UserAccount user)
        {
            try
            {
                _dbContext.UserAccounts.Add(user);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (DbUpdateException ex)
            {
                // Handle duplicate email error
                return OperationResult.Failure("Email already exists.");
            }
        }

        public async Task<UserAccount> GetByIdAsync(Guid id)
        {
            return await _dbContext.UserAccounts.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserAccount> GetByEmail(string email)
        {
            return await _dbContext.UserAccounts.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<UserAccount>> FetchAllAccountsAsync()
        {
            return await _dbContext.UserAccounts.ToListAsync();
        }
    }
}
