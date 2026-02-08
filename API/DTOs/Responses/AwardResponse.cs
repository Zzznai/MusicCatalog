namespace MusicCatalog.Api.DTOs.Responses;

public class AwardResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }

    public List<string> ArtistName {get; set;}
    public static AwardResponse FromEntity(MusicCatalog.Common.Entities.Award award) => new()
    {
        Id = award.Id,
        Name = award.Name,
        Year = award.Year,
        ArtistName = award.Artists.Select(a => a.StageName).ToList()
    };
}
