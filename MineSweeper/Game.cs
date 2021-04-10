using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeper
{
    public class Game
    {
        private readonly Cell[,] _Cells;
        private int _NumOfNotDiggedCells;

        public Game(int height, int width, IEnumerable<(int row, int col)> mineMaps)
        {
            _Cells = new Cell[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _Cells[i, j] = new Cell(false);
                }
            }

            foreach(var (row, col) in mineMaps)
            {
                _Cells[row, col] = new Cell(true);
            }

            GameState = GameState.Playing;
            _NumOfNotDiggedCells = height * width - mineMaps.Count();
        }

        public Game(int height, int width, int numberOfMines) : this(height, width, new List<(int, int)>())
        {
        }

        public GameState GameState { get; private set; }

        public void Dig(int row, int col)
        {
            _Cells[row, col].State = CellState.Digged;

            if (_Cells[row, col].ExistsMine)
            {
                GameState = GameState.GameOver;
            }
            else
            {
                _NumOfNotDiggedCells--;
                if(_NumOfNotDiggedCells <= 0)
                {
                    GameState = GameState.GameClear;
                }
            }
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
