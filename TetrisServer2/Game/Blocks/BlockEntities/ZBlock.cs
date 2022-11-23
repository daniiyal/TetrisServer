namespace TetrisServer2.Game.Blocks.BlockEntities
{
    internal class ZBlock : Block
    {
        public override int BlockId => 7;

        public override Position StartOffset => new(0, 3);

        public override Position[][] BlockTiles => new Position[][] {
            new Position[] {new(0,0, 97), new(0,1, 98), new(1,1, 99), new(1,2, 100)},
            new Position[] {new(0,2, 101), new(1,1, 102), new(1,2, 103), new(2,1, 104)},
            new Position[] {new(1,0, 105), new(1,1, 106), new(2,1, 107), new(2,2, 108)},
            new Position[] {new(0,1, 109), new(1,0, 110), new(1,1, 111), new(2,0, 112)}
        };
    }
}
