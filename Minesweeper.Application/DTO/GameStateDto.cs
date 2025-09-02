namespace Minesweeper.Application.DTO;

public class GameStateDto
{
    public Guid Id { get; set; }
    public string GameStatus { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public List<List<CellDto>> GameBoard { get; set; }
}