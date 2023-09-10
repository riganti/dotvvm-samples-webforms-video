using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Altairis.VtipBaze.Data
{
    public class VtipBazeContext : IdentityDbContext
    {

        public DbSet<Joke> Jokes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public VtipBazeContext()
        {
        }

        public VtipBazeContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS; Integrated Security=True; Initial Catalog=VtipBazeMigrated; MultipleActiveResultSets=True; Trust Server Certificate=true");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Joke>()
                .HasMany(j => j.Tags)
                .WithMany(t => t.Jokes)
                .UsingEntity<Dictionary<string, object>>(
                    "TagJokes",
                    b => b.HasOne<Tag>().WithMany().HasForeignKey("Tag_TagName"),
                    b => b.HasOne<Joke>().WithMany().HasForeignKey("Joke_JokeId"));

            base.OnModelCreating(builder);
        }

    }
}
