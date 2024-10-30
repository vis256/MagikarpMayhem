using System.ComponentModel.DataAnnotations;

namespace MagikarpMayhem.Models;

public class PokemonType
{
    [Key]
    public string Name { get; set; }    
    public string Color { get; set; }
    public string Emoji { get; set; }
}