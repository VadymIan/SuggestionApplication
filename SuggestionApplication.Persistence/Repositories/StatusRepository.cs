using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Persistence.Repositories
{
    public class StatusRepository : BaseRepository<Status>, IStatusRepository
    {
        public StatusRepository(SuggestionApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
