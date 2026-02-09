namespace MusicCatalog.Common.Entities;

public class Song : BaseEntity
{
    public string Title { get; set; } = null!;
    public TimeSpan Duration { get; set; }
    public int ArtistId { get; set; }
    public int? AlbumId { get; set; }
    public  Artist Artist { get; set; } = null!;
    public Album? Album { get; set; }
    public ICollection<Genre> Genres { get; } = new List<Genre>();
    public ICollection<Playlist> Playlists { get; } = new List<Playlist>();
}