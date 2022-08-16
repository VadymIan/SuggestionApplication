using Microsoft.EntityFrameworkCore;
using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(SuggestionApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<User> GetByIdAsync(Guid guid)
        {
            return await _dbContext.Users
                .Include(a => a.AuthoredSuggestions)
                .Include(v => v.VotedOnSuggestions)
                .FirstOrDefaultAsync(x => x.Id == guid);
        }

        public async Task<User> GetUserByEmail(string value)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.EmailAddress == value);
        }
    }
}
