namespace MagikarpMayhem.Models;

public class ArenaBattleRecord
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArenaId { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
}