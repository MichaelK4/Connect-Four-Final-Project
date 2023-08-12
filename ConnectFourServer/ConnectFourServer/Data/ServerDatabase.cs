using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ConnectFourServer.Data
{
    public class ServerDatabase : DbContext
    {
        public ServerDatabase(DbContextOptions<ServerDatabase> options)
            : base(options)
        {
        }

        public DbSet<Models.Player> TblUsers { get; set; }
        public DbSet<Models.Game> TblGames { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Game>().HasKey(g => new { g.Username, g.GameId }); 
        }

    }
}