using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourClient
{
    public class Game
    {
        public string Username { get; set; }
        public int GameId { get; set; }
        public string GameDuration { get; set; }
        public string GameResult { get; set; }
        public DateTime GameStartTime { get; set; }
    }
}
