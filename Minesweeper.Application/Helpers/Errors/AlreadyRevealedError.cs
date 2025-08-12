using Minesweeper.Application.Helpers.Errors.Base;

namespace Minesweeper.Application.Helpers.Errors;

public class AlreadyRevealedError(string message) : AppErrorBase(AppErrorCode.AlreadyRevealed, message)
{
    
}