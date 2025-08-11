using FluentResults;
using Minesweeper.Application.DTO;
using Minesweeper.Application.Interfaces;
using Minesweeper.Core.Models;

namespace Minesweeper.Application.Mappers;

public class GameStateDtoMapper(IGameSessionService gameSessionService)
{
    public Result<GameStateDto> MapGameStateDto(Guid gameId)
    {
        if (!gameSessionService.TryGetGame(gameId, out var currentGame))
            return Result.Fail("Game not found");

        var gameState = new GameStateDto
        {
            GameStatus = currentGame.CurrentGameStatus.ToString(),
            GameBoard = MapGameBoard(currentGame),
            Width = currentGame.GameBoard.Width,
            Height = currentGame.GameBoard.Height
        };

        return Result.Ok(gameState);
    }
    
    public GameStateDto MapGameStateDto(Game game)
    {

        var gameState = new GameStateDto
        {
            GameStatus = game.CurrentGameStatus.ToString(),
            GameBoard = MapGameBoard(game),
            Width = game.GameBoard.Width,
            Height = game.GameBoard.Height
        };

        return gameState;
    }

    private List<List<CellDto>> MapGameBoard(Game game)
    {
        var boardDto = new List<List<CellDto>>();

        for (int y = 0; y < game.GameBoard.Height; y++)
        {
            var row = new List<CellDto>();

            for (int x = 0; x < game.GameBoard.Width; x++)
            {
                var cell = game.GameBoard.GameBoard[x, y];
                var cellDto = new CellDto
                {
                    X = cell.X,
                    Y = cell.Y,
                    IsRevealed = cell.IsRevealed,
                    IsFlagged = cell.IsFlagged,
                    AdjacentMinesCount =
                        cell.IsRevealed
                            ? cell.AdjacentMinesCount
                            : null
                };
                row.Add(cellDto);
            }

            boardDto.Add(row);
        }

        return boardDto;
    }
}