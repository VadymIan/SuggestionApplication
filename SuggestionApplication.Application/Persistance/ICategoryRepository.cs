using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Persistance
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
    }
}
