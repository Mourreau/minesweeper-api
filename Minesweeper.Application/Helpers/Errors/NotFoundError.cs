
using Minesweeper.Application.Helpers.Errors.Base;

namespace Minesweeper.Application.Helpers.Errors;

public class NotFoundError(string message) : AppErrorBase(AppErrorCode.NotFound, message)
{
    public static NotFoundError GameNotFound(Guid id)
        => new($"Game with Id: {id} is not found");
}