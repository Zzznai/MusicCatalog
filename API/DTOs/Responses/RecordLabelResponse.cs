using MusicCatalog.Common.Entities;

namespace MusicCatalog.Api.DTOs.Responses;

public class RecordLabelResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BasedIn { get; set; } = string.Empty;
    public int FoundedYear { get; set; }
    public ICollection<Artist> Artists { get; set; } = [];

    public static RecordLabelResponse FromEntity(RecordLabel label) => new()
    {
        Id = label.Id,
        Name = label.Name,
        BasedIn = label.BasedIn,
        FoundedYear = label.FoundedYear,
        Artists = label.Artists
    };
}

