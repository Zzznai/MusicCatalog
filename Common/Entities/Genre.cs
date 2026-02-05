namespace MusicCatalog.Common.Entities;

public class Genre : BaseEntity
{
    public required string Name {get; set; }
    public ICollection<Song> Songs {get;} = new List<Song>();
}
