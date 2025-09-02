namespace Minesweeper.Application.DTO;

public class CellDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int? AdjacentMinesCount { get; set; }
    
    public bool? IsMine { get; set; }
}