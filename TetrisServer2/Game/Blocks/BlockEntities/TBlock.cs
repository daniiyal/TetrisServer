using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class TBlock : Block
    {
        public override int BlockId => 6;

        public override Position StartOffset => new(0, 3);

        public override Position[][] BlockTiles => new Position[][] {
            new Position[] {new(0,1), new(1,0), new(1,1), new(1,2)},
            new Position[] {new(0,1), new(1,1), new(1,2), new(2,1)},
            new Position[] {new(1,0), new(1,1), new(1,2), new(2,1)},
            new Position[] {new(0,1), new(1,0), new(1,1), new(2,1)}
        };
    }
}
