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
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }
        };
    }
}
