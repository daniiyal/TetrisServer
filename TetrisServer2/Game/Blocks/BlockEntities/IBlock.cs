namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class IBlock : Block
    {
        public override int BlockId => 1;
        public override Position StartOffset => new Position(-1, 3);
        public override Position[][] BlockTiles => new Position[][]
        {
            new Position[] { new(1,0, 1), new(1,1, 2), new(1,2, 3), new(1,3, 4) },
            new Position[] { new(0,2, 5), new(1,2, 6), new(2,2, 7), new(3,2, 8) },
            new Position[] { new(2,0, 9), new(2,1, 10), new(2,2, 11), new(2,3, 12) },
            new Position[] { new(0,1, 13), new(1,1, 14), new(2,1, 15), new(3,1, 16) }
        };
    }
}
