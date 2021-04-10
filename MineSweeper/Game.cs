using System;
using System.Collections.Generic;

namespace MineSweeper
{
    public class Game
    {
        private readonly CellState[,] _Cells;

        public Game(int height, int width, IEnumerable<(int row, int col)> mineMaps)
        {
            _Cells = new CellState[height, width];
        }

        public Game(int height, int width, int numberOfMines)
        {
            _Cells = new CellState[height, width];
        }

        public GameState GameState
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dig(int row, int col)
        {
            _ = _Cells[row, col];
        }

        public CellState GetCellState(int row, int col)
        {
            return CellState.Initial;
        }

        public void Flag(int row, int col)
        {
            throw new NotImplementedException();
        }

        public int CountNeighborMines(int row, int col)
        {
            throw new NotImplementedException();
        }
    }
}
