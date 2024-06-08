using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public interface IGameplay
    {
        int[,] CreateBoard(int x, int y);
        void CreateButtonArray(int[,] board);
        void SetBombs(int[,] board, int bombQuantity);
        void SetNumbers(int[,] board);
    }
}
