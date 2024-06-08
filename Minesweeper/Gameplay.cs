using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{  
    //PPM robić flagę

    public partial class Gameplay : Form, IGameplay
    {
        private const int _buttonWidth = 50;
        private const int _buttonHeight = 50;
        private const int _padding = 0;

        public Gameplay()
        {
            int[,] board = CreateBoard(8, 10);
            SetBombs(board, 10);
            SetNumbers(board);

            InitializeComponent();

            CreateButtonArray(board);
            //ShowBoard(board);
        }

        public int[,] CreateBoard(int x, int y)
        {
            int[,] board = new int[x, y];

            return board;
        }

        public void CreateButtonArray(int[,] board)
        {
            var buttonArray = new Button[board.GetLength(0), board.GetLength(1)];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Button button = new Button
                    {
                        Width = _buttonWidth,
                        Height = _buttonHeight,
                        Text = $"{board[i, j]}",
                        Tag = board[i, j],
                        Location = new System.Drawing.Point(j * (_buttonWidth + _padding), i * (_buttonHeight + _padding))
                    };

                    button.Click += new EventHandler(Button_Click);
                    this.Controls.Add(button);
                    buttonArray[i, j] = button;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            var board = clickedButton.Tag;
            clickedButton.Text = $"{board}";

            if((int)board == 0)
            {
                //Jak kliknę na 0 to mają się wszystkie 0 sąsiadujące "odsłonić"
            }
            else if((int)board == -1)
            {
                MessageBox.Show("You clicked on bomb! :(");
            }

            //Po kliknięciu LPM ma już nie być buttona w tym miejscu tylko sama liczba jeśli jest
        }

        public void SetBombs(int[,] board, int bombQuantity)
        {
            Random random = new Random();

            while(bombQuantity > 0)
            {
                int randomX = random.Next(0, board.GetLength(0) - 1);
                int randomY = random.Next(0, board.GetLength(1) - 1);

                if(board[randomX, randomY] != -1)
                {
                    board[randomX, randomY] = -1;
                    bombQuantity -= 1;
                }
            }
        }

        private void ShowBoard(int[,] board)
        {
            dataGridView.ColumnCount = board.GetLength(1);
            dataGridView.RowCount = board.GetLength(0);

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    dataGridView.Rows[i].Cells[j].Value = board[i, j];
                }
            }
        }

        public void SetNumbers(int[,] board)
        {
            //Positions of the eight possible neighbors around any given cell in the grid
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1};
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1};

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == -1)
                    {
                        continue;
                    }

                    int adjacentBombs = 0;

                    for (int k = 0; k < dx.Length; k++) 
                    {
                        //Checking all 8 neighbors of current cell
                        int ni = i + dx[k];
                        int nj = j + dy[k];

                        //If one of neighbors is bomb then increase the value of the "adjacentBombs"
                        if (ni >= 0 && ni < board.GetLength(0) && nj >= 0 && nj < board.GetLength(1) && board[ni, nj] == -1)
                        {
                            adjacentBombs++;
                        }
                    }

                    board[i, j] = adjacentBombs;
                }
            }
        }
    }
}
