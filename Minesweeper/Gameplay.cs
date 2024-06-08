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
    public partial class Gameplay
    {
        public bool IsTimerStarted { get; set; }
        public int Rows => _rows;
        public int Cols => _cols;
        public int[,] Board => _board;
        public Button[,] ButtonArray => _buttonArray;

        private MinesweeperUI _minesweeperUI;
        private int[,] _board;
        private Button[,] _buttonArray;

        private int _rows;
        private int _cols;
        private int _revealedCells;
        private int _numberOfBombs;

        //Positions of the eight possible neighbors
        private int[] _dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        private int[] _dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        //Timer
        private bool _isTimerStarted;

        public Gameplay(MinesweeperUI minesweeperUI)
        {
            _minesweeperUI = minesweeperUI;
        }

        public void InitializeGame(int rows, int cols, int bombQuantity)
        {
            InitializeBoard(rows, cols);
            _minesweeperUI.CreateButtonArray(_board, _buttonArray);

            PlaceBombs(_board, bombQuantity);
            CalculateAdjacentBombs(_board);
            _minesweeperUI.InitializeTimer();
            _minesweeperUI.CreateFlagNumberLabel();
            _minesweeperUI.CreateTimerLabel();

            _revealedCells = 0;
            _isTimerStarted = false;
        }

        private void InitializeBoard(int x, int y)
        {
            _rows = x;
            _cols = y;

            _board = new int[_rows, _cols];
            _buttonArray = new Button[_rows, _cols];
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
                EndGame("Congratulations! You've won!");
            }
        }

        private void PlaceBombs(int[,] board, int bombQuantity)
        {
            _numberOfBombs = bombQuantity;
            _minesweeperUI.NumberOfFlags = _numberOfBombs;

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

        //public void DisplayBoard(int[,] board, Button[,] buttonArray)
        //{
        //    for (int i = 0; i < _rows; i++)
        //    {
        //        for (int j = 0; j < _cols; j++)
        //        {
        //            if (board[i, j] == -1)
        //            {
        //                buttonArray[i, j].Text = "B";
        //            }
        //            else if (board[i, j] > 0)
        //            {
        //                buttonArray[i, j].Text = board[i, j].ToString();
        //            }
        //        }
        //    }
        //}

        private void CalculateAdjacentBombs(int[,] board)
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

        public void EndGame(string message)
        {
            _minesweeperUI.GameTimer.Stop();
            MessageBox.Show(message);
            ResetGame(_buttonArray);
        }

        private void ResetGame(Button[,] buttonArray)
        {
            // Remove all existing buttons
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (buttonArray[i, j] != null)
                    {
                        _minesweeperUI.Controls.Remove(buttonArray[i, j]);
                        buttonArray[i, j].Dispose();
                    }
                }
            }

            //Reset timer
            _minesweeperUI.ElapsedTime = 0;
            _minesweeperUI.TimerLabel.Text = "Time: 0 sec";
            _isTimerStarted = false;

            // Reinitialize the game
            _minesweeperUI.Controls.Clear();
            _minesweeperUI.InitializeUI();
            _minesweeperUI.CreateDifficultyButtons();
        }
    }
}
