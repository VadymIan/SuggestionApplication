using Microsoft.EntityFrameworkCore;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Persistence
{
    public class SuggestionApplicationDbContext : DbContext
    {
        public SuggestionApplicationDbContext(DbContextOptions<SuggestionApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Suggestion>()
                .HasOne(p => p.Author)
                .WithMany(t => t.AuthoredSuggestions);

            modelBuilder.Entity<Suggestion>()
                .HasMany(p => p.UserVotes)
                .WithMany(t => t.VotedOnSuggestions);
        }
    } 
}
