using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class ArtistResponse
{
    public int Id { get; set; }
    public string StageName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RecordLabelId { get; set; }
    public string RecordLabelName { get; set; } = string.Empty;

    public static ArtistResponse FromEntity(Artist artist) => new()
    {
        Id = artist.Id,
        StageName = artist.StageName,
        Description = artist.Description,
        RecordLabelId = artist.RecordLabelId,
        RecordLabelName = artist.RecordLabel.Name
    };
}
