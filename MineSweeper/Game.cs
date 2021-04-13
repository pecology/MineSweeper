using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeper
{
    public class Game
    {
        private readonly Cell[,] _Cells;
        private int _NumOfNotDiggedCells;

        public Game(int height, int width, params (int row, int col)[] mineMaps)
        {
            if (mineMaps == null)
            {
                mineMaps = new (int, int)[0];
            }

            _Cells = new Cell[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _Cells[i, j] = new Cell(false);
                }
            }

            foreach (var (row, col) in mineMaps)
            {
                _Cells[row, col] = new Cell(true);
            }

            State = GameState.Playing;
            _NumOfNotDiggedCells = height * width - mineMaps.Count();
        }

        public Game(int height, int width, int numberOfMines) : this(height, width, RandomCoordinates(height, width, numberOfMines))
        {
        }

        private static (int, int)[] RandomCoordinates(int height, int width, int numberOfMines)
        {
            var random = new Random();
            var returnArray = new (int, int)[numberOfMines];
            for (int i = 0; i < numberOfMines; i++)
            {
                var row = random.Next(0, height - 1);
                var col = random.Next(0, width - 1);
                returnArray[i] = (row, col);
            }
            return returnArray;
        }

        public GameState State { get; private set; }

        public IList<(int row, int col)> Dig(int row, int col)
        {
            if (State == GameState.GameClear ||
                State == GameState.GameOver)
            {
                return new List<(int, int)>();
            }

            if (_Cells[row, col].State == CellState.Digged ||
                _Cells[row, col].State == CellState.Flagged ||
                _Cells[row, col].State == CellState.Question)
            {
                return new List<(int, int)>();
            }


            if (_Cells[row, col].ExistsMine)
            {
                State = GameState.GameOver;
                return new List<(int, int)>();
            }

            _Cells[row, col].State = CellState.Digged;
            _NumOfNotDiggedCells--;

            if (_NumOfNotDiggedCells == 0)
            {
                State = GameState.GameClear;
                return new List<(int, int)>();
            }

            if (CountNeighborMines(row, col) == 0)
            {
                return GetNeighborCoordinates(row, col)
                    .Select(neighbor => Dig(neighbor.row, neighbor.col))
                    .Aggregate((curr, next) => curr.Concat(next).ToList())
                    .Append((row, col))
                    .ToList();
            }
            else
            {
                return new List<(int, int)>() { (row, col) };
            }
        }

        public IList<(int row, int col)> DigAround(int row, int col)
        {
            return GetNeighborCoordinates(row, col)
                .Select(coordinate => Dig(coordinate.row, coordinate.col))
                .Aggregate((curr, next) => curr.Concat(next).ToList());
        }

        public CellState GetCellState(int row, int col)
        {
            return _Cells[row, col].State;
        }

        public void Flag(int row, int col)
        {
            if (State == GameState.GameClear ||
                State == GameState.GameOver)
            {
                return;
            }

            switch (_Cells[row, col].State)
            {
                case CellState.Initial:
                    _Cells[row, col].State = CellState.Flagged;
                    break;
                case CellState.Flagged:
                    _Cells[row, col].State = CellState.Question;
                    break;
                case CellState.Question:
                    _Cells[row, col].State = CellState.Initial;
                    break;
                case CellState.Digged:
                    break;
                default:
                    break;
            }
        }

        public int CountNeighborMines(int row, int col)
        {
            // 範囲外の場合に例外を発生させる
            _ = _Cells[row, col];

            var count = 0;
            foreach (var (neighborRow, neighborCol) in GetNeighborCoordinates(row, col))
            {
                if (_Cells[neighborRow, neighborCol].ExistsMine)
                {
                    count++;
                }
            }

            return count;
        }

        public IEnumerable<(int row, int col)> GetNeighborCoordinates(int row, int col)
        {
            var rowMin = Math.Max(0, row - 1);
            var rowMax = Math.Min(_Cells.GetLength(0) - 1, row + 1);

            var colMin = Math.Max(0, col - 1);
            var colMax = Math.Min(_Cells.GetLength(1) - 1, col + 1);

            for (int i = rowMin; i <= rowMax; i++)
            {
                for (int j = colMin; j <= colMax; j++)
                {
                    if (i == row && j == col)
                    {
                        continue;
                    }

                    yield return (i, j);
                }
            }
        }
    }
}
