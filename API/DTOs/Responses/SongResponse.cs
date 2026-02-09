namespace MusicCatalog.Api.DTOs.Responses;

public class SongResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public int? AlbumId { get; set; }
    public string? AlbumName { get; set; }
    public List<string> Genres { get; set; }

    public static SongResponse FromEntity(MusicCatalog.Common.Entities.Song song) => new()
    {
        Id = song.Id,
        Title = song.Title,
        Duration = song.Duration,
        ArtistId = song.ArtistId,
        ArtistName = song.Artist?.StageName ?? string.Empty,
        AlbumId = song.AlbumId,
        AlbumName = song.Album?.Name,
        Genres = song.Genres?.Select(g => g.Name).ToList()
    };
}
