using ChessApp.Business;
using ChessApp.Business.Moves;
using ChessApp.Business.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Tests;

public class BoardTests
{
    [Fact]
    public void FENTest1()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        Assert.NotNull(board);
    }

    [Fact]
    public void FENTest2()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        Assert.True(board.Board[0, 0] is Rook);
    }

    [Fact]
    public void FENTest3()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        Assert.True(board.Board[0, 1] is Knight);
    }

    [Fact]
    public void FENTest4()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        Assert.True(board.Board[4, 4] is null);
    }

    [Fact]
    public void FENTest5()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        Assert.True(board.Board[7, 0]?.Colour == PieceColour.White);
    }

    [Fact]
    public void FENTest6()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        Assert.True(board.Board[0, 0]?.Colour == PieceColour.Black);
    }

    [Fact]
    public void FENTest7()
    {
        ChessBoard board = new("8/8/2P1P3/3B4/2P1P3/8/8/8", 8, 8);
        Assert.True(board.TileIsOccupied(2, 2));
    }

    [Fact]
    public void TestSetMove1()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        var newBoard = ChessBoard.SetMove(board, new StandardMove(new(6, 0), new(4, 0)));
        Assert.True(board.Board != newBoard.Board);
    }

    [Fact]
    public void TestSetMove2()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        var newBoard = ChessBoard.SetMove(board, new StandardMove(new(6, 0), new(4, 0)));
        Assert.True(board.Board[6, 0]?.Position != newBoard.Board[6, 0]?.Position);
    }

    [Fact]
    public void TestCanCastle()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        board.MovePiece(new(7, 6), new(5, 5));
        board.MovePiece(new(6, 4), new(4, 4));
        board.MovePiece(new(7, 5), new(6, 4));
        Assert.True(board.CanCastle(PieceColour.White, 1));
    }
}
