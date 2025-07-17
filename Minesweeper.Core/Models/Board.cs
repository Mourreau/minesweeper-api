using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core.Models
{
    public class Board
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        public Cell[,] GameBoard { get; set; }

        /// <summary>
        /// Генерация пустого игрового поля
        /// </summary>
        /// <param name="boardWidth">Ширина поля (Х)</param>
        /// <param name="boardHeight">Высота поля (Y)</param>
        /// <returns>Возвращает пустое игровое поле состоящее из двумерного массива Cell</returns>
        public Cell[,] EmptyBoardGenerator(int boardWidth, int boardHeight)
        {
            return new Cell[boardWidth, boardHeight];
        }
    }
}
