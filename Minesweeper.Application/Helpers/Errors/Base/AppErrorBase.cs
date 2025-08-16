using FluentResults;

namespace Minesweeper.Application.Helpers.Errors.Base;

public abstract class AppErrorBase(AppErrorCode errorCode, string message) : Error(message)
{
    public AppErrorCode ErrorCode { get; } = errorCode;
    
}