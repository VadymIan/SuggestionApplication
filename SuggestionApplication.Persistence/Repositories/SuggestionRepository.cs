using Microsoft.EntityFrameworkCore;
using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Persistence.Repositories
{
    public class SuggestionRepository : BaseRepository<Suggestion>, ISuggestionRepository
    {
        public SuggestionRepository(SuggestionApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public override async Task<Suggestion> AddAsync(Suggestion entity)
        {
            await _dbContext.Set<Suggestion>().AddAsync(entity);
            _dbContext.Entry(entity.Categoty).State = EntityState.Unchanged;
            _dbContext.Entry(entity.Status).State = EntityState.Unchanged;
            _dbContext.Entry(entity.Author).State = EntityState.Unchanged;
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public override async Task<Suggestion> GetByIdAsync(Guid guid)
        {
            return await _dbContext.Suggestions
                .Include(c => c.Categoty)
                .Include(s => s.Status)
                .Include(u => u.UserVotes)
                .Include(a => a.Author)
                .Where(s => !s.Archived)
                .FirstOrDefaultAsync(x => x.Id == guid);
        }

        public async Task<IEnumerable<Suggestion>> GetAllWithDependencies()
        {
            return await _dbContext.Suggestions
                .Include(c => c.Categoty)
                .Include(s => s.Status)
                .Include(u => u.UserVotes)
                .Include(a => a.Author)
                .Where(s => !s.Archived)
                .ToListAsync();
        }

        public async Task<IEnumerable<Suggestion>> GetUsersSuggestions(Guid userId)
        {
            return await _dbContext.Suggestions
                .Include(c => c.Categoty)
                .Include(s => s.Status)
                .Include(u => u.UserVotes)
                .Include(a => a.Author)
                .Where(s => s.Author.Id == userId)
                .ToListAsync();
        }
    }
}
