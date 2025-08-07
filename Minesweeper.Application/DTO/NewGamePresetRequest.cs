using Minesweeper.Core.Enums;

namespace Minesweeper.Application.DTO;

public class NewGamePresetRequest
{
    public GameDifficulty Difficulty { get; set; }
}