using Microsoft.AspNetCore.Mvc;
using Minesweeper.Application.DTO;
using Minesweeper.Application.Interfaces;
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
    
    [HttpPost("preset")]
    public async Task<IActionResult> CreateNewGameByPreset([FromBody] NewGamePresetRequest request, CancellationToken ct)
    {
        var result = await _gameManagerService.CreateNewGameByPreset(request, ct);

        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        // Получаем состояние игры - GameStateDto.
        var state = await _gameManagerService.GetGameState(result.Value, ct);
        
        if (state.IsFailed)
            return BadRequest(state.Errors);

        return CreatedAtAction(nameof(GetGameById), new { id = result.Value }, state.Value);
    }
    
    [HttpPost("custom")]
    public async Task<IActionResult> CreateCustomNewGame([FromBody] CreateNewCustomGameRequest request, CancellationToken ct)
    {
        var result = await _gameManagerService.CreateCustomNewGame(request, ct);

        if (result.IsFailed)
            return BadRequest(result.Errors);
 
        // Получаем состояние игры - GameStateDto.
        var state = await _gameManagerService.GetGameState(result.Value, ct);
        
        if (state.IsFailed)
            return BadRequest(state.Errors);

        return CreatedAtAction(nameof(GetGameById), new { id = result.Value }, state.Value);
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
        
        var result = await _gameManagerService.GetGameState(id, ct);

        if (result.IsFailed)
        {
            _logger.LogWarning("Game with ID {id} is not found", id);
            return NotFound(result.Errors);
        }

        _logger.LogInformation("Game was received with ID {Id}", id);
        return Ok(result.Value);
    }
}