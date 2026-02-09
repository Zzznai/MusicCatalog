using Common.Enums;

namespace MusicCatalog.Common.Entities;

public class User : BaseEntity
{
    public required string Username { get; set; }
    public string PasswordHash { get; set; } = null!;
    public Role Role { get; set; }
    public ICollection<Playlist> Playlists { get; } = new List<Playlist>();
}

