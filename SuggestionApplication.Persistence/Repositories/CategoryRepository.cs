using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(SuggestionApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
