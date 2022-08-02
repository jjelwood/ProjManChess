﻿using ChessApp.Business;
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
        Assert.True(board.Board[7, 0]?.IsWhite);
    }

    [Fact]
    public void FENTest6()
    {
        ChessBoard board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        Assert.True(!board.Board[0, 0]?.IsWhite);
    }

    [Fact]
    public void FENTest7()
    {
        ChessBoard board = new("8/8/2P1P3/3B4/2P1P3/8/8/8", 8, 8);
        Assert.True(board.TileIsOccupied(2, 2));
    }
}