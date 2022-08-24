using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public enum PieceColour
    {
        Black = 0,
        White = 1
    }

    public static class PieceColourExtensions
    {
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
