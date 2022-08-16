using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Infrastructure
{
    public interface ICategoryService
    {
        Task CreateCategotyAsync(Category category);

        ValueTask<List<Category>> GetAllCategoriesAsync();
    }
}
