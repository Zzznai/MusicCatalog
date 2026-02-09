using Common.Enums;
using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class UserResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Playlists { get; set; }

    public static UserResponse FromEntity(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        Role = user.Role.ToString(),
        Playlists = user.Playlists?.Select(p => p.Name).ToList()
    };
}
