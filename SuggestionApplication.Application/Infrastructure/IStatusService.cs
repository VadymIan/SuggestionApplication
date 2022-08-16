using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Application.Infrastructure
{
    public interface IStatusService
    {
        Task CreateStatusAsync(Status category);

        Task<List<Status>> GetAllStatusesAsync();
    }
}
