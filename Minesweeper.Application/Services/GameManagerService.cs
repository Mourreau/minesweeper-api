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


    public Task<Result<Guid>> CreateNewGameByPreset(NewGamePresetRequest presetRequest, CancellationToken ct)
    {
        var settings = GameSettings.Create(presetRequest.Difficulty);
        var newGame = new Game(settings);

        if (!_gameSessionService.StoreNewGame(newGame, out var gameId))
        {
            _logger.LogWarning("Failed to store new game. Difficulty: {Difficulty}, Game Id: {GameId}",
                settings.Difficulty, gameId);
            return Task.FromResult(Result.Fail<Guid>("Cannot create new game"));
        }


        return Task.FromResult(Result.Ok(gameId));
    }

    public Task<Result<Guid>> CreateCustomNewGame(CreateNewCustomGameRequest customRequest, CancellationToken ct)
    {
        var settings = GameSettings.Create(
            customRequest.Difficulty,
            customRequest.BoardWidth,
            customRequest.BoardHeight,
            customRequest.MinesCount);
        
        var newGame = new Game(settings);
        
        if (!_gameSessionService.StoreNewGame(newGame, out var gameId))
        {
            _logger.LogWarning("Failed to store new game. Difficulty: {Difficulty}, Game Id: {GameId}",
                settings.Difficulty, gameId);
            return Task.FromResult(Result.Fail<Guid>("Cannot create new game"));
        }
        
        return Task.FromResult(Result.Ok(gameId));
    }

    public async Task<Result<GameStateDto>> RevealCell(Guid gameId, CellPositionDto cellPositionDto, CancellationToken ct)
    {
        var fetchResult = await GetGameSession(gameId, ct);
        
        if (fetchResult.IsFailed) 
            return Result.Fail<GameStateDto>(NotFoundError.GameNotFound(gameId));

        var game = fetchResult.Value;

        if (IsGameOver(game)) return Result.Fail<GameStateDto>(GameOverError.GameOver(game));
        if (IsNewGame()) StartGame();
        else MakeMove();

        return Result.Ok(_mapper.MapGameStateDto(game, gameId));


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
    

    public async Task<Result<GameStateDto>> ToggleFlag(Guid gameId, CellPositionDto cellPositionDto, CancellationToken ct)
    {
        var fetchResult = await GetGameSession(gameId, ct);

        if (fetchResult.IsFailed) 
            return Result.Fail<GameStateDto>(NotFoundError.GameNotFound(gameId));

        var game = fetchResult.Value;

        if (IsGameOver(game)) return Result.Fail<GameStateDto>(GameOverError.GameOver(game));

        game.ToggleFlag(cellPositionDto.X, cellPositionDto.Y);

        return Result.Ok(_mapper.MapGameStateDto(game, gameId));
    }
    

    public Task<Result<Game>> GetGameSession(Guid gameId, CancellationToken ct)
    {
        return _gameSessionService.TryGetGame(gameId, out var game)
            ? Task.FromResult(Result.Ok(game))
            : Task.FromResult(Result.Fail<Game>(NotFoundError.GameNotFound(gameId)));
    }

    public Task<Result<GameStateDto>> GetGameState(Guid gameId, CancellationToken ct)
    {
        var state = _mapper.MapGameStateDto(gameId);
        
        if (state.IsFailed) return Task.FromResult(Result.Fail<GameStateDto>(NotFoundError.GameNotFound(gameId)));
        
        return Task.FromResult(state);
    }

    private static bool IsGameOver(Game game)
    {
        return game.CurrentGameStatus is GameStatus.Lost or GameStatus.Won;
    }
}