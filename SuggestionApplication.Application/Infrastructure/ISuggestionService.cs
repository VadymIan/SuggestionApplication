using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Infrastructure
{
    public interface ISuggestionService
    {
        Task CreateSuggestionAsync(Suggestion suggestion);

        Task UpvoteSuggestionAsync(Guid suggestionId, Guid userId);

        Task UpdateSuggestionAsync(Suggestion suggestion);

        Task<Suggestion> GetSuggestionAsync(Guid suggestionId);

        Task<IEnumerable<Suggestion>> GetAllSuggestionsAsync();

        Task<IEnumerable<Suggestion>> GetAllSuggestionsWaitingForApprovalAsync();

        Task<IEnumerable<Suggestion>> GetAllApprovedSuggestionsAsync();

        Task<IEnumerable<Suggestion>> GetUsersSuggestionsAsync(Guid userId);
    }
}
