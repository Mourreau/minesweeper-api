using Minesweeper.Core.Models;
using Xunit.Abstractions;

namespace Minesweeper.Tests.Models
{
    public class BoardTests
    {
        private readonly ITestOutputHelper _output;

        public BoardTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void EqualityTest_ShouldReturnTrue_IfCellsAreEquals()
        {
            Board testBoard1 = new Board(10, 10);
            Board testBoard2 = new Board(10, 10);

            Cell testCell1 = testBoard1.GameBoard[1, 1];
            Cell testCell2 = testCell1;
            Cell testCell3 = testBoard1.GameBoard[1, 1];

            Assert.Equal(testCell1, testCell2);
        }

        [Fact]
        public void EqualityTest_ShouldReturnTrue_IfCellsAreEquals2()
        {
            Board testBoard1 = new Board(10, 10);
            Board testBoard2 = new Board(10, 10);

            Cell testCell1 = testBoard1.GameBoard[1, 1];
            Cell testCell2 = testCell1;
            Cell testCell3 = testBoard1.GameBoard[1, 1];

            Assert.Equal(testCell2, testCell3);
        }

        [Fact]
        public void EqualityTest_ShouldReturnTrue_IfCellsAreEquals3()
        {
            Board testBoard1 = new Board(10, 10);
            Board testBoard2 = new Board(10, 10);

            Cell testCell1 = testBoard1.GameBoard[1, 1];
            Cell testCell2 = testCell1;
            Cell testCell3 = testBoard1.GameBoard[1, 1];

            Assert.Equal(testCell1, testCell3);
        }

        [Fact]
        public void EqualityTest_ShouldReturnTrue_IfCellsAreEquals4()
        {
            Board testBoard1 = new Board(10, 10);
            Board testBoard2 = new Board(10, 10);

            Cell testCell1 = testBoard1.GameBoard[1, 1];
            Cell testCell2 = testCell1;
            Cell testCell3 = testBoard1.GameBoard[1, 1];

            Assert.True(testCell1 == testCell2);
        }


        [Fact]
        public void GenerateMinesPosition_ShouldReturnTrue_WhenMinesAreGeneratedCorrectly_GeneratedTest()
        {
            Board testBoard = new Board(10, 10);
            Cell startingCell = testBoard.GetCell(0, 0);
            int specifiedMinesQuantity = 10;
            testBoard.GenerateMinesPosition(specifiedMinesQuantity, startingCell);
            int actualMinesCount = 0;
            for (int x = 0; x < testBoard.GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < testBoard.GameBoard.GetLength(1); y++)
                {
                    if (testBoard.GameBoard[x, y].IsMine)
                    {
                        actualMinesCount++;
                    }
                }
            }
            Assert.Equal(specifiedMinesQuantity, actualMinesCount);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(40)]
        public void GenerateMinesPosition_ShouldReturnTrue_WhenMinesAreGeneratedCorrectly_HandWriteTest(int minesCount)
        {
            Board testBoard = new Board(10, 10);

            testBoard.GenerateMinesPosition(minesCount, testBoard.GameBoard[5,5]);
            _output.WriteLine($"Mines count: {minesCount}");

            int actualMinesCount = 0;
            for (int x = 0; x < testBoard.GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < testBoard.GameBoard.GetLength(1); y++)
                {
                    if (testBoard.GameBoard[x,y].IsMine)
                        actualMinesCount++;
                }
            }

            _output.WriteLine($"Actual mines count: {actualMinesCount}");
            Assert.Equal(minesCount, actualMinesCount);
        }

        [Theory]
        [InlineData(10, 10, 10)]
        [InlineData(100, 10, 10)]
        [InlineData(1000, 10, 10)]
        public void GenerateMinesPosition_ShouldReturnException_WhenMinesCountExceedsOrEqualTheBoardCellsCount(int minesCount, int boardX, int boardY)
        {
            string expectedExceptionMessage = "Specified mines quantity exceeds or equals the number of cells on the board.";
            Board testBoard = new Board(boardX, boardY);

            var actualMessage = Assert.Throws<ArgumentException>(() => testBoard.GenerateMinesPosition(minesCount, testBoard.GameBoard[5, 5]));



            Assert.Equal(expectedExceptionMessage, actualMessage.Message);
        }
        
        [Fact]
        public void CountMinesAroundCell_ShouldReturnRightNumberOfAdjacentMines_OfNotEdgeCell()
        {
            int expectedAdjacentMinesCount = 4;
            
            Board testBoard = new Board(10, 10);
            Cell startingCell = testBoard.GetCell(5, 5);
            Cell searchingCell = testBoard.GetCell(2, 2);
            
            testBoard.GameBoard[1, 1].IsMine = true;
            testBoard.GameBoard[3, 3].IsMine = true;
            testBoard.GameBoard[1, 2].IsMine = true;
            testBoard.GameBoard[3, 2].IsMine = true;
            
            testBoard.CountMinesAroundCell(searchingCell);


            int actualAdjacentMinesCount = searchingCell.AdjacentMinesCount;
            Assert.Equal(expectedAdjacentMinesCount, actualAdjacentMinesCount);
        }
        
        [Fact]
        public void CountMinesAroundCell_ShouldReturnRightNumberOfAdjacentMines_OfEdgeCell()
        {
            int expectedAdjacentMinesCount = 3;
            
            Board testBoard = new Board(10, 10);
            Cell startingCell = testBoard.GetCell(5, 5);
            Cell searchingCell = testBoard.GetCell(0, 0);
            
            testBoard.GameBoard[0, 1].IsMine = true;
            testBoard.GameBoard[1, 1].IsMine = true;
            testBoard.GameBoard[1, 0].IsMine = true;
            
            testBoard.CountMinesAroundCell(searchingCell);


            int actualAdjacentMinesCount = searchingCell.AdjacentMinesCount;
            Assert.Equal(expectedAdjacentMinesCount, actualAdjacentMinesCount);
        }
    }
}