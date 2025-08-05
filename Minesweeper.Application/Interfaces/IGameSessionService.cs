using Minesweeper.Core.Models;

namespace Minesweeper.Application.Interfaces;

public interface IGameSessionService
{
    public Guid StoreNewGame(Game game);
    public bool TryGetGame(Guid id,  out Game game);
    public bool RemoveGame(Guid id);
}