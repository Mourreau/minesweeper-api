using Minesweeper.Application.DTO;
using Minesweeper.Application.Helpers.Errors.Base;
using Minesweeper.Core.Models;

namespace Minesweeper.Application.Helpers.Errors;

public class InvalidCoordinatesError(string message) : AppErrorBase(AppErrorCode.InvalidCoordinates, message)
{
    public static InvalidCoordinatesError OutOfBoard(CellPositionDto position)
    => new($"Position: {position.X}:{position.Y} is out of board");
}