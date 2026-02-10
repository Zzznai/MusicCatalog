using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class MoodResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MoodAlbumResponse> Albums { get; set; } = [];

    public static MoodResponse FromEntity(Mood mood) => new()
    {
        Id = mood.Id,
        Name = mood.Name,
        Albums = mood.Albums.Select(a => new MoodAlbumResponse
        {
            Id = a.Id,
            Name = a.Name
        }).ToList()
    };
}

public class MoodAlbumResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
