using SuggestionApplication.Application.Models;

namespace SuggestionApplication.Application.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmail(Email email);
    }
}
