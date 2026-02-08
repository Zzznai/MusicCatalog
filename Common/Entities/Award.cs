namespace MusicCatalog.Common.Entities;

public class Award : BaseEntity
{
    public required string Name { get; set; }
    public int Year { get; set; }
    public ICollection<Artist> Artists { get; } = new List<Artist>();
}
