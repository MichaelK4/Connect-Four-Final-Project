using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourClient
{
    [Serializable]
    public class Token
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Player { get; set; }
        public int Step { get; set; }

        public Token(int row, int column, int player, int step)
        {
            Row = row;
            Column = column;
            Player = player;
            Step = step;
        }

    }
}
