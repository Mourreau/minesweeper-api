
namespace Minesweeper.Core.Models
{
    public class Cell
    {


        public bool IsMine { get; internal set; } = false; // Клетка является Миной
        public bool IsFlagged { get; internal set; } = false; // Поставлен ли на клетку Флаг
        public bool IsRevealed { get; internal set; }  = false; // Открыта ли клетка
        public int AdjacentMinesCount { get; internal set; } = 0; // Количество Мин вокруг клетки. 


        /// <summary>
        /// Координата X клетки на игровом поле
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Координата Y клетки на игровом поле
        /// </summary>
        public int Y { get; private set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
