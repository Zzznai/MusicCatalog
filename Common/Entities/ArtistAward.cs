namespace MusicCatalog.Common.Entities;

public class ArtistAward
{
    public int ArtistId { get; set; }
    public int AwardId { get; set; }
    public int Year { get; set; }
    public required Artist Artist { get; set; }
    public required Award Award { get; set; }
}
