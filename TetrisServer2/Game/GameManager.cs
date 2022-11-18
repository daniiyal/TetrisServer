using System.Runtime.CompilerServices;
using TetrisServer2.Game.Blocks;

namespace TetrisServer2.Game
{
    public class GameManager
    {
        private Block currentBlock;
        public int Score { get; private set; }

        private readonly int maxDelay = 500;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;


        public Block CurrentBlock
        {
            get => currentBlock;
            private set => currentBlock = value;
        }


        public Field Field { get; set; }
        public BlockPicker BlockPicker { get; set; }
        public bool GameOver { get; set; }

        public GameManager(int rows, int cols)
        {
            Field = new Field(rows, cols);
            BlockPicker = new BlockPicker();
            CurrentBlock = BlockPicker.GetRandomBlock();
            StartGame();
        }

        private async Task GameLoop()
        {
            while (!GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (Score * delayDecrease));
                await Task.Delay(delay);
                MoveBlockDown();
            }

            Console.WriteLine("Игра закончена");
        }


        private async Task StartGame()
        {
            await GameLoop();
        }

        public bool IsBlockFit()
        {
            foreach (var position in CurrentBlock.BlockTilesPositions())
            {
                if (!Field.IsEmpty(position.Row, position.Column))
                    return false;
            }

            return true;
        }
        
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!IsBlockFit())
                CurrentBlock.Move(0, 1);
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!IsBlockFit())
                CurrentBlock.Move(0, -1);
        }

        private bool IsGameOver()
        {
            return !(Field.IsRowEmpty(0));
        }

        private void PlaceBlock()
        {
            foreach (var position in CurrentBlock.BlockTilesPositions())
            {
                Field[position.Row, position.Column] = CurrentBlock.BlockId;
            }

            Field.ClearFullRows();

            if (IsGameOver())
                GameOver = true;
            else
                CurrentBlock = BlockPicker.GetAndUpdate();

            Console.WriteLine($"Текущий блок - {CurrentBlock.GetType()}");
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!IsBlockFit())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
    }
}
