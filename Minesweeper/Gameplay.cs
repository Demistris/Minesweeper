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
using static System.Windows.Forms.AxHost;

namespace Minesweeper
{  
    public partial class Gameplay : Form, IGameplay
    {
        private const int _buttonWidth = 50;
        private const int _buttonHeight = 50;
        private const int _padding = 0;

        private int[,] _board;
        private int _rows;
        private int _cols;
        private Button[,] _buttonArray;
        private int _revealedCells;
        private int _numberOfBombs;
        private int _numberOfFlags;

        //Positions of the eight possible neighbors
        private int[] _dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        private int[] _dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        //private Label flagNumberLabel;

        public Gameplay()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            InitializeBoard(8, 10);
            PlaceBombs(_board, 10);
            CalculateAdjacentBombs(_board);
            CreateButtonArray(_board, _buttonArray);
            CreateFlagNumberLabel();
            //DisplayBoard(_board, _buttonArray);
            _revealedCells = 0;
        }

        public void InitializeBoard(int x, int y)
        {
            _rows = x;
            _cols = y;

            _board = new int[_rows, _cols];
            _buttonArray = new Button[_rows, _cols];
        }

        private void CreateFlagNumberLabel()
        {
            _flagNumberLabel = new Label
            {
                Text = _numberOfFlags.ToString(),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 16),
                Location = new System.Drawing.Point(225, 10)
            };

            // Add the label to the form
            this.Controls.Add(_flagNumberLabel);
        }

        public void CreateButtonArray(int[,] board, Button[,] buttonArray)
        {
            int startY = _flagNumberLabel.Height + 20;

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    Button button = new Button
                    {
                        Width = _buttonWidth,
                        Height = _buttonHeight,
                        //Text = $"{board[i, j]}",
                        Tag = (i, j),
                        Location = new System.Drawing.Point(j * (_buttonWidth + _padding), i * (_buttonHeight + _padding) + startY)
                    };

                    button.MouseDown += new MouseEventHandler(Button_MouseDown);
                    this.Controls.Add(button);
                    buttonArray[i, j] = button;
                }
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button clickedButton = sender as Button;
            var (i, j) = ((int, int))clickedButton.Tag;

            if(e.Button == MouseButtons.Left)
            {
                if (clickedButton.Text == "F")
                {
                    return;
                }

                if (_board[i, j] == -1)
                {
                    clickedButton.Text = "BOMB";
                    MessageBox.Show("Boom! You hit a bomb!");
                    ResetGame(_buttonArray);
                }
                else
                {
                    RevealCell(i, j);
                    CheckForWin();
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                if(clickedButton.Text == "F")
                {
                    clickedButton.Text = "";
                    _numberOfFlags++;
                }
                else
                {
                    clickedButton.Text = "F";
                    _numberOfFlags--;
                }

                UpdateFlagNumberLabel();
            }
        }

        private void UpdateFlagNumberLabel()
        {
            _flagNumberLabel.Text = _numberOfFlags.ToString();
        }

        public void RevealCell(int i, int j)
        {
            if (i < 0 || i >= _rows || j < 0 || j >= _cols || !_buttonArray[i, j].Enabled)
            {
                return;
            }

            _buttonArray[i, j].Text = _board[i, j] == 0 ? "" : _board[i, j].ToString();
            _buttonArray[i, j].Enabled = false;
            _revealedCells++;

            if (_board[i, j] == 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    int ni = i + _dx[k];
                    int nj = j + _dy[k];
                    RevealCell(ni, nj);
                }
            }
        }

        public void CheckForWin()
        {
            int totalNonBombCells = _rows * _cols - _numberOfBombs;

            if (_revealedCells == totalNonBombCells)
            {
                MessageBox.Show("Congratulations! You've won!");
                ResetGame(_buttonArray);
            }
        }

        public void PlaceBombs(int[,] board, int bombQuantity)
        {
            _numberOfBombs = bombQuantity;
            _numberOfFlags = _numberOfBombs;

            Random random = new Random();

            while(bombQuantity > 0)
            {
                int randomX = random.Next(0, _rows - 1);
                int randomY = random.Next(0, _cols - 1);

                if(board[randomX, randomY] != -1)
                {
                    board[randomX, randomY] = -1;
                    bombQuantity -= 1;
                }
            }
        }

        public void DisplayBoard(int[,] board, Button[,] buttonArray)
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (board[i, j] == -1)
                    {
                        buttonArray[i, j].Text = "B";
                    }
                    else if (board[i, j] > 0)
                    {
                        buttonArray[i, j].Text = board[i, j].ToString();
                    }
                }
            }
        }

        public void CalculateAdjacentBombs(int[,] board)
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (board[i, j] == -1)
                    {
                        continue;
                    }

                    int adjacentBombs = 0;

                    for (int k = 0; k < _dx.Length; k++) 
                    {
                        //Checking all 8 neighbors of current cell
                        int ni = i + _dx[k];
                        int nj = j + _dy[k];

                        //If one of neighbors is bomb then increase the value of the "adjacentBombs"
                        if (ni >= 0 && ni < _rows && nj >= 0 && nj < _cols && board[ni, nj] == -1)
                        {
                            adjacentBombs++;
                        }
                    }

                    board[i, j] = adjacentBombs;
                }
            }
        }

        public void ResetGame(Button[,] buttonArray)
        {
            // Remove all existing buttons
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (buttonArray[i, j] != null)
                    {
                        this.Controls.Remove(buttonArray[i, j]);
                        buttonArray[i, j].Dispose();
                    }
                }
            }

            // Reinitialize the game
            InitializeGame();
        }
    }
}
