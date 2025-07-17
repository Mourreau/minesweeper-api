using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Models
{
    public class Cell
    {
        public bool IsMine; // Клетка является Миной
        public bool IsFlaged; // Поставлен ли на клетку Флаг
        public bool IsRevealed; // Открыта ли клетка
        public int AdjacentMinesCount; // Количетво Мин вокруг клетки. 

    }
}
