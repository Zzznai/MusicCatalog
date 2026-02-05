namespace MusicCatalog.Common.Entities;

public class Song : BaseEntity
{
    public required string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public int ArtistId { get; set; }
    public int? AlbumId { get; set; }
    public required Artist Artist { get; set; }
    public Album? Album { get; set; }
    public ICollection<Genre> Genres { get; } = new List<Genre>();
    public ICollection<Playlist> Playlists { get; } = new List<Playlist>();
}