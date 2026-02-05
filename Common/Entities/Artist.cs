namespace MusicCatalog.Common.Entities;

public class Artist : BaseEntity
{
    public required string StageName { get; set; }
    public string? Description { get; set; }
    public int RecordLabelId { get; set; }
    public required RecordLabel RecordLabel { get; set; }
    public ICollection<Album> Albums { get; } = new List<Album>();
    public ICollection<Song> Songs { get; } = new List<Song>();
    public ICollection<ArtistAward> ArtistAwards { get; } = new List<ArtistAward>();
}