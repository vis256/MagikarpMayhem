namespace MagikarpMayhem.Models;

public class Battle
{
    public int Id { get; set; }
    public int AttackerId { get; set; }
    public int DefenderId { get; set; }
    public int WinnerId { get; set; }
    public DateTime Date { get; set; }
    public int AttackerPokemonId { get; set; }
    public int DefenderPokemonId { get; set; }
    public int ArenaId { get; set; }
}