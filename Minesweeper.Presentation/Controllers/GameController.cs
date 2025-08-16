using Microsoft.AspNetCore.Mvc;
using Minesweeper.Application.DTO;
using Minesweeper.Application.Interfaces;
using Minesweeper.Application.Mappers;
using Minesweeper.Presentation.Infrastructure.ResultMapping;

namespace Minesweeper.Presentation.Controllers;

[ApiController]
[Route("games/minesweeper")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IGameManagerService _gameManagerService;

    public GameController(
        ILogger<GameController> logger,
        IGameManagerService gameManagerService)
    {
        _logger = logger;
        _gameManagerService = gameManagerService;
    }

    [HttpPost]
    public IActionResult CreateNewGame([FromBody] NewGamePresetRequest request)
    {
        var result = _gameManagerService.CreateNewGame(request);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetGameById), new { id = result.Value }, null);
    }

    [HttpPatch("{id:guid}/reveal")]
    public IActionResult RevealCell([FromRoute] Guid id, [FromBody] CellPositionDto positionDto)
    {
        var result = _gameManagerService.RevealCell(id, positionDto);
        
        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameStateDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetGameById(Guid id)
    {
        var result = _gameManagerService.GetGameSession(id);

        if (result.IsFailed)
        {
            _logger.LogWarning("Game with ID {id} is not found", id);
            return NotFound(result.Errors);
        }

        _logger.LogInformation("Game created with ID {Id}", id);
        return Ok(result.Value);
    }
}