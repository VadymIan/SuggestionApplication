using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuggestionApplication.Domain.Entities
{
    public class Suggestion
    {
        public Guid Id { get; set; }

        [MaxLength(40)]
        public string Text { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [ForeignKey("Categoty_Id")]
        public Category Categoty { get; set; }

        [ForeignKey("Author_Id")]
        public User Author { get; set; }
        public HashSet<User> UserVotes { get; set; } = new();

        [ForeignKey("Status_Id")]
        public Status Status { get; set; }

        [MaxLength(100)]
        public string OwnerNotes { get; set; }
        public bool ApprovedForRelease { get; set; } = false;
        public bool Archived { get; set; } = false;
        public bool Rejected { get; set; }
    }
}
