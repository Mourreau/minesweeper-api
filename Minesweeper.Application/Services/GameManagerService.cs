using FluentResults;
using Microsoft.Extensions.Logging;
using Minesweeper.Application.DTO;
using Minesweeper.Application.Interfaces;
using Minesweeper.Core.Models;
using Minesweeper.Core.Settings;

namespace Minesweeper.Application.Services;

public class GameManagerService : IGameManagerService
{
    private readonly IGameSessionService _gameSessionService;
    private readonly ILogger<GameManagerService> _logger;

    public GameManagerService(IGameSessionService gameSessionService, ILogger<GameManagerService> logger)
    {
        _gameSessionService = gameSessionService;
        _logger = logger;
    }


    public Result<Guid> CreateNewGame(NewGamePresetRequest presetRequest)
    {
        var settings = GameSettings.Create(presetRequest.Difficulty);
        Game newGame = new Game(settings);

        if (!_gameSessionService.StoreNewGame(newGame, out var gameId))
        {
            _logger.LogWarning("Failed to store new game. Difficulty: {Difficulty}, Game Id: {GameId}", settings.Difficulty, gameId);
            return Result.Fail("Cannot create new game");
        }
        
        
        return Result.Ok(gameId);
    }
    
}