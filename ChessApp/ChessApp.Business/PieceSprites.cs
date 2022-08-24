using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    /// <summary>
    /// Static class that determines what sprites are used in the program
    /// </summary>
    public static class Sprites
    {
        public static string PieceSpriteName => spriteNames[1];
        public static (string BoardColourOne, string BoardColourTwo) BoardTheme => boardThemes["brown"];

        public static readonly Dictionary<string, (string BoardColourOne, string BoardColourTwo)> boardThemes = new()
        {
            { "pink", new("#f07373", "#ebecba") },
            { "brown", new("#b56f3c", "#dfbe91") },
            { "green", new("#769656", "#eeeed2") }
        };

        public static readonly List<string> spriteNames = new()
        {
            "anarcandy",
            "alpha",
        };
    }
}
