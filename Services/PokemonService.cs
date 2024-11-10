using MagikarpMayhem.Data;
using MagikarpMayhem.Models;

namespace MagikarpMayhem.Services;

public class PokemonService
{
    private readonly MagikarpMayhemContext _context;

    public PokemonService(MagikarpMayhemContext context)
    {
        _context = context;
    }

    public async Task AddPokemon(Pokemon pokemon)
    {
        _context.Pokemon.Add(pokemon);
        await _context.SaveChangesAsync();
    }

    public async Task RemovePokemon(int id)
    {
        var pokemon = await _context.Pokemon.FindAsync(id);
        if (pokemon != null)
        {
            _context.Pokemon.Remove(pokemon);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditPokemon(PokemonUpdateDTO updatedPokemon)
    {
        var existingPokemon = await _context.Pokemon.FindAsync(updatedPokemon.Id);
        if (existingPokemon != null)
        {
            existingPokemon.Name = updatedPokemon.Name;
            existingPokemon.Level = updatedPokemon.Level;
            await _context.SaveChangesAsync();
        }
    }

    public List<Pokemon> GetUserPokemons(int userId)
    {
        return _context.Pokemon.Where(p => p.OwnerId == userId).ToList();
    }
}