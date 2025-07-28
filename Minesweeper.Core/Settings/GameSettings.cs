using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Settings
{
    public class GameSettings
    {
        public GameSettings(int boardWidth, int boardHeight, int minesCount, GameDifficulty difficulty)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            MinesCount = minesCount;
            Difficulty = difficulty;
        }

        public int BoardWidth { get; init; }
        public int BoardHeight { get; init; }
        public int MinesCount { get; init; }
        public GameDifficulty Difficulty { get; init; } = GameDifficulty.Easy; // Уровень сложности по умолчанию



        public static GameSettings Create(GameDifficulty difficulty,
                                          int width = default,
                                          int height = default,
                                          int mines = default) =>
            difficulty switch
            {
                GameDifficulty.Easy => new(9, 9, 10, difficulty),
                GameDifficulty.Medium => new(16, 16, 40, difficulty),
                GameDifficulty.Hard => new(30, 16, 99, difficulty),
                GameDifficulty.Custom => new(width, height, mines, difficulty),
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty)),
            };
        
    }
}

