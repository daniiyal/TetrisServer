using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class OBlock : Block
    {
        public override int BlockId => 4;
        public override Position StartOffset => new Position(0, 3);
        public override Position[][] BlockTiles => new Position[][]
        {
            new Position[] { new(0,0, 49), new(0,1, 50), new(1,0, 51), new(1,1, 52) },
            new Position[] { new(0,0, 53), new(0,1, 54), new(1,0, 55), new(1,1, 56) },
            new Position[] { new(0,0, 57), new(0,1, 58), new(1,0, 59), new(1,1, 60) },
            new Position[] { new(0,0, 61), new(0,1, 62), new(1,0, 63), new(1,1, 64) }
        };
    }
}
