using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Core
{
    public static class Config
    {

        public static readonly Dictionary<string, (string BoardColourOne, string BoardColourTwo)> boardThemes = new()
        {
            { "pink", new("#ebecba", "#f07373") },
            { "brown", new("#dfbe91", "#b56f3c") },
            { "green", new("#eeeed2", "#769656") }
        };

        public static readonly List<string> spriteNames = new()
        {
            "anarcandy",
            "alpha",
        };

        public static string PieceSpriteName = spriteNames [1];
        public static (string BoardColourOne, string BoardColourTwo) BoardTheme { get; set; } = boardThemes["pink"];
        public static bool AutoPromotion = true;
        public static bool BoardFlipsBetweenMoves = false;
    }
}
