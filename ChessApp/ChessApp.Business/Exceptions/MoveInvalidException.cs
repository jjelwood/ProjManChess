using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Exceptions
{
    public class MoveInvalidException : Exception
    {
        public MoveInvalidException(string? message) : base(message)
        {
        }
    }
}
