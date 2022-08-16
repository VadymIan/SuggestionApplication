using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Persistance
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetUserByEmail(string value);
    }
}
