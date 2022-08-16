using SuggestionApplication.Application.DTO;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Infrastructure
{
    public interface IUserService
    {
        Task<User> Register(RegisterUserViewModel registerUser);
        Task<string> Login(LoginUserViewModel loginUser);
        Task UpdateUser(User user);
        Task<User> GetUserById(Guid userId);
        Task ComfirmEmail(Guid userId);
    }
}
