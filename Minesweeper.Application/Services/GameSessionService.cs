using System.Collections.Concurrent;
using Minesweeper.Application.Interfaces;
using Minesweeper.Core.Models;

namespace Minesweeper.Application.Services;

public class GameSessionService : IGameSessionService
{
    private readonly ConcurrentDictionary<Guid, Game> _gameVault = new();
    
    /// <summary>
    /// Добавить новую игру в хранилище.
    /// </summary>
    /// <param name="game">Игра, которую требуется добавить в хранилище.</param>
    /// <returns>Id добавленной игры.</returns>
    /// <exception cref="Exception">Failed to store the new game session.</exception>
    public Guid StoreNewGame(Game game)
    {
        var gameId = Guid.NewGuid();
        if (!_gameVault.TryAdd(gameId, game))
            throw new Exception("Failed to store the new game session.");
        
        return gameId;
    }

    /// <summary>
    /// Получить игру из хранилища.
    /// </summary>
    /// <param name="id">Id получаемой игровой сессии.</param>
    /// <param name="game">Получаемая игровая сессия.</param>
    /// <returns>True и Game.</returns>
    public bool TryGetGame(Guid id, out Game game)
    {
        if (!_gameVault.TryGetValue(id, out game))
        {
            game = default!;
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// Удалить игровую сессию из хранилища.
    /// </summary>
    /// <param name="id">Id сессии.</param>
    /// <returns>Получилось или нет удалить сессию.</returns>
    public bool RemoveGame(Guid id)
    {
        return _gameVault.TryRemove(id, out Game game);
    }
}