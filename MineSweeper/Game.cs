using System;
using System.Collections.Generic;

namespace MineSweeper
{
    public class Game
    {
        public Game(int height, int width, IEnumerable<(int row, int col)> mineMaps)
        {

        }

        public Game(int height, int width, int numberOfMines)
        {

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
            throw new NotImplementedException();
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
