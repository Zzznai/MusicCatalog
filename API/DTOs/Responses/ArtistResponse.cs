using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class ArtistResponse
{
    public int Id { get; set; }
    public string StageName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RecordLabelId { get; set; }
    public RecordLabel RecordLabel { get; set; } = null!;
    public ICollection<Album> Albums { get; set; } = [];
    public ICollection<Song> Songs { get; set; } = [];
    public ICollection<ArtistAward> ArtistAwards { get; set; } = [];

    public static ArtistResponse FromEntity(Artist artist) => new()
    {
        Id = artist.Id,
        StageName = artist.StageName,
        Description = artist.Description,
        RecordLabelId = artist.RecordLabelId,
        RecordLabel = artist.RecordLabel,
        Albums = artist.Albums,
        Songs = artist.Songs,
        ArtistAwards = artist.ArtistAwards
    };
}
