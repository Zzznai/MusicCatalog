using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class AwardResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ArtistAward> ArtistAwards { get; set; } = [];

    public static AwardResponse FromEntity(Award award) => new()
    {
        Id = award.Id,
        Name = award.Name,
        ArtistAwards = award.ArtistAwards
    };
}
