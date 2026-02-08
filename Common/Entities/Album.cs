namespace MusicCatalog.Common.Entities;

public class Album : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;
    public ICollection<Song> Songs{get;} = new List<Song>();
    public ICollection<Mood> Moods{get;} = new List<Mood>();
}