using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectFourServer.Models
{
    public class Game
    {
        [Key, Column(Order = 1)]
        public string Username { get; set; }
        [Key, Column(Order = 2)]
        public int GameId { get; set; }
        public string GameDuration { get; set; }
        public string GameResult { get; set; }
        public DateTime GameStartTime { get; set; }

        // represent the relationship between game and player
        [ForeignKey("Username")]
        public Player Player { get; set; }

    }
}
