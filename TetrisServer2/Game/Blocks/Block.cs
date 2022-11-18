namespace TetrisServer2.Game.Blocks
{
    public abstract class Block
    {
        public abstract Position[][] BlockTiles { get; }
        public abstract Position StartOffset { get; }

        public abstract int BlockId { get; }

        private int rotationState;
        private Position offset;

        public Block()
        {
           ResetOffset();
        }

        public void ResetOffset()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public void Move(int rows, int cols)
        {
            offset.Row += rows;
            offset.Column += cols;
        }

        public IEnumerable<Position> BlockTilesPositions()
        {
            foreach (var position in BlockTiles[rotationState])
            {
                yield return new Position(position.Row + offset.Row, position.Column + offset.Column);
            }
        }

    }
}
