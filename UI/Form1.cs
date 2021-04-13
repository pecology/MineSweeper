using MineSweeper;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        private readonly Label[,] _Cells;

        private Game _Game;

        private bool _IsMouseDownRightAndLeft = false;

        private readonly int NUM_OF_ROW_CELLS = 16;
        private readonly int NUM_OF_COL_CELLS = 30;
        private readonly int NUM_OF_MINES = 99;
        private readonly int CELL_SIZE = 30;

        public Form1()
        {
            InitializeComponent();

            Width = NUM_OF_COL_CELLS * CELL_SIZE + 16;
            Height = NUM_OF_ROW_CELLS * CELL_SIZE + 39;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            _Cells = new Label[NUM_OF_ROW_CELLS, NUM_OF_COL_CELLS];
            _Game = new Game(NUM_OF_ROW_CELLS, NUM_OF_COL_CELLS, NUM_OF_MINES);

            var onMouseMove = CreateOnMouseMove();
            var onMouseUp = CreateMouseUpEvent();
            for (int i = 0; i < NUM_OF_ROW_CELLS; i++)
            {
                for (int j = 0; j < NUM_OF_COL_CELLS; j++)
                {
                    var label = new Label
                    {
                        Location = new Point(CELL_SIZE * j, CELL_SIZE * i),
                        Name = "label" + i + j,
                        Size = new Size(CELL_SIZE, CELL_SIZE),
                        Text = "",
                        BorderStyle = BorderStyle.Fixed3D
                    };
                    label.Font = new Font(label.Font.FontFamily, 12, FontStyle.Bold);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    _Cells[i, j] = label;

                    label.MouseDown += (object sender, MouseEventArgs e) =>
                    {
                        _IsMouseDownRightAndLeft = MouseButtons.HasFlag(MouseButtons.Right | MouseButtons.Left);

                        var p = PointToClient(Cursor.Position);
                        var row = p.Y / CELL_SIZE;
                        var col = p.X / CELL_SIZE;

                        if (_IsMouseDownRightAndLeft)
                        {
                            UpdateCell(row, col);
                            var diggableCoordinates =
                                _Game.GetNeighborCoordinates(row, col)
                                     .Where(c => _Game.GetCellState(c.row, c.col) == CellState.Initial);
                            foreach (var (aroundRow, aroundCol) in diggableCoordinates)
                            {
                                _Cells[aroundRow, aroundCol].BackColor = Color.LightGray;
                            }
                        }
                        else
                        {
                            _Cells[row, col].BackColor = Color.LightGray;
                        }

                    };

                    label.MouseMove += onMouseMove;
                    label.MouseUp += onMouseUp;
                    Controls.Add(label);
                }
            }
        }

        private MouseEventHandler CreateOnMouseMove()
        {
            var row = 0;
            var col = 0;
            return (object sender, MouseEventArgs e) =>
            {
                if (!MouseButtons.HasFlag(MouseButtons.Left) &&
                    !MouseButtons.HasFlag(MouseButtons.Right))
                {
                    UpdateCell(row, col);
                    return;
                }

                var p = PointToClient(Cursor.Position);

                var currentRow = p.Y / CELL_SIZE;
                var currentCol = p.X / CELL_SIZE;

                if (currentRow < 0 || currentRow >= NUM_OF_ROW_CELLS ||
                    currentCol < 0 || currentCol >= NUM_OF_COL_CELLS)
                {
                    UpdateCell(row, col);
                    return;
                }

                if (row != currentRow ||
                    col != currentCol)
                {
                    if (_IsMouseDownRightAndLeft)
                    {
                        UpdateCell(row, col);
                        var diggableCoordinates =
                            _Game.GetNeighborCoordinates(row, col)
                                 .Where(c => _Game.GetCellState(c.row, c.col) == CellState.Initial);
                        foreach (var (aroundRow, aroundCol) in diggableCoordinates)
                        {
                            UpdateCell(aroundRow, aroundCol);
                        }

                        row = currentRow;
                        col = currentCol;
                        diggableCoordinates =
                            _Game.GetNeighborCoordinates(row, col)
                                 .Where(c => _Game.GetCellState(c.row, c.col) == CellState.Initial);
                        foreach (var (aroundRow, aroundCol) in diggableCoordinates)
                        {
                            _Cells[aroundRow, aroundCol].BackColor = Color.LightGray;
                        }
                    }
                    else
                    {
                        UpdateCell(row, col);

                        row = currentRow;
                        col = currentCol;
                        _Cells[row, col].BackColor = Color.LightGray;
                    }

                }
            };
        }

        private MouseEventHandler CreateMouseUpEvent()
        {
            return (object sender, MouseEventArgs e) =>
            {
                var p = PointToClient(Cursor.Position);

                var row = p.Y / CELL_SIZE;
                var col = p.X / CELL_SIZE;

                if (row < 0 || row >= NUM_OF_ROW_CELLS ||
                    col < 0 || col >= NUM_OF_COL_CELLS)
                {
                    _IsMouseDownRightAndLeft = false;
                    return;
                }

                if (_IsMouseDownRightAndLeft)
                {
                    foreach (var (aroundRow, aroundCol) in _Game.GetNeighborCoordinates(row, col))
                    {
                        UpdateCell(row, col);
                    }

                    var diggedCoordinates = _Game.DigAround(row, col);
                    UpdateCells(diggedCoordinates);

                    _IsMouseDownRightAndLeft = false;
                }
                else if (e.Button.HasFlag(MouseButtons.Left))
                {
                    var diggedCoordinates = _Game.Dig(row, col);
                    UpdateCells(diggedCoordinates);
                }
                else if (e.Button.HasFlag(MouseButtons.Right))
                {
                    _Game.Flag(row, col);
                    UpdateCell(row, col);
                }

                if (_Game.State == GameState.GameClear)
                {
                    MessageBox.Show("clear!!");
                }
                else if (_Game.State == GameState.GameOver)
                {
                    var retry = MessageBox.Show("Bomb!!\n Do you want to do it again?", "GameOver", MessageBoxButtons.YesNo);
                    if (retry == DialogResult.Yes)
                    {
                        Restart();
                    }
                    else if (retry == DialogResult.No)
                    {
                        Close();
                    }
                }
            };
        }

        private void UpdateCells(IEnumerable<(int row, int col)> coordinates)
        {
            foreach (var (row, col) in coordinates)
            {
                UpdateCell(row, col);
            }
        }

        private void UpdateCell(int row, int col)
        {
            switch (_Game.GetCellState(row, col))
            {
                case CellState.Initial:
                    _Cells[row, col].Text = "";
                    _Cells[row, col].BackColor = Color.Transparent;
                    break;
                case CellState.Flagged:
                    _Cells[row, col].Text = "🏁";
                    _Cells[row, col].BackColor = Color.Transparent;
                    break;
                case CellState.Question:
                    _Cells[row, col].Text = "？";
                    _Cells[row, col].BackColor = Color.Transparent;
                    break;
                case CellState.Digged:
                    var numOfMines = _Game.CountNeighborMines(row, col);
                    if (numOfMines == 0)
                    {
                        _Cells[row, col].Text = "";
                    }
                    else
                    {
                        _Cells[row, col].Text = _Game.CountNeighborMines(row, col).ToString();
                    }
                    _Cells[row, col].BackColor = Color.LightGray;
                    break;
                default:
                    break;
            }
        }

        private void Restart()
        {
            _Game = new Game(NUM_OF_ROW_CELLS, NUM_OF_COL_CELLS, NUM_OF_MINES);

            for (int i = 0; i < NUM_OF_ROW_CELLS; i++)
            {
                for (int j = 0; j < NUM_OF_COL_CELLS; j++)
                {
                    _Cells[i, j].Text = "";
                    _Cells[i, j].BackColor = Color.Transparent;
                }
            }
        }
    }
}
