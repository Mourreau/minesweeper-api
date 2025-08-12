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

        switch (error)
        {
            case NotFoundError: return controller.NotFound(error.Message);
            case GameOverError: return controller.BadRequest(error.Message);
            case InvalidCoordinatesError: return controller.BadRequest(error.Message);
            case AlreadyRevealedError: return controller.StatusCode(100, error.Message);
            default: controller.StatusCode(500, error.Message);
                break;
        }

        return controller.StatusCode(500, error.Message);
    }
    
    
    public static IActionResult ToActionResult(this ControllerBase controller, Result result)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        var error = result.Errors.First();

        switch (error)
        {
            case NotFoundError: return controller.NotFound(error.Message);
            case GameOverError: return controller.BadRequest(error.Message);
            case InvalidCoordinatesError: return controller.BadRequest(error.Message);
            case AlreadyRevealedError: return controller.StatusCode(100, error.Message);
            default: controller.StatusCode(500, error.Message);
                break;
        }

        return controller.StatusCode(500, error.Message);
    }
}