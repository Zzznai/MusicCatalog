namespace MusicCatalog.Api.DTOs.Responses;

public class PlaylistResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<string> Songs { get; set; }

    public static PlaylistResponse FromEntity(MusicCatalog.Common.Entities.Playlist playlist) => new()
    {
        Id = playlist.Id,
        Name = playlist.Name,
        UserId = playlist.UserId,
        Username = playlist.User?.Username ?? string.Empty,
        Songs = playlist.Songs?.Select(s=>$"{s.Title} -> {s.Artist?.StageName}").ToList()
    };
}
