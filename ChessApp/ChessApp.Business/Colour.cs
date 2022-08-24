using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    /// <summary>
    /// Colour of a piece
    /// </summary>
    public enum PieceColour
    {
        Black = 0,
        White = 1
    }

    public static class PieceColourExtensions
    {
        /// <summary>
        /// Gets the opposite colour
        /// </summary>
        /// <param name="colour">Colour to get opposite of</param>
        /// <returns>The opposite of the given colour</returns>
        public static PieceColour Opposite(this PieceColour colour)
        {
            if (colour == PieceColour.White)
            {
                return PieceColour.Black;
            }
            return PieceColour.White;
        }
    }
}
