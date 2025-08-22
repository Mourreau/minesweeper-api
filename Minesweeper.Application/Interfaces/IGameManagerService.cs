using FluentResults;
using Minesweeper.Application.DTO;
using Minesweeper.Core.Models;

namespace Minesweeper.Application.Interfaces;

public interface IGameManagerService
{
    Task<Result<Guid>> CreateNewGame(NewGamePresetRequest presetRequest);
    Task<Result<GameStateDto>> RevealCell(Guid gameId, CellPositionDto cellPositionDto);
    Task<Result<GameStateDto>> ToggleFlag(Guid gameId, CellPositionDto cellPositionDto);
    Task<Result<Game>> GetGameSession(Guid gameId);
}