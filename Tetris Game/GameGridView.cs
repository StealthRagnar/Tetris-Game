using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Tetris_Game
{
    public class GameGridView
    {
        private readonly int[,] grid;
        public int rows {  get; private set; }
        public int columns { get; private set; }
        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;

        }
        public GameGridView(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInside(int r, int c)
        {
            return r >= 0 && r < rows && c >= 0 && c < columns;
        }

        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c] == 0;
        }
       

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < columns; c++) {
                if (grid[r, c] == 0)
                    return false;
            }
        return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < columns; c++)
            {
                if (grid[r, c] != 0)
                    return false;
            }
            return true;
        }

        private void ClearRow(int r)
        {
            for(int c = 0; c < columns; c++)
            {
                grid[r,c] = 0;
            }
        }
        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;
            for (int r = rows-1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;

                }
                else if(cleared>0)
                {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared;
        }


    }
}
