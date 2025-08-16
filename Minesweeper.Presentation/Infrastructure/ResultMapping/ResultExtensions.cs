using System.Runtime.InteropServices.ComTypes;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Minesweeper.Application.Helpers.Errors;
using Minesweeper.Application.Helpers.Errors.Base;

namespace Minesweeper.Presentation.Infrastructure.ResultMapping;

public static class ResultExtensions
{
    private static readonly IReadOnlyDictionary<AppErrorCode, Func<ControllerBase, Error, IActionResult>> CodeMap =
        new Dictionary<AppErrorCode, Func<ControllerBase, Error, IActionResult>>
        {
            [AppErrorCode.NotFound] = (c, e) => c.NotFound(e.Message),
            [AppErrorCode.GameOver] = (@c, e) => c.BadRequest(e.Message),
            [AppErrorCode.InvalidCoordinates] = (c, e) => c.BadRequest(e.Message),
            [AppErrorCode.AlreadyRevealed] = (c, e) => c.Ok(e.Message),
        };
    
    
    public static IActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
    {
        if (result.IsSuccess)
            return controller.Ok(result.Value);

        var error = result.Errors.First();

        return error switch
        {
            NotFoundError => controller.NotFound(error.Message),
            GameOverError => controller.BadRequest(error.Message),
            InvalidCoordinatesError => controller.BadRequest(error.Message),
            AlreadyRevealedError => controller.Ok(error.Message),
            _ => controller.StatusCode(500, error.Message)
        };
    }


    public static IActionResult ToActionResult(this ControllerBase controller, Result result)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        var error = result.Errors.First();

        return error switch
        {
            NotFoundError => controller.NotFound(error.Message),
            GameOverError => controller.BadRequest(error.Message),
            InvalidCoordinatesError => controller.BadRequest(error.Message),
            AlreadyRevealedError => controller.Ok(error.Message),
            _ => controller.StatusCode(500, error.Message)
        };
    }
}