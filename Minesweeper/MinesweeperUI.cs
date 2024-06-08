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

namespace Minesweeper
{
    public partial class MinesweeperUI : Form
    {
        public Label TimerLabel => _timerLabel;
        public System.Windows.Forms.Timer GameTimer => _gameTimer;
        public int NumberOfFlags { get; set; }
        public int ElapsedTime { get; set; }

        private Gameplay _gameplay;

        //Dificulty buttons
        private Button _easyButton;
        private Button _mediumButton;
        private Button _hardButton;

        //Board buttons
        private int _buttonWidth = 50;
        private int _buttonHeight = 50;
        private int _padding = 0;

        //Labels
        private Label _flagNumberLabel;
        private Label _timerLabel;

        //Timer
        private System.Windows.Forms.Timer _gameTimer;


        public MinesweeperUI()
        {
            _gameplay = new Gameplay(this);

            InitializeUI();
        }

        public void InitializeUI()
        {
            InitializeComponent();
            CreateDifficultyButtons();
        }

        public void CreateButtonArray(Button[,] buttonArray)
        {
            int startY = _flagNumberLabel.Height + 20;

            for (int i = 0; i < _gameplay.Rows; i++)
            {
                for (int j = 0; j < _gameplay.Cols; j++)
                {
                    Button button = new Button
                    {
                        Width = _buttonWidth,
                        Height = _buttonHeight,
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

            //Start timer
            if (!_gameplay.IsTimerStarted)
            {
                _gameTimer.Start();
                _gameplay.IsTimerStarted = true;
            }

            var (i, j) = ((int, int))clickedButton.Tag;

            if (e.Button == MouseButtons.Left)
            {
                if (clickedButton.Text == "F")
                {
                    return;
                }

                if (_gameplay.Board[i, j] == -1)
                {
                    clickedButton.Text = "BOMB";
                    _gameplay.EndGame("Boom! You hit a bomb!");
                }
                else
                {
                    _gameplay.RevealCell(i, j);
                    _gameplay.CheckForWin();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (clickedButton.Text == "F")
                {
                    clickedButton.Text = "";
                    NumberOfFlags++;
                }
                else
                {
                    clickedButton.Text = "F";
                    NumberOfFlags--;
                }

                UpdateFlagNumberLabel();
            }
        }

        public void CreateDifficultyButtons()
        {
            _easyButton = new Button
            {
                Text = "Easy",
                Location = new System.Drawing.Point(50, 50),
                AutoSize = true
            };
            _easyButton.Click += EasyButton_Click;

            _mediumButton = new Button
            {
                Text = "Medium",
                Location = new System.Drawing.Point(150, 50),
                AutoSize = true
            };
            _mediumButton.Click += MediumButton_Click;

            _hardButton = new Button
            {
                Text = "Hard",
                Location = new System.Drawing.Point(250, 50),
                AutoSize = true
            };
            _hardButton.Click += HardButton_Click;

            this.Controls.Add(_easyButton);
            this.Controls.Add(_mediumButton);
            this.Controls.Add(_hardButton);
        }

        private void EasyButton_Click(object sender, EventArgs e)
        {
            _gameplay.InitializeGame(8, 10, 10);
            this.ClientSize = new Size(500, 450);
            SetButtonSize(50, 50, 0);
            HideDifficultyButtons();
        }

        private void MediumButton_Click(object sender, EventArgs e)
        {
            _gameplay.InitializeGame(14, 18, 40);
            this.ClientSize = new Size(900, 750);
            SetButtonSize(50, 50, 0);
            HideDifficultyButtons();
        }

        private void HardButton_Click(object sender, EventArgs e)
        {
            _gameplay.InitializeGame(20, 24, 99);
            this.ClientSize = new Size(975, 850);
            SetButtonSize(40, 40, 0);
            HideDifficultyButtons();
        }

        private void HideDifficultyButtons()
        {
            _easyButton.Visible = false;
            _mediumButton.Visible = false;
            _hardButton.Visible = false;
        }

        private void SetButtonSize(int width, int height, int padding)
        {
            _buttonWidth = width;
            _buttonHeight = height;
            _padding = padding;

            CreateButtonArray(_gameplay.ButtonArray);
        }

        public void InitializeTimer()
        {
            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 1000; // 1 second interval
            _gameTimer.Tick += GameTimer_Tick;
            ElapsedTime = 0;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            ElapsedTime++;
            _timerLabel.Text = $"Time: {ElapsedTime} sec";
        }

        public void CreateFlagNumberLabel()
        {
            _flagNumberLabel = new Label
            {
                Text = NumberOfFlags.ToString(),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 16),
                Location = new System.Drawing.Point(300, 10)
            };

            // Add the label to the form
            this.Controls.Add(_flagNumberLabel);
        }

        public void CreateTimerLabel()
        {
            _timerLabel = new Label
            {
                Text = "Time: 0 sec",
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 16),
                Location = new System.Drawing.Point(100, 10)
            };

            // Add the label to the form
            this.Controls.Add(_timerLabel);
        }

        private void UpdateFlagNumberLabel()
        {
            _flagNumberLabel.Text = NumberOfFlags.ToString();
        }
    }
}
