namespace MagikarpMayhem.Models;

public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PokedexNumber { get; set; }
    public int Level { get; set; }
    public int OwnerId { get; set; }
}