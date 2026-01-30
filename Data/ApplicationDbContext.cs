using Microsoft.EntityFrameworkCore;
using RickAndMortyBackend.Models;

namespace RickAndMortyBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("Characters");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever(); // Use API Id
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Species).HasMaxLength(100);
                entity.Property(e => e.Type).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(50);
                entity.Property(e => e.Image).HasMaxLength(500);
                entity.Property(e => e.Url).HasMaxLength(500);
                entity.Property(e => e.OriginName).HasMaxLength(255);
                entity.Property(e => e.OriginUrl).HasMaxLength(500);
                entity.Property(e => e.LocationName).HasMaxLength(255);
                entity.Property(e => e.LocationUrl).HasMaxLength(500);
                entity.Property(e => e.EpisodesJson).HasColumnType("TEXT");
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Species);
            });
        }
    }
}
