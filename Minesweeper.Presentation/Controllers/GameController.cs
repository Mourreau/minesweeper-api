using Microsoft.AspNetCore.Mvc;
using Minesweeper.Application.DTO;
using Minesweeper.Application.Interfaces;

namespace Minesweeper.Presentation.Controllers;

[ApiController]
[Route("game")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IGameSessionService _gameSessionService;
    private readonly IGameManagerService _gameManagerService;
    
    public GameController(
        ILogger<GameController> logger, 
        IGameSessionService gameSessionService, 
        IGameManagerService gameManagerService)
    {
        _logger = logger;
        _gameSessionService = gameSessionService;
        _gameManagerService = gameManagerService;
    }

    [HttpPost]
    public IActionResult CreateNewGame([FromBody] NewGamePresetRequest request)
    {
        var result = _gameManagerService.CreateNewGame(request);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetGameById), new {id = result.Value}, null);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetGameById(Guid id)
    {
        throw new Exception();
    }


    
}