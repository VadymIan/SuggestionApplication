using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SuggestionApplication.Application.Exceptions;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Infrastructure.Services
{
    public class SuggestionService : ISuggestionService
    {
        private readonly ILogger<SuggestionService> _logger;
        private readonly ISuggestionRepository _suggestionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;

        private const string CacheName = "SuggestionData";

        public SuggestionService(ILogger<SuggestionService> logger, ISuggestionRepository suggestionRepository, IUserRepository userRepository, IMemoryCache cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _suggestionRepository = suggestionRepository ?? throw new ArgumentNullException(nameof(suggestionRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task CreateSuggestionAsync(Suggestion incomingSuggestion)
        {
            await _suggestionRepository.AddAsync(incomingSuggestion);
        }

        public async Task<IEnumerable<Suggestion>> GetAllApprovedSuggestionsAsync()
        {
            var suggestions = await GetAllSuggestionsAsync();
            return suggestions.Where(s => s.ApprovedForRelease).ToList();
        }

        public async Task<IEnumerable<Suggestion>> GetAllSuggestionsAsync()
        {
            var output = _cache.Get<List<Suggestion>>(CacheName);

            if (output is null)
            {
                var suggestions = await _suggestionRepository.GetAllWithDependencies();

                _cache.Set(CacheName, suggestions, TimeSpan.FromMinutes(1));

                output = (List<Suggestion>?)suggestions;
            }

            return output ?? new List<Suggestion>();
        }

        public async Task<IEnumerable<Suggestion>> GetAllSuggestionsWaitingForApprovalAsync()
        {
            var suggestions = await GetAllSuggestionsAsync();
            return suggestions.Where(s => s.ApprovedForRelease && !s.Rejected).ToList();
        }

        public async Task<Suggestion> GetSuggestionAsync(Guid suggestionId)
        {
            var suggestion = await _suggestionRepository.GetByIdAsync(suggestionId);

            if (suggestion == null)
            {
                throw new NotFoundException(nameof(Suggestion), suggestionId);
            }

            return suggestion;
        }

        public async Task<IEnumerable<Suggestion>> GetUsersSuggestionsAsync(Guid userId)
        {
            var output = _cache.Get<List<Suggestion>>(userId);

            if (output is null)
            {
                var suggestions = await _suggestionRepository.GetUsersSuggestions(userId);

                _cache.Set(userId, suggestions, TimeSpan.FromMinutes(1));

                output = (List<Suggestion>?)suggestions;
            }

            return output ?? new List<Suggestion>();
        }

        public async Task UpdateSuggestionAsync(Suggestion incomingSuggestion)
        {
            var suggestion = await _suggestionRepository.GetByIdAsync(incomingSuggestion.Id);

            if (suggestion is null)
            {
                throw new NotFoundException(nameof(Suggestion), incomingSuggestion.Id);
            }

            await _suggestionRepository.UpdateAsync(suggestion);

            _cache.Remove(CacheName);
        }

        public async Task UpvoteSuggestionAsync(Guid suggestionId, Guid userId)
        {
            var suggestion = await _suggestionRepository.GetByIdAsync(suggestionId);
            var user = await _userRepository.GetByIdAsync(userId);

            if (suggestion is null)
            {
                throw new NotFoundException(nameof(Suggestion), suggestionId);
            }
            if (user is null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var votedUser = suggestion.UserVotes.FirstOrDefault(x => x.Id == userId);

            if (votedUser is null)
            {
                suggestion.UserVotes.Add(user);
            }
            else
            {
                suggestion.UserVotes.Remove(user);
            }

            await _suggestionRepository.UpdateAsync(suggestion);

            _cache.Remove(CacheName);
        }
    }
}
