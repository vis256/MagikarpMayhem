using MagikarpMayhem.Data;
using MagikarpMayhem.Models;

namespace MagikarpMayhem.Services;

public class PokedexService
{
    private readonly MagikarpMayhemContext _context;

    public PokedexService(MagikarpMayhemContext context)
    {
        _context = context;
    }

    public List<PokedexInfo> GetAllPokedexInfos()
    {
        return _context.PokedexInfo.ToList();
    }

    public PokedexInfo GetPokedexInfo(int id)
    {
        return _context.PokedexInfo.FirstOrDefault(p => p.Id == id);
    }
}