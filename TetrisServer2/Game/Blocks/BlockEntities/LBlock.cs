namespace TetrisServer2.Game.Blocks.BlockEntities
{
    public class LBlock : Block
    {
        public override int BlockId => 3;
        public override Position StartOffset => new Position(0, 3);
        public override Position[][] BlockTiles => new Position[][]
        {
            new Position[] {new(0,2, 33), new(1,0, 34), new(1,1, 35), new(1,2, 36)},
            new Position[] {new(0,1, 37), new(1,1, 38), new(2,1, 39), new(2,2, 40)},
            new Position[] {new(1,0, 41), new(1,1, 42), new(1,2, 43), new(2,0, 44)},
            new Position[] {new(0,0, 45), new(0,1, 46), new(1,1, 47), new(2,1, 48)}
        };
    }
}
