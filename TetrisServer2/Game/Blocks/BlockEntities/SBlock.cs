using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class SBlock : Block
    {
        public override int BlockId => 5;

        public override Position StartOffset => new(0, 3);

        public override Position[][] BlockTiles => new Position[][] {
            new Position[] { new(0,1, 65), new(0,2, 66), new(1,0, 67), new(1,1, 68) },
            new Position[] { new(0,1, 69), new(1,1, 70), new(1,2, 71), new(2,2, 72) },
            new Position[] { new(1,1, 73), new(1,2, 74), new(2,0, 75), new(2,1, 76) },
            new Position[] { new(0,0, 77), new(1,0, 78), new(1,1, 79), new(2,1, 80) }
        };
    }
}
