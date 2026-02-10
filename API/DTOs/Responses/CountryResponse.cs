using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class CountryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<string> RecordLabels { get; set; } = [];

    public static CountryResponse FromEntity(Country country) => new()
    {
        Id = country.Id,
        Name = country.Name,
        RecordLabels = country.RecordLabels?.Select(r => r.Name).ToList() ?? []
    };
}
