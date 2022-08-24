using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ChessApp.Business.Moves;
using ChessApp.Business.Exceptions;

namespace ChessApp.Business
{
    /// <summary>
    /// Class respresenting a game of chess
    /// </summary>
    public class Game
    {
        private readonly ChessBoard _board;

        public Game(ChessBoard board)
        {
            _board = board;
        }

        /// <summary>
        /// The board of the game
        /// </summary>
        public ChessBoard Board
        {
            get { return _board; }
        }

        private int _moves = 0;

        /// <summary>
        /// Amount of moves played
        /// </summary>
        public int Moves
        {
            get { return _moves; }
            set { _moves = value; }
        }

        /// <summary>
        /// The current player whose turn it is
        /// </summary>
        public PieceColour PlayerToMove => (PieceColour)((Moves + 1) % 2);

        /// <summary>
        /// Determines whether a move is valid
        /// </summary>
        /// <param name="board">Board the move is being played on</param>
        /// <param name="playerToMove">Player whose turn it is</param>
        /// <param name="move">The move to validate</param>
        /// <returns>Whether the move is valid on the given board</returns>
        public static bool MoveIsValid(ChessBoard board, PieceColour playerToMove, IMove move)
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
            return true;
        }

        /// <summary>
        /// Determines whether a double pawn move is valid
        /// </summary>
        /// <param name="board">Board the move is being played on</param>
        /// <param name="playerToMove">Player whose turn it is</param>
        /// <param name="move">The move to validate</param>
        /// <returns>Whether the dobule pawn move is valid on the given board</returns>
        private static bool DoublePawnMoveIsValid(ChessBoard board, PieceColour playerToMove, DoublePawnMove move)
        {
            // If no piece a from location then move is invalid
            if (!board.TileIsOccupied(move.From))
            {
                return false;
            }

            IPiece piece = board[move.From]!;

            if (piece.Colour != playerToMove
                || Math.Abs(move.From.Row - move.To.Row) != 2
                || move.From.Column != move.To.Column
                || PlayerIsCheckedAfterMove(board, piece.Colour, move))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether a standard move is valid
        /// </summary>
        /// <param name="board">Board the move is being played on</param>
        /// <param name="playerToMove">Player whose turn it is</param>
        /// <param name="move">The move to validate</param>
        /// <returns>Whether the standard move is valid on the given board</returns>
        public static bool StandardMoveIsValid(ChessBoard board, PieceColour playerToMove, StandardMove move)
        {
            // If no piece a from location then move is invalid
            if (!board.TileIsOccupied(move.From))
            {
                return false;
            }

            IPiece piece = board[move.From]!;

            // Move not valid if it's not in the moveable tiles of the piece
            // If it's not the right colour
            // Or if you move into check
            if (piece.Colour != playerToMove
                || !piece.GetMoveableTiles(board).Contains(move.To)
                || PlayerIsCheckedAfterMove(board, piece.Colour, move))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets all valid moves of a piece
        /// </summary>
        /// <param name="piece">The piece to check</param>
        /// <returns>An enumerable of all valid moves for the piece</returns>
        public IEnumerable<IMove> GetValidMovesForPiece(IPiece piece)
        {
            return piece.GetMoves(Board).Where(m => MoveIsValid(Board, PlayerToMove, m));
        }

        /// <summary>
        /// Determines whether a player is in check after a move
        /// </summary>
        /// <param name="board">The board being checked</param>
        /// <param name="player">The player to check</param>
        /// <param name="move">The move being played</param>
        /// <returns>Whether the specified player is in check</returns>
        public static bool PlayerIsCheckedAfterMove(ChessBoard board, PieceColour player, IMove move)
        {
            var newBoard = ChessBoard.SetMove(board, move);
            if (ChessBoard.PlayerIsChecked(newBoard, player))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all moves for a player on a board
        /// </summary>
        /// <param name="board">The board being checked</param>
        /// <param name="player">The player to check</param>
        /// <returns>An enumerable of all the moves the player can play</returns>
        public IEnumerable<IMove> AllMovesForPlayer(PieceColour player)
        {
            IEnumerable<IMove> result = Enumerable.Empty<IMove>();
            foreach (var piece in Board.Board)
            {
                if (piece is null)
                    continue;

                result = result.Union(GetValidMovesForPiece(piece));
            }
            return result;
        }

        /// <summary>
        /// Plays a move
        /// </summary>
        /// <param name="move">Move to be played</param>
        /// <exception cref="MoveInvalidException">The passed move was invalid</exception>
        public void PlayMove(IMove move)
        {
            if (!MoveIsValid(Board, PlayerToMove, move))
            {
                throw new MoveInvalidException("Move invalid");
            }

            move.DoMove(Board);
            Moves++;
            Board.TickEnPassantSquares();
        }
    }
}
