// <copyright file="CritteryMeContext.cs" company="Jack Kelly">
// Copyright © 2026 Jack Kelly.
//
// The crittery.me website is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation, either version 3 of the License, or
// (at your option)any later version.
//
// The crittery.me source code is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.
//
// You should have received a copy of the GNU General Public License along with
// The crittery.me source code. If not, see [https://www.gnu.org/licenses/].
// </copyright>

namespace Crittery.Me.Data
{
    using Crittery.Me.Data.Enums;
    using Crittery.Me.Data.Models;
    using Crittery.Me.Data.Models.Images;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The database context for the crittery.me website.
    /// </summary>
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
        /// Gets or sets the links table.
        /// </summary>
        public DbSet<Link> Links { get; set; }

        /// <summary>
        /// Gets or sets the photos table.
        /// </summary>
        public DbSet<Photo> Photos { get; set; }

        /// <summary>
        /// Gets or sets the relationship status table.
        /// </summary>
        public DbSet<RelationshipContent> RelationshipItems { get; set; }

        /// <summary>
        /// Gets or sets the relationship status table.
        /// </summary>
        public DbSet<Visitor> Visitors { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source=content.db");

        /// <inheritdoc/>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<CalendarEventType>()
                .HaveConversion<string>();
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BioContent>()
                .ToTable(nameof(this.BioItems))
                .HasIndex(content => content.Order);

            modelBuilder.Entity<InterestContent>()
                .ToTable(nameof(this.Interests))
                .HasIndex(content => content.Order);

            modelBuilder.Entity<RelationshipContent>()
                .ToTable(nameof(this.RelationshipItems))
                .HasIndex(content => content.Order);

            modelBuilder.Entity<Link>()
                .ToTable(nameof(this.Links))
                .HasIndex(content => content.Order);

            modelBuilder.Entity<Link>()
                .ToTable(nameof(this.Links))
                .OwnsOne(link => link.Icon, builder =>
                    builder.OwnsMany(image => image.Files)
                    .WithOwner(file => file.Image)
                    .HasForeignKey(file => file.ImageId));

            modelBuilder.Entity<Photo>()
                .ToTable(nameof(this.Photos))
                .HasIndex(photo => photo.Stub)
                .IsUnique();

            modelBuilder.Entity<Photo>()
                .ToTable(nameof(this.Photos))
                .HasIndex(photo => photo.CapturedOn)
                .IsDescending();

            modelBuilder.Entity<Photo>()
                .ToTable(nameof(this.Photos))
                .OwnsOne(photo => photo.Metadata);

            modelBuilder.Entity<Photo>()
                .ToTable(nameof(this.Photos))
                .OwnsOne(link => link.ImageFullSize, builder =>
                    builder.OwnsMany(image => image.Files)
                    .ToTable("Photos_Files")
                    .WithOwner(file => file.Image)
                    .HasForeignKey(file => file.ImageId));

            modelBuilder.Entity<Photo>()
                .ToTable(nameof(this.Photos))
                .OwnsOne(link => link.ImageThumbnail, builder =>
                    builder.OwnsMany(image => image.Files)
                    .ToTable("Photos_Thumbnails")
                    .WithOwner(file => file.Image)
                    .HasForeignKey(file => file.ImageId));

            modelBuilder.Entity<Visitor>()
                .ToTable(nameof(this.Visitors))
                .HasIndex(visitor => new { visitor.Page, visitor.Referrer })
                .IsUnique();
        }
    }
}
