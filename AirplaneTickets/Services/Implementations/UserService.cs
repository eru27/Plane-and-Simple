using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using AirplaneTickets.Models.ViewModels;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Services.Interfaces;
using AirplaneTickets.Repositories.Interfaces;
using AirplaneTickets.Models.Entities;
using Sodium;
using AirplaneTickets.Models.Enums;


namespace AirplaneTickets.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> CreateAccountAsync(CreateAccountViewModel model)
        {
            if (model.Type == UserAccountType.Admin)
            {
                return OperationResult.Failure("Cannot create an admin account");
            }
            var user = new UserAccount
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Type = model.Type
            };

            var hash = PasswordHash.ScryptHashString(model.Password, PasswordHash.Strength.Medium);
            user.PasswordHash = hash;

            var result = await _userRepository.CreateAsync(user);

            return result;
        }

        public async Task<ClaimsIdentity> CreateIdentitiyAsync(LoginViewModel model)
        {
            var user = await _userRepository.GetByEmail(model.Email);

            if (user == null)
            {
                return null;
            }

            var isValid = PasswordHash.ScryptHashStringVerify(user.PasswordHash, model.Password);

            if (!isValid)
            {
                return null;
            }

            // Give cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Type.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return identity;
        }

        public async Task<IEnumerable<UserAccount>> FetchAllAccountsAsync()
        {
            var users = await _userRepository.FetchAllAccountsAsync();

            if (users == null)
            {
                return Enumerable.Empty<UserAccount>(); // Return an empty enumerable if flights is null
            }

            return users.OrderBy(u => u.Type).ThenBy(u => u.LastName).ThenBy(u => u.FirstName);
        }

        public async Task<UserAccount> GetAccountByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return user;
        }

        public async Task<UserAccount> GetAccountByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            return user;
        }
    }
}
