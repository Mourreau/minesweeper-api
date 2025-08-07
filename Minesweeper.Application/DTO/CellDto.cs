namespace Minesweeper.Application.DTO;

public class CellDto
{
    public int x { get; set; }
    public int y { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int? AdjacentMinesCount { get; set; }
}