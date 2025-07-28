
namespace Minesweeper.Core.Models
{
    public class Cell
    {


        public bool IsMine { get; set; } = false; // Клетка является Миной
        public bool IsFlagged { get; set; } = false; // Поставлен ли на клетку Флаг
        public bool IsRevealed { get; set; }  = false; // Открыта ли клетка
        public int AdjacentMinesCount { get; set; } = 0; // Количетво Мин вокруг клетки. 


        /// <summary>
        /// Координата X клетки на игровом поле
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координата Y клетки на игровом поле
        /// </summary>
        public int Y { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
