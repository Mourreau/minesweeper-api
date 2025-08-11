using FluentResults;
using Minesweeper.Application.DTO;

namespace Minesweeper.Application.Interfaces;

public interface IGameManagerService
{
    Result<Guid> CreateNewGame(NewGamePresetRequest presetRequest);
    Result<GameStateDto> RevealCell(Guid gameId, CellPositionDto cellPositionDto);
    Result<GameStateDto> ToggleFlags(Guid gameId, CellPositionDto cellPositionDto);
}