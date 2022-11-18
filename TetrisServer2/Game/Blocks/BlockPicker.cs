using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisServer2.Game.Blocks.BlockEntities;

namespace TetrisServer2.Game.Blocks
{
    public class BlockPicker
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBLock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random rand = new Random();

        public Block NextBlock { get; private set; }

        public BlockPicker()
        {
            NextBlock = GetRandomBlock();
        }

        public Block GetRandomBlock()
        {
            return blocks[rand.Next(blocks.Length)];
        }

        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            do
            {
                NextBlock = GetRandomBlock();
            }
            while (block.BlockId == NextBlock.BlockId);

            block.ResetOffset();
            return block;
        }

    }
}
