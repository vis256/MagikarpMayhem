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

        public DbSet<MagikarpMayhem.Models.User> User { get; set; } = default!;
    }
}
