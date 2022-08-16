using System.ComponentModel.DataAnnotations;

namespace SuggestionApplication.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(15)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string DisplayName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsAdmin { get; set; }
        public bool EmailIsConfirmed { get; set; }
        public List<Suggestion> AuthoredSuggestions { get; set; } = new();
        public List<Suggestion> VotedOnSuggestions { get; set; } = new();
    }
}
