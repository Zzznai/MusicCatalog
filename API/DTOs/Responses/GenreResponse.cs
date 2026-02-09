using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class GenreResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<string> Songs { get; set; } = [];

    public static GenreResponse FromEntity(Genre genre) => new()
    {
        Id = genre.Id,
        Name = genre.Name,
        Songs = genre.Songs?.Select(s=>s.Title).ToList()
    };
}
