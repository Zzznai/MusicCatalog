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
    public DbSet<ArtistAward> ArtistAwards { get; set; }
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

        modelBuilder.Entity<ArtistAward>()
            .HasKey(aa => new { aa.ArtistId, aa.AwardId, aa.Year});

        modelBuilder.Entity<ArtistAward>()
            .HasOne(aa => aa.Artist)
            .WithMany(a => a.ArtistAwards)
            .HasForeignKey(aa => aa.ArtistId);

        modelBuilder.Entity<ArtistAward>()
            .HasOne(aa => aa.Award)
            .WithMany(a => a.ArtistAwards)
            .HasForeignKey(aa => aa.AwardId);
    }
}
