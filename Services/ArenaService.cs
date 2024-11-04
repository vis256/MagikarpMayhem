using MagikarpMayhem.Data;
using MagikarpMayhem.Models;
using Microsoft.EntityFrameworkCore;

namespace MagikarpMayhem.Services;

public class ArenaService
{
    private readonly MagikarpMayhemContext _context;

    public ArenaService(MagikarpMayhemContext context)
    {
        _context = context;
    }

    public async Task<bool> JoinArena(int userId, int arenaId)
    {
        var user = await _context.User.FindAsync(userId);
        var arena = await _context.Arenas.FindAsync(arenaId);
        if (user == null || arena == null)
        {
            return false;
        }

        // Check if user is already part of an arena
        var existingMembership = _context.ArenaMemberships.FirstOrDefault(am => am.UserId == userId);
        if (existingMembership != null)
        {
            return false;
        }

        var membership = new ArenaMembership { UserId = userId, ArenaId = arenaId };
        _context.ArenaMemberships.Add(membership);
        var battleRecord = new ArenaBattleRecord { UserId = userId, ArenaId = arenaId, Wins = 0, Losses = 0 };
        _context.ArenaBattleRecords.Add(battleRecord);
        await _context.SaveChangesAsync();
        await UpdateArenaLeader(arenaId);
        return true;
    }

    public async Task<bool> LeaveArena(int userId, int arenaId)
    {
        var membership = _context.ArenaMemberships.FirstOrDefault(am => am.UserId == userId && am.ArenaId == arenaId);
        if (membership == null)
        {
            return false;
        }

        _context.ArenaMemberships.Remove(membership);
        await _context.SaveChangesAsync();
        await UpdateArenaLeader(arenaId);
        return true;
    }

    public async Task<IEnumerable<Arena>> GetArenas()
    {
        var arenas = await _context.Arenas.ToListAsync();
        return arenas;
    }

    public async Task<Arena?> GetArenaById(int arenaId)
    {
        return await _context.Arenas.FindAsync(arenaId);
    }

    public async Task<ArenaMembership?> GetArenaOfUser(int userId)
    {
        return await _context.ArenaMemberships.FirstOrDefaultAsync(am => am.UserId == userId);
    }

    public async Task<ArenaBattleRecord?> GetArenaBattleRecordOfUser(int userId, int arenaId)
    {
        return await _context.ArenaBattleRecords.FirstOrDefaultAsync(abr =>
            abr.UserId == userId && abr.ArenaId == arenaId);
    }

    public async Task<List<int>> GetUsersOfArena(int arenaId)
    {
        return await _context.ArenaMemberships.Where(am => am.ArenaId == arenaId).Select(am => am.UserId).ToListAsync();
    }

    private async Task UpdateArenaLeader(int arenaId)
    {
        // Implementation for updating the arena leader
        var leader = _context.ArenaBattleRecords.Where(abr => abr.ArenaId == arenaId).OrderByDescending(abr => abr.Wins)
            .Select(abr => abr.UserId).FirstOrDefault();
        if (leader != 0)
        {
            var arena = await _context.Arenas.FindAsync(arenaId);
            if (arena != null)
            {
                arena.LeaderId = leader;
                await _context.SaveChangesAsync();
            }
        }
    }
}