namespace MusicCatalog.Common.Entities;

public class Award : BaseEntity
{
    public required string Name { get; set; }
    public ICollection<ArtistAward> ArtistAwards { get; } = new List<ArtistAward>();
}
