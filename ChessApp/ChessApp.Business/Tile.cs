using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    /// <summary>
    /// Record of a tile on a chessboard
    /// </summary>
    /// <param name="Row">The tile's row</param>
    /// <param name="Column">The tile's column</param>
    public record Tile(int Row, int Column)
    {
        /// <summary>
        /// Checks if the tile is on a passed board
        /// </summary>
        /// <param name="board">Board to check</param>
        /// <returns>Whether the tile is on the board</returns>
        public bool IsOnBoard(ChessBoard board)
        {
            return 0 <= Row && Row <= board.Rows - 1 &&
                   0 <= Column && Column <= board.Columns - 1;
        }

        /// <summary>
        /// Gets a tile a number of rows and columns over from a given tile
        /// </summary>
        /// <param name="tile">Tile to move from</param>
        /// <param name="rows">Number of rows to move</param>
        /// <param name="columns">Number of columns to move</param>
        /// <returns>A new tile moved the specified amount from the given tile</returns>
        public static Tile Move(Tile tile, int rows, int columns)
        {
            return new Tile(tile.Row + rows, tile.Column + columns);
        }
    }
}
