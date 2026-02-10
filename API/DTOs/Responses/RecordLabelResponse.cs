using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class RecordLabelResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public int FoundedYear { get; set; }

    public static RecordLabelResponse FromEntity(RecordLabel label) => new()
    {
        Id = label.Id,
        Name = label.Name,
        CountryId = label.CountryId,
        CountryName = label.Country?.Name ?? string.Empty,
        FoundedYear = label.FoundedYear
    };
}

