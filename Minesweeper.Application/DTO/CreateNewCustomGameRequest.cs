using Minesweeper.Core.Enums;

namespace Minesweeper.Application.DTO;

public class CreateNewCustomGameRequest
{
    public GameDifficulty Difficulty { get; set; } = GameDifficulty.Custom;
    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }
    public int MinesCount { get; set; }
}