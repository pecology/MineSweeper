﻿using System;
using System.Collections.Generic;

namespace MineSweeper
{
    public class MineSweeper
    {
        public MineSweeper(int height, int width, IEnumerable<(int row, int col)> mineMaps)
        {

        }

        public MineSweeper(int height, int width, int numberOfMines)
        {

        }

        public GameState GameState
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DigResult Dig(int row, int col)
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
