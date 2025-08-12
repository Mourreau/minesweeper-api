using System.Runtime.InteropServices.ComTypes;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Minesweeper.Application.Helpers.Errors;

namespace Minesweeper.Presentation.Infrastructure.ResultMapping;

public static class ResultExtensions
{
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