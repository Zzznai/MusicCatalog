namespace MusicCatalog.Common.Entities;

public class Playlist : BaseEntity
{
    public required string Name { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
    public ICollection<Song> Songs { get; } = new List<Song>();
}
