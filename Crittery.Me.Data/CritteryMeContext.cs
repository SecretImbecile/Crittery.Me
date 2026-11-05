namespace Crittery.Me.Data
{
    using Crittery.Me.Data.Enums;
    using Crittery.Me.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CritteryMeContext : DbContext
    {
        /// <summary>
        /// Gets or sets the bio table.
        /// </summary>
        public DbSet<BioContent> BioItems { get; set; }

        /// <summary>
        /// Gets or sets the calendar table.
        /// </summary>
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        /// <summary>
        /// Gets or sets the interests table.
        /// </summary>
        public DbSet<InterestContent> Interests { get; set; }

        /// <summary>
        /// Gets or sets the relationship status table.
        /// </summary>
        public DbSet<RelationshipContent> RelationshipItems { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source=content.db");

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<CalendarEventType>()
                .HaveConversion<string>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BioContent>()
                .ToTable(nameof(BioItems))
                .HasIndex(content => content.Order);

            modelBuilder.Entity<InterestContent>()
                .ToTable(nameof(Interests))
                .HasIndex(content => content.Order);

            modelBuilder.Entity<RelationshipContent>()
                .ToTable(nameof(RelationshipItems))
                .HasIndex(content => content.Order);
        }
    }
}
