using FluentResults;
using Minesweeper.Application.DTO;
using Minesweeper.Core.Models;

namespace Minesweeper.Application.Interfaces;

public interface IGameManagerService
{
    Task<Result<Guid>> CreateNewGameByPreset(NewGamePresetRequest presetRequest, CancellationToken ct);
    Task<Result<Guid>> CreateCustomNewGame(CreateNewCustomGameRequest customRequest, CancellationToken ct);
    Task<Result<GameStateDto>> RevealCell(Guid gameId, CellPositionDto cellPositionDto, CancellationToken ct);
    Task<Result<GameStateDto>> ToggleFlag(Guid gameId, CellPositionDto cellPositionDto, CancellationToken ct);
    Task<Result<Game>> GetGameSession(Guid gameId, CancellationToken ct);
}