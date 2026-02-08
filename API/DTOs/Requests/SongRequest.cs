namespace MusicCatalog.Api.DTOs.Requests;

public class CreateSongRequest
{
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int ArtistId { get; set; }
    public int? AlbumId { get; set; }
}

public class UpdateSongRequest
{
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int ArtistId { get; set; }
    public int? AlbumId { get; set; }
}
