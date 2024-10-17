namespace MagikarpMayhem.Models;

public class PokemonTypeCounter
{
    public int Id { get; set; }
    
    // List of PokemonType Ids that Id is strong against
    public ICollection<int> Counters { get; set; }
}