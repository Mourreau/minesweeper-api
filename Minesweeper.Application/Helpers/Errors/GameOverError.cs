using Minesweeper.Application.Helpers.Errors.Base;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Models;

namespace Minesweeper.Application.Helpers.Errors;

public class GameOverError(string message) : AppErrorBase(AppErrorCode.GameOver, message)
{
    public static GameOverError GameOver(Game game)
        => new($"Game over. Player {(
            game.CurrentGameStatus == GameStatus.Win
                ? "won"
                : "lost")}");
}