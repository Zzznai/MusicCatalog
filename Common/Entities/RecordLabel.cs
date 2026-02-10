namespace MusicCatalog.Common.Entities;

public class RecordLabel : BaseEntity
{
    public required string Name { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
    public int FoundedYear { get; set; }
    public ICollection<Artist> Artists { get; } = new List<Artist>();
}