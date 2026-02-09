namespace MusicCatalog.Common.Entities;

public class Playlist : BaseEntity
{
    public required string Name { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Song> Songs { get; } = new List<Song>();
}
