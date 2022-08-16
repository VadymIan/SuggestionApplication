using System.ComponentModel.DataAnnotations;

namespace SuggestionApplication.Application.DTO
{
    public class LoginUserViewModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
