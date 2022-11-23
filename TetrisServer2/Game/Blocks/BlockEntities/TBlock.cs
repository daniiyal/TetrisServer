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
            new Position[] {new(0,1, 81), new(1,0, 82), new(1,1, 83), new(1,2, 84)},
            new Position[] {new(0,1, 85), new(1,1, 86), new(1,2, 87), new(2,1, 88)},
            new Position[] {new(1,0, 89), new(1,1, 90), new(1,2, 91), new(2,1, 92)},
            new Position[] {new(0,1, 93), new(1,0, 94), new(1,1, 95), new(2,1, 96)}
        };
    }
}
