﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ChessApp.Business.Moves;

namespace ChessApp.Business
{
    public class Game
    {
        private readonly ChessBoard _board;

        public Game(ChessBoard board)
        {
            _board = board;
        }

        public ChessBoard Board
        {
            get { return _board; }
        }

        private int _moves = 0;

        public int Moves
        {
            get { return _moves; }
            set { _moves = value; }
        }

        public int PlayerToMove => (Moves + 1) % 2;

        public static bool MoveIsValid(ChessBoard board, int playerToMove, IMove move)
        {
            if (move is DoublePawnMove doublePawnMove)
            {
                return DoublePawnMoveIsValid(board, playerToMove, doublePawnMove);
            }
            if (move is StandardMove standardMove)
            {
                return StandardMoveIsValid(board, playerToMove, standardMove);
            }
            if (move is CastlingMove castlingMove)
            {
                return board.CanCastle(castlingMove.Player, castlingMove.Direction);
            }
            if (move is EnPassantMove)
            {
                return true;
            }
            throw new NotImplementedException();
        }

        private static bool DoublePawnMoveIsValid(ChessBoard board, int playerToMove, DoublePawnMove move)
        {
            // If no piece a from location then move is invalid
            if (!board.TileIsOccupied(move.From))
            {
                return false;
            }

            IPiece piece = board[move.From]!;
            int player = piece.IsWhite ? 1 : 0;

            if (player != playerToMove
                || Math.Abs(move.From.Row - move.To.Row) != 2
                || move.From.Column != move.To.Column
                || PlayerIsCheckedAfterMove(board, player, move))
            {
                return false;
            }

            return true;
        }

        public static bool StandardMoveIsValid(ChessBoard board, int playerToMove, StandardMove move)
        {
            // If no piece a from location then move is invalid
            if (!board.TileIsOccupied(move.From))
            {
                return false;
            }

            IPiece piece = board[move.From]!;
            int player = piece.IsWhite ? 1 : 0;

            // Move not valid if it's not in the moveable tiles of the piece
            // If it's not the right colour
            // Or if you move into check
            if (player != playerToMove
                || !piece.GetMoveableTiles(board).Contains(move.To)
                || PlayerIsCheckedAfterMove(board, player, move))
            {
                return false;
            }

            return true;
        }

        public static IEnumerable<IMove> GetValidMovesForPiece(IPiece piece, ChessBoard board, int player)
        {
            return piece.GetMoves(board).Where(m => MoveIsValid(board, player, m));
        }

        public static bool PlayerIsCheckedAfterMove(ChessBoard board, int player, IMove move)
        {
            var newBoard = ChessBoard.SetMove(board, move);
            if (ChessBoard.PlayerIsChecked(newBoard, player))
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<IMove> AllMovesForPlayer(ChessBoard board, int player)
        {
            List<IMove> result = new();
            foreach (var piece in board.Board)
            {
                if (piece is null)
                    continue;

                foreach (Tile tile in piece.GetMoveableTiles(board))
                {
                    var move = new StandardMove(tile, piece.Position);
                    if (MoveIsValid(board, player, move))
                    {
                        result.Add(move);
                    }
                }
            }
            return result;
        }

        public void MovePiece(IMove move)
        {
            if (!MoveIsValid(Board, PlayerToMove, move))
            {
                throw new ArgumentException("Move invalid");
            }

            move.DoMove(Board);
            Moves++;
            Board.TickEnPassantSquares();
        }
    }
}
