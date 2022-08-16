using System.ComponentModel.DataAnnotations;

namespace SuggestionApplication.Application.DTO
{
    public class RegisterUserViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
