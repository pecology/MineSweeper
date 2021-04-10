using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper
{
    class Cell
    {
        public CellState State { get; set; }
        public bool ExistsMine { get; }

        public Cell(bool existsMine)
        {
            ExistsMine = existsMine;
            State = CellState.Initial;
        }
    }
}
