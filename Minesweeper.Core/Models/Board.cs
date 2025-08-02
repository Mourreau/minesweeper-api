namespace Minesweeper.Core.Models
{
    public class Board
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        public Cell[,] GameBoard { get; private set; }

        /// <summary>
        /// Ширина игрового поля (X) определяется размерами двумерного массива GameBoard
        /// </summary>
        private int Width => GameBoard.GetLength(0);

        /// <summary>
        /// Высота игрового поля (Y) определяется размерами двумерного массива GameBoard
        /// </summary>
        private int Height => GameBoard.GetLength(1);

        public Board(int boardWidth, int boardHeight)
        {
            GameBoard = GenerateBoard(boardWidth, boardHeight);
        }


        /// <summary>
        /// Генерация игрового поля, где каждая клетка представлена объектом Cell со значениями по умолчанию
        /// </summary>
        /// <param name="boardWidth">Ширина поля (Х)</param>
        /// <param name="boardHeight">Высота поля (Y)</param>
        /// <returns>Возвращает игровое поле состоящее из двумерного массива Cell</returns>
        private Cell[,] GenerateBoard(int boardWidth, int boardHeight)
        {
            Cell[,]
                generatedBoard =
                    new Cell[boardWidth, boardHeight]; // Создание двумерного массива Cell для игрового поля
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    generatedBoard[x, y] = new Cell // Инициализация каждой клетки игрового поля
                    (
                        x, // Установка координаты X для клетки
                        y // Установка координаты Y для клетки
                    );
                }
            }

            return generatedBoard;
        }

        /// <summary>
        /// Получить клетку игрового поля по координатам X Y.
        /// </summary>
        /// <param name="x">По Ширине</param>
        /// <param name="y">По Высоте</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of bounds of the board.");
            }

            return GameBoard[x, y];
        }

        /// <summary>
        /// Генерация позиций мин на игровом поле
        /// </summary>
        /// <param name="specifiedMinesQuantity">Заданное в параметрах количество мин</param>
        /// <param name="startedCell">Клетка на которую нажал игрок в начале игры</param>
        public void GenerateMinesPosition(int specifiedMinesQuantity, Cell startedCell)
        {
            int minesCount = 0;
            int cellCount = Width * Height; // Количество клеток на игровом поле
            int attempts = 0; // Количество попыток для генерации мин
            int maxAttempts =
                cellCount * 2; // Максимальное количество попыток для генерации мин, чтобы избежать бесконечного цикла


            if (specifiedMinesQuantity >= cellCount)
            {
                throw new ArgumentException(
                    "Specified mines quantity exceeds or equals the number of cells on the board.");
            }


            while (minesCount < specifiedMinesQuantity && attempts < maxAttempts)
            {
                attempts++;
                Cell selectedCell = GetCell(Random.Shared.Next(0, Width), Random.Shared.Next(0, Height));
                if (selectedCell.IsMine
                    || IsSaveZone(selectedCell, startedCell)
                    || (selectedCell.X == startedCell.X && selectedCell.Y == startedCell.Y))
                    continue; // Если клетка уже является миной, пропускаем итерацию и выбираем другую клетку
                selectedCell.IsMine = true;
                minesCount++;
            }
        }


        /// <summary>
        /// Проверка находится ли клетка в "Безопасной" зоне стартовой клетки.
        /// </summary>
        /// <param name="selectedCell"></param>
        /// <param name="startedCell"></param>
        /// <returns></returns>
        public bool IsSaveZone(Cell selectedCell, Cell startedCell)
        {
            return Math.Abs(selectedCell.X - startedCell.X) <= 1
                   && Math.Abs(selectedCell.Y - startedCell.Y) <= 1;
        }


        public void CalculateAdjacentMinesAtBoard()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cell searchingCell = GetCell(x, y);

                    CountMinesAroundCell(searchingCell);
                }
            }
        }

        public void CountMinesAroundCell(Cell searchingCell)
        {
            searchingCell.AdjacentMinesCount = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int tryX = searchingCell.X + i;
                    int tryY = searchingCell.Y + j;
                    if (TryPositionCell(tryX, tryY, out Cell positionCell) && positionCell.IsMine)
                    {
                        searchingCell.AdjacentMinesCount++;
                    }
                }
            }
        }

        private bool TryPositionCell(int tryX, int tryY, out Cell positionCell)
        {
            positionCell = default!;
            if (tryX < 0 || tryX >= Width || tryY < 0 || tryY >= Height)
            {
                return false;
            }

            positionCell = GameBoard[tryX, tryY];
            return true;
        }
    }
}
// Добавить проверку для счетчика соседних мин - если клетка находится у края, то проверять все 8 клеток вокруг нее не нужно
// Дополнение по UX: включить startedCell в «запретную зону»
// Сейчас мина не ставится только на первую клетку, но не исключается, что рядом всё заминировано, и у игрока будет автопоражение на 2 - 3 ходу
// Позже можешь расширить:
// Генерация мин с исключением startedCell и всех её соседей
// Пока не нужно — просто запомни как идею для улучшения UX.