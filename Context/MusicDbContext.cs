using Microsoft.EntityFrameworkCore;
using SongsInLearning.Models;

namespace SongsInLearning.Database;
public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

    public DbSet<Music> Musics { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Music>(entity =>
        {
            entity.Property(m => m.Name).HasMaxLength(100).IsRequired();

            entity.Property(m => m.Artist).HasMaxLength(100).IsRequired();

            entity.Property(m => m.Year).IsRequired();

            entity.Property(m => m.Bpm).HasPrecision(6, 2);

            entity.Property(m => m.UserAnnotations).HasMaxLength(4000);

            entity.Property(m => m.Difficulty).HasConversion<string>();

            entity.Property(m => m.Tuning).HasConversion<string>();

            entity.Property(m => m.Progress).HasConversion<string>();

            entity.Property(m => m.Instrument).HasConversion<string>();

            entity.Property(m => m.CreatedAt).IsRequired();
        });
    }
}
