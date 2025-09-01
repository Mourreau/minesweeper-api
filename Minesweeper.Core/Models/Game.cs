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
        /// Поле с ячейками, генерируется при старте игры.
        /// </summary>
        public Board GameBoard { get; private set; }

        public GameSettings Settings { get; private set; }


        public Game(GameSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings), "Game settings cannot be null.");
            GameBoard = new Board(settings.BoardWidth, settings.BoardHeight);
            _notMinesCellsCount = GameBoard.GameBoard.Length - Settings.MinesCount;
            CurrentGameStatus = GameStatus.Created;
        }

        /// <summary>
        /// Метод для запуска игровой сессии, которая начинается с первого клика игрока по любой клетке игрового поля.
        /// </summary>
        /// <param name="x">Координата X клетки на которую нажал игрок.</param>
        /// <param name="y">Координата Y клетки на которую нажал игрок.</param>
        public void StartGame(int x, int y)
        {
            Cell startedCell = GameBoard.GetCell(x, y);
            GameBoard.GenerateMinesPosition(Settings.MinesCount,
                startedCell); // При нажатии на клетку генерируем позицию мин.
            GameBoard
                .CalculateAdjacentMinesAtBoard(); // Считаем количество мин-соседей для каждой клетки на игровом поле.
            GameBoard.RevealAdjacentNotMineCells(
                startedCell); // Раскрываем все "Безопасные" пустые клетки от нажатой игроком.
            CurrentGameStatus = GameStatus.InProgress; // Назначаем статус игры - "В процессе".
        }

        /// <summary>
        /// Переключение паузы.
        /// </summary>
        public void TogglePauseGame()
        {
            if (CurrentGameStatus is GameStatus.Loose or GameStatus.Win)
                return; // Если игра уже завершена, то она не может быть поставлена на паузу.

            CurrentGameStatus =
                CurrentGameStatus == GameStatus.Paused
                    ? GameStatus.InProgress
                    : GameStatus.Paused; // Если игра на паузе и не завершена, то возобновляем.
        }


        public void RevealCell(int x, int y)
        {
            var cell = GameBoard.GetCell(x, y); // Получаем клетку с игрового поля.
            if (cell.IsMine)
            {
                RevealAllMines();
                CurrentGameStatus = GameStatus.Loose;
                return;
            }

            GameBoard.RevealAdjacentNotMineCells(cell); // Открыть клетку и все "Безопасные" клетки вокруг.
            CheckWin();
        }

        /// <summary>
        /// Открыть все Мины.
        /// </summary>
        public void RevealAllMines()
        {
            foreach (Cell cell in GameBoard.GameBoard)
            {
                if (cell.IsMine)
                    GameBoard.TryOpenCell(cell); // Если не получилось открыть клетку, то продолжить со следующей.
            }
        }

        public void CheckWin()
        {
            if (GameBoard.OpenedCells == _notMinesCellsCount && CurrentGameStatus == GameStatus.InProgress)
                CurrentGameStatus = GameStatus.Win;
        }

        public void ToggleFlag(int x, int y)
        {
            var cell = GameBoard.GetCell(x, y);

            if (CurrentGameStatus != GameStatus.InProgress)
                return;

            GameBoard.TryToggleFlag(cell);
        }

        private readonly int _notMinesCellsCount;
    }
}