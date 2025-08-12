using FluentResults;
using Microsoft.Extensions.Logging;
using Minesweeper.Application.DTO;
using Minesweeper.Application.Helpers.Errors;
using Minesweeper.Application.Interfaces;
using Minesweeper.Application.Mappers;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Models;
using Minesweeper.Core.Settings;

namespace Minesweeper.Application.Services;

public class GameManagerService : IGameManagerService
{
    private readonly IGameSessionService _gameSessionService;
    private readonly ILogger<GameManagerService> _logger;
    private readonly GameStateDtoMapper _mapper;

    public GameManagerService(
        IGameSessionService gameSessionService,
        ILogger<GameManagerService> logger,
        GameStateDtoMapper mapper)
    {
        _gameSessionService = gameSessionService;
        _logger = logger;
        _mapper = mapper;
    }


    public Result<Guid> CreateNewGame(NewGamePresetRequest presetRequest)
    {
        var settings = GameSettings.Create(presetRequest.Difficulty);
        Game newGame = new Game(settings);

        if (!_gameSessionService.StoreNewGame(newGame, out var gameId))
        {
            _logger.LogWarning("Failed to store new game. Difficulty: {Difficulty}, Game Id: {GameId}",
                settings.Difficulty, gameId);
            return Result.Fail("Cannot create new game");
        }


        return Result.Ok(gameId);
    }

    public Result<GameStateDto> RevealCell(Guid gameId, CellPositionDto cellPositionDto)
    {
        var fetchResult = GetGameSession(gameId);
        
        if (fetchResult.IsFailed) return Result.Fail(NotFoundError.GameNotFound(gameId));
        
        var game = fetchResult.Value;    

        if (IsGameOver(game)) return Result.Fail(GameOverError.GameOver(game));
        if (IsNewGame()) StartGame();
        else MakeMove();

        return Result.Ok(_mapper.MapGameStateDto(game));

        

        void MakeMove()
        {
            game.RevealCell(cellPositionDto.X, cellPositionDto.Y);
        }

        bool IsNewGame()
        {
            return game.CurrentGameStatus is GameStatus.Created;
        }

        void StartGame()
        {
            game.StartGame(cellPositionDto.X, cellPositionDto.Y);
        }
    }

    private bool IsGameOver(Game game)
    {
        return game.CurrentGameStatus is GameStatus.Loose or GameStatus.Win;
    }

    public Result<GameStateDto> ToggleFlag(Guid gameId, CellPositionDto cellPositionDto)
    {
        var fetchResult = GetGameSession(gameId);
        
        if (fetchResult.IsFailed) return Result.Fail(NotFoundError.GameNotFound(gameId));
        
        var game = fetchResult.Value;    
        
        if (IsGameOver(game)) return Result.Fail(GameOverError.GameOver(game));
        
        game.ToggleFlag(cellPositionDto.X, cellPositionDto.Y);
        
        return Result.Ok(_mapper.MapGameStateDto(game));
    }

    private Result<Game> GetGameSession(Guid gameId)
    {
        return _gameSessionService.TryGetGame(gameId,  out var game)
            ? Result.Ok(game)
            : Result.Fail(NotFoundError.GameNotFound(gameId));
    }


}