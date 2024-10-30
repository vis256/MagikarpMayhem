using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MagikarpMayhem.Models;

namespace MagikarpMayhem.Data
{
    public class MagikarpMayhemContext : DbContext
    {
        public MagikarpMayhemContext (DbContextOptions<MagikarpMayhemContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<PokemonType> PokemonType { get; set; } = default!;
        
        public DbSet<Pokemon> Pokemon { get; set; } = default!;
        public DbSet<PokedexInfo> PokedexInfo { get; set; } = default!;
        
        public DbSet<Arena> Arenas { get; set; } = default!;
        
        public DbSet<Battle> Battles { get; set; } = default!;
        
        public DbSet<ArenaMembership> ArenaMemberships { get; set; } = default!;
        
        public DbSet<PokemonTypeCounter> PokemonTypeCounters { get; set; } = default!;
        
        
    }
}
