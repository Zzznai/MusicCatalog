using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class MoodResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Album> Albums { get; set; } = [];

    public static MoodResponse FromEntity(Mood mood) => new()
    {
        Id = mood.Id,
        Name = mood.Name,
        Albums = mood.Albums
    };
}
