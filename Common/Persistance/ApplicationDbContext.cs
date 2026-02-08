using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;

namespace MusicCatalog.Common.Persistance;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Award> Awards { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Mood> Moods { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<RecordLabel> RecordLabels { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Artist>()
            .HasMany(a => a.Awards)
            .WithMany(a => a.Artists);
    }
}
