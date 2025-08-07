using Microsoft.AspNetCore.Mvc;
using Minesweeper.Application.DTO;
using Minesweeper.Application.Interfaces;
using Minesweeper.Application.Mappers;

namespace Minesweeper.Presentation.Controllers;

[ApiController]
[Route("game")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IGameSessionService _gameSessionService;
    private readonly IGameManagerService _gameManagerService;
    private readonly GameStateDtoMapper _mapper;

    public GameController(
        ILogger<GameController> logger,
        IGameSessionService gameSessionService,
        IGameManagerService gameManagerService,
        GameStateDtoMapper mapper)
    {
        _logger = logger;
        _gameSessionService = gameSessionService;
        _gameManagerService = gameManagerService;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult CreateNewGame([FromBody] NewGamePresetRequest request)
    {
        var result = _gameManagerService.CreateNewGame(request);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetGameById), new { id = result.Value }, null);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetGameById(Guid id)
    {
        var result = _mapper.MapGameStateDto(id);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }
}