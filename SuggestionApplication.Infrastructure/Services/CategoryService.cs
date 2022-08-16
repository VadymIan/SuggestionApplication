using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMemoryCache _cache;

        private const string CacheName = "CategoryData";

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, IMemoryCache cache)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task CreateCategotyAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async ValueTask<List<Category>> GetAllCategoriesAsync()
        {
            var output = _cache.Get<List<Category>>(CacheName);

            if (output is null)
            {
                var categories = await _categoryRepository.GetAllAsync();

                output = categories.ToList();

                _cache.Set(CacheName, output, TimeSpan.FromDays(1));
            }

            return output;
        }
    }
}
