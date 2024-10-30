using MagikarpMayhem.Data;
using MagikarpMayhem.Models;

namespace MagikarpMayhem.Services;

public interface IPokemonTypeService
{
    PokemonType? GetPokemonTypeByName(string name);
}

public class PokemonTypeService : IPokemonTypeService
{
    private readonly MagikarpMayhemContext _context;

    public PokemonTypeService(MagikarpMayhemContext context)
    {
        _context = context;
    }

    public PokemonType? GetPokemonTypeByName(string name)
    {
        return _context.PokemonType.Find(name);
    }
}