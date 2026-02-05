namespace MusicCatalog.Common.Entities;

public class RecordLabel : BaseEntity
{
    public required string Name { get; set; }
    public required string BasedIn { get; set; }
    public int FoundedYear { get; set; }
    public ICollection<Artist> Artists { get; } = new List<Artist>();
}