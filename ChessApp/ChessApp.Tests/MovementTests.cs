using ChessApp.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Tests;

public class MovementTests
{
    [Theory]
    [InlineData("8/8/8/8/8/8/8/B7", 7, 0, 7)]
    [InlineData("8/8/8/8/8/8/1B6/8", 6, 1, 9)]
    [InlineData("8/8/2P1P3/3B4/2P1P3/8/8/8", 3, 3, 0)]
    [InlineData("8/1P3P2/8/3B4/8/1P3P2/8/8", 3, 3, 4)]
    [InlineData("8/8/8/3B4/8/8/8/8", 3, 3, 13)]
    [InlineData("8/1P3P2/2B5/8/8/1P3P2/8/8", 2, 2, 6)]
    public void BishopMovementTest(string FEN, int row, int column, int expectedMoves)
    {
        ExpectedMoves(FEN, row, column, expectedMoves);
    }

    [Theory]
    [InlineData("8/8/8/8/8/P7/8/P7", 7, 0, 1)]
    [InlineData("8/8/8/8/8/P7/P7/P7", 7, 0, 0)]
    [InlineData("8/8/8/8/8/8/P7/P7", 7, 0, 0)]
    [InlineData("8/8/8/8/8/8/1p6/P7", 7, 0, 3)]
    public void PawnMovementTest(string FEN, int row, int column, int expectedMoves)
    {
        ExpectedMoves(FEN, row, column, expectedMoves);
    }

    [Theory]
    [InlineData("K7/8/8/8/8/8/8/8", 0, 0, 3)]
    [InlineData("8/1K6/8/8/8/8/8/8", 1, 1, 8)]
    [InlineData("P8/1K6/8/8/8/8/8/8", 1, 1, 7)]
    [InlineData("p8/1K6/8/8/8/8/8/8", 1, 1, 8)]
    public void KingMovementTest(string FEN, int row, int column, int expectedMoves)
    {
        ExpectedMoves(FEN, row, column, expectedMoves);
    }

    [Theory]
    [InlineData("R7/8/8/8/8/8/8/8", 0, 0, 14)]
    [InlineData("8/1R7/8/8/8/8/8/8", 1, 1, 14)]
    [InlineData("8/6R1/8/8/8/8/8/8", 1, 6, 14)]
    [InlineData("8/8/8/8/7R/8/8/8", 4, 7, 14)]
    [InlineData("8/1R/8/8/8/8/8/7R", 7, 7, 14)]
    [InlineData("RP6/P7/8/8/8/8/8/8", 0, 0, 0)]
    [InlineData("R1P5/8/P7/8/8/8/8/8", 0, 0, 2)]
    public void RookMovementTest(string FEN, int row, int column, int expectedMoves)
    {
        ExpectedMoves(FEN, row, column, expectedMoves);
    }

    [Theory]
    [InlineData("Q7/8/8/8/8/8/8/8", 0, 0, 21)]
    [InlineData("8/1Q7/8/8/8/8/8/8", 1, 1, 23)]
    [InlineData("8/6Q1/8/8/8/8/8/8", 1, 6, 23)]
    [InlineData("8/8/8/8/7Q/8/8/8", 4, 7, 21)]
    [InlineData("8/1Q/8/8/8/8/8/7Q", 7, 7, 19)]
    [InlineData("QP6/P7/8/8/8/8/8/8", 0, 0, 7)]
    [InlineData("QP6/PP6/8/8/8/8/8/8", 0, 0, 0)]
    [InlineData("Q1P5/8/P7/8/8/8/8/8", 0, 0, 9)]
    [InlineData("Q1p5/8/p7/8/8/8/8/8", 0, 0, 11)]
    public void QueenMovementTest(string FEN, int row, int column, int expectedMoves)
    {
        ExpectedMoves(FEN, row, column, expectedMoves);
    }

    [Theory]
    [InlineData("N7/8/8/8/8/8/8/8", 0, 0, 2)]
    [InlineData("1N6/8/8/8/8/8/8/8", 0, 1, 3)]

    public void KnightMovementTest(string FEN, int row, int column, int expectedMoves)
    {
        ExpectedMoves(FEN, row, column, expectedMoves);
    }

    private static void ExpectedMoves(string FEN, int row, int column, int expectedMoves)
    {
        ChessBoard board = new(FEN, 8, 8);

        var piece = board.Board[row, column];
        var moves = piece?.GetMoves(board);
        var movesLength = moves?.Count();
        Assert.Equal(expectedMoves, movesLength);
    }

}
