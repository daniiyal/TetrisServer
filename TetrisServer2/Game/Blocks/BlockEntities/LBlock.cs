﻿namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class LBlock : Block
    {
        public override int BlockId => 3;
        public override Position StartOffset => new Position(0, 3);
        public override Position[][] BlockTiles => new Position[][]
        {
            new Position[] {new(0,2), new(1,0), new(1,1), new(1,2)},
            new Position[] {new(0,1), new(1,1), new(2,1), new(2,2)},
            new Position[] {new(1,0), new(1,1), new(1,2), new(2,0)},
            new Position[] {new(0,0), new(0,1), new(1,1), new(2,1)}
        };
    }
}
