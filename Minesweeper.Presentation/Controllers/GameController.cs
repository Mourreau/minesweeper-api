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
    public async Task<IActionResult> CreateNewGame([FromBody] NewGamePresetRequest request, CancellationToken ct)
    {
        var result = await _gameManagerService.CreateNewGame(request, ct);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        //TODO: Возможно стоит возвращать GameStateDto вместо null.
        return CreatedAtAction(nameof(GetGameById), new { id = result.Value }, null);
    }

    [HttpPatch("{id:guid}/reveal")]
    public async Task<IActionResult> RevealCell([FromRoute] Guid id, [FromBody] CellPositionDto positionDto, CancellationToken ct)
    {
        var result = await _gameManagerService.RevealCell(id, positionDto, ct);
        
        return this.ToActionResult(result);
    }

    [HttpPatch("{id:guid}/toggle-flag")]
    public async Task<IActionResult> ToggleFlag([FromRoute] Guid id, [FromBody] CellPositionDto positionDto, CancellationToken ct)
    {
        var result = await _gameManagerService.ToggleFlag(id,  positionDto, ct);
        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameStateDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameById(Guid id, CancellationToken ct)
    {
        //TODO: Мне разве нужно здесь возвращать Game, а не GameStateDto?
        
        var result = await _gameManagerService.GetGameSession(id, ct);

        if (result.IsFailed)
        {
            _logger.LogWarning("Game with ID {id} is not found", id);
            return NotFound(result.Errors);
        }

        _logger.LogInformation("Game was received with ID {Id}", id);
        return Ok(result);
    }
}