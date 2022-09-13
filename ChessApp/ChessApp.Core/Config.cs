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
            { "green", new("#eeeed2", "#769656") },
            { "blue", new("#dee3e6", "#8ca2ad") },
            { "blue2", new("#8398ad", "#5e7489") },
            { "wood", new("#d9a45d", "#9b5925") },
            { "wood2", new("#967c4f", "#786032") },
            { "wood4", new("#c1a06b", "#855a35") },
            { "maple", new("#e1c093", "#b46f3b") },
            { "leather", new("#cbcbc3", "#c08c14") },
            { "green2", new("#ffffdd", "#86a666") },
            { "marble", new("#7d957f", "#627861") },
            { "green-plastic", new("#f1f6b3", "#58945d") },
            { "grey", new("#a8a8a8", "#868686") },
            { "olive", new("#afa897", "#847d6b") },
            { "purple", new("#9f90b0", "#7d4a8d") },
            { "purple2", new("#e2d7ec", "#9a7eb6") },
            { "ic", new("#ececec", "#c1c18e") },
        };

        public static readonly List<string> spriteNames = new()
        {
            "anarcandy",
            "alpha",
            "california",
            "cardinal",
            "cburnett",
            "chess7",
            "chessnut",
            "companion",
            "dubrovny",
            "fantasy",
            "fresca",
            "gioco",
            "governor",
            "horsey",
            "icpieces",
            "kosal",
            "leipzig",
            "letter",
            "libra",
            "maestro",
            "merida",
            "pirouetti",
            "pixel",
            "reillycraig",
            "riohacha",
            "shapes",
            "spatial",
            "staunty",
            "tatiana"
        };

        public static KeyValuePair<string, (string BoardColourOne, string BoardColourTwo)> DefaultTheme = new("maple", new("#e1c093", "#b46f3b"));
        public static string DefaultPieceSet = spriteNames[1];

        public static string PieceSpriteName = DefaultPieceSet;
        public static (string BoardColourOne, string BoardColourTwo) BoardTheme { get; set; } = DefaultTheme.Value;
        public static bool AutoPromotion = true;
        public static bool BoardFlipsBetweenMoves = false;
    }
}
