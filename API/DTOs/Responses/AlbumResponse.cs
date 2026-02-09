using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class AlbumResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;

    public List<string> Moods {get;set;}

    public List<string> Songs {get; set;}

    public static AlbumResponse FromEntity(MusicCatalog.Common.Entities.Album album) => new()
    {
        Id = album.Id,
        Name = album.Name,
        Description = album.Description,
        ArtistId = album.ArtistId,
        ArtistName = album.Artist?.StageName ?? string.Empty,
        Songs = album.Songs?.Select(s => $"{s.Title} - {s.Duration}").ToList(),
        Moods = album.Moods?.Select(m=>m.Name).ToList()
    };
}
