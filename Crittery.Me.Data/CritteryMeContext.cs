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
        public DbSet<BioItem> BioItems { get; set; }

        /// <summary>
        /// Gets or sets the calendar table.
        /// </summary>
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        /// <summary>
        /// Gets or sets the interests table.
        /// </summary>
        public DbSet<Interest> Interests { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source=content.db");

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<CalendarEventType>()
                .HaveConversion<string>();
        }
    }
}
