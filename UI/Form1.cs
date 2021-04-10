using MineSweeper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        private Label[,] Cells;
        public Form1()
        {
            InitializeComponent();

            const int HEIGHT = 16;
            const int WIDTH = 30;
            const int NUM_OF_MINES = 99;
            const int CELL_SIZE = 20;

            Cells = new Label[HEIGHT, WIDTH];
            var game = new MineSweeper.Game(HEIGHT, WIDTH, NUM_OF_MINES);

            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    var label = new Label();
                    label.Location = new Point(CELL_SIZE * j, CELL_SIZE + CELL_SIZE * i);
                    label.Name = "label" + i + j;
                    label.Size = new Size(CELL_SIZE, CELL_SIZE);
                    label.Text = i.ToString() + j;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    Cells[i, j] = label;



                    label.MouseUp += CreateLeftClickEvent(i, j, game);
                    this.Controls.Add(label);
                }
            }
        }

        private MouseEventHandler CreateLeftClickEvent(int row, int col, MineSweeper.Game game)
        {
            return (object sender, MouseEventArgs e) =>
            {
                game.Dig(row, col);

                if (game.GameState == GameState.GameClear)
                {
                    MessageBox.Show("clear!!");
                }
                else if(game.GameState == GameState.GameOver)
                {
                    MessageBox.Show("bomb!!");
                }
                else
                {
                    Cells[row, col].Text = game.CountNeighborMines(row, col).ToString();
                }
            };
        }
    }
}
