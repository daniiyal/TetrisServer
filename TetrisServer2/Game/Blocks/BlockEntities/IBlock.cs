namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class IBlock : Block
    {
        public override int BlockId => 1;
        public override Position StartOffset => new Position(-1, 3);
        public override Position[][] BlockTiles => new Position[][]
        {
            new Position[] { new(1,0), new(1,1), new(1,2), new(1,3) },
            new Position[] { new(0,2), new(1,2), new(2,2), new(3,2) },
            new Position[] { new(2,0), new(2,1), new(2,2), new(2,3) },
            new Position[] { new(0,1), new(1,1), new(2,1), new(3,1) }
        };
    }
}
