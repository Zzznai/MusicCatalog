namespace MusicCatalog.Common.Entities;

public class Artist : BaseEntity
{
    public required string StageName { get; set; }
    public string? Description { get; set; }
    public int RecordLabelId { get; set; }
    public RecordLabel RecordLabel { get; set; } = null!;
    public ICollection<Album> Albums { get; } = new List<Album>();
    public ICollection<Song> Songs { get; } = new List<Song>();
    public ICollection<ArtistAward> ArtistAwards { get; } = new List<ArtistAward>();
}