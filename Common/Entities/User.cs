using Common.Enums;

namespace MusicCatalog.Common.Entities;

public class User : BaseEntity
{
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public Role Role { get; set; }
    public ICollection<Playlist> Playlists { get; } = new List<Playlist>();
}

