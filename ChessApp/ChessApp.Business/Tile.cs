using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public record Tile(int Row, int Column)
    {
        public bool IsOnBoard(ChessBoard board)
        {
            return 0 <= Row && Row <= board.Rows - 1 &&
                   0 <= Column && Column <= board.Columns - 1;
        }

        public static Tile Move(Tile tile, int rows, int columns)
        {
            return new Tile(tile.Row + rows, tile.Column + columns);
        }
    }
}
