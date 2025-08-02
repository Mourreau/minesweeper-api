using Minesweeper.Core.Enums;
using Minesweeper.Core.Settings;

namespace Minesweeper.Core.Models
{
    public class Game
    {

        /// <summary>
        /// Статус игры в данный момент. Например, создана, запущена, завершена.
        /// </summary>
        public GameStatus CurrentGameStatus { get; private set; }

        /// <summary>
        /// Поле с ячейками, генерируется при старте игры
        /// </summary>
        public Board GameBoard { get; private set; }
        
        public GameSettings Settings { get; private set; }

        public Game(GameSettings settings)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings), "Game settings cannot be null.");
            GameBoard = new Board(settings.BoardWidth, settings.BoardHeight);
            CurrentGameStatus = GameStatus.Created;

        }
    }
}
