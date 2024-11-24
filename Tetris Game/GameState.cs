namespace Tetris_Game
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for(int i = 0; i<2; i++)
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGridView GameGridView { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public GameState()
        {
            GameGridView = new GameGridView(22, 10);
            BlockQueue = new BlockQueue();
            currentBlock = BlockQueue.GetAndUpdate();
        }

        private bool BlockFits()
        {
            foreach(Position p in CurrentBlock.TilePositions())
            {
                if(!GameGridView.IsEmpty(p.Row, p.Column))
                    return false;
            }

            return true;
        }

        public void RotateBlockCW()
        {
            currentBlock.RotateCW();
            if (!BlockFits())
                CurrentBlock.RotateCCW();
        }

        public void RotateBlockCCW()
        {
            currentBlock.RotateCCW();
            if (!BlockFits())
                CurrentBlock.RotateCW();
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if(!BlockFits())
                CurrentBlock.Move(0, 1);
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!BlockFits())
                CurrentBlock.Move(0, -1);
        }

        private bool IsGameOver()
        {
            return !(GameGridView.IsRowEmpty(0) && GameGridView.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGridView[p.Row, p.Column] = CurrentBlock.Id;
            }
            GameGridView.ClearFullRows();
            if (IsGameOver())
                GameOver = true;
            else
                CurrentBlock = BlockQueue.GetAndUpdate();
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if(!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
    }
}
