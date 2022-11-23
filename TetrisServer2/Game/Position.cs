using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisServer2.Game
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int BlockPosId { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;

        }
        public Position(int row, int column, int blockPosId)
        {
            Row = row;
            Column = column;
            BlockPosId = blockPosId;
        }
    }
}
