namespace MusicCatalog.Common.Entities;

public class Mood : BaseEntity
{
    public required string Name { get; set; }
    public ICollection<Album> Albums { get; } = new List<Album>();
}
