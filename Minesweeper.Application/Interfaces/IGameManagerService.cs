using FluentResults;
using Minesweeper.Application.DTO;
using Minesweeper.Core.Models;

namespace Minesweeper.Application.Interfaces;

public interface IGameManagerService
{
    Result<Guid> CreateNewGame(NewGamePresetRequest presetRequest);
    Result<GameStateDto> RevealCell(Guid gameId, CellPositionDto cellPositionDto);
    Result<GameStateDto> ToggleFlag(Guid gameId, CellPositionDto cellPositionDto);
    Result<Game> GetGameSession(Guid gameId);
}