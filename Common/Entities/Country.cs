namespace MusicCatalog.Common.Entities;

public class Country : BaseEntity
{
    public required string Name { get; set; }
    public ICollection<RecordLabel> RecordLabels { get; } = new List<RecordLabel>();
}
