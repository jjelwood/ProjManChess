using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public interface IPiece
    {
        /// <summary>
        /// The amount of times this piece has moved
        /// </summary>
        public int Moves { get; set; }

        /// <summary>
        /// The colour of te piece
        /// </summary>
        public PieceColour Colour { get; }

        /// <summary>
        /// The path to the image of the piece
        /// </summary>
        public string ImagePath { get; }

        /// <summary>
        /// The character associated with the piece
        /// </summary>
        public char Character { get; }

        /// <summary>
        /// The position of the piece
        /// </summary>
        public Tile Position { get; set; }

        /// <summary>
        /// Creates a memberwise clone of the piece
        /// </summary>
        /// <returns>A memberwise clone of the piece</returns>
        public IPiece Clone();

        /// <summary>
        /// Gets all of the board tiles the piece is attacking
        /// </summary>
        /// <param name="board">The board the piece is on</param>
        /// <returns>An enumerable of tiles the piece is attacking</returns>
        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board);

        /// <summary>
        /// Gets all of the possible moves for the piece
        /// </summary>
        /// <param name="board">The board the piece is on</param>
        /// <returns>An enumerable of all moves for the piece</returns>
        public IEnumerable<IMove> GetMoves(ChessBoard board);
    }
}
