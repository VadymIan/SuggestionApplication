using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Persistance
{
    public interface ISuggestionRepository : IAsyncRepository<Suggestion>
    {
        Task<IEnumerable<Suggestion>> GetAllWithDependencies();

        Task<IEnumerable<Suggestion>> GetUsersSuggestions(Guid userId);
    }
}
