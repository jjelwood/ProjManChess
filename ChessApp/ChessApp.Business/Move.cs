using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public record Move(Tile To, Tile From)
    {
    }
}
