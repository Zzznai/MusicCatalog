using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public abstract class BaseService
{
    protected readonly ApplicationDbContext _context;

    protected BaseService(ApplicationDbContext context)
    {
        _context = context;
    }
}