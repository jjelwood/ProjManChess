using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Core
{
    public class ColourPair
    {

        public ColourPair(string colour1, string colour2)
        {
            Colour1 = colour1;
            Colour2 = colour2;
        }

        public string Colour1 { get; }
        public string Colour2 { get; }
    }
}
