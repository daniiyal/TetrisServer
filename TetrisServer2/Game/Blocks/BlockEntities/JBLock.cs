using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class JBLock : Block
    {
        public override int BlockId => 2;
        public override Position StartOffset => new Position(0, 3);
        public override Position[][] BlockTiles => new Position[][]
        {
            new Position[] {new(0, 0, 17), new(1, 0, 18), new(1, 1, 19), new(1, 2, 20)},
            new Position[] {new(0, 1, 21), new(0, 2, 22), new(1, 1, 23), new(2, 1, 24)},
            new Position[] {new(1, 0, 25), new(1, 1, 26), new(1, 2, 27), new(2, 2, 28)},
            new Position[] {new(0, 1, 29), new(1, 1, 30), new(2, 1, 31), new(2, 0, 32)}
        };

    }
}
