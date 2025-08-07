using FluentResults;
using Minesweeper.Application.DTO;

namespace Minesweeper.Application.Interfaces;

public interface IGameManagerService
{
    Result<Guid> CreateNewGame(NewGamePresetRequest presetRequest);
}