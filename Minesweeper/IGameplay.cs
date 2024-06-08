using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public interface IGameplay
    {
        void InitializeBoard(int x, int y);
        void CreateButtonArray(int[,] board, Button[,] buttonArray);
        void RevealCell(int i, int j);
        void PlaceBombs(int[,] board, int bombQuantity);
        void DisplayBoard(int[,] board, Button[,] buttonArray);
        void CalculateAdjacentBombs(int[,] board);
        void ResetGame(Button[,] buttonArray);
        void CheckForWin();
    }
}
