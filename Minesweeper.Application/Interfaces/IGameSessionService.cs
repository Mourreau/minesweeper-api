using Minesweeper.Core.Models;

namespace Minesweeper.Application.Interfaces;

public interface IGameSessionService
{
    public bool StoreNewGame(Game game,  out Guid gameId);
    public bool TryGetGame(Guid id,  out Game game);
    public bool RemoveGame(Guid id);
}