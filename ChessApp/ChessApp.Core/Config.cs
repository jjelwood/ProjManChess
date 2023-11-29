using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Core
{
    public static class Config
    {

        public static readonly Dictionary<string, ColourPair> boardThemes = new()
        {
            { "Pink", new("#ebecba", "#f07373") },
            { "Green", new("#eeeed2", "#769656") },
            { "Blue", new("#dee3e6", "#8ca2ad") },
            { "Blue 2", new("#8398ad", "#5e7489") },
            { "Wood", new("#d9a45d", "#9b5925") },
            { "Wood 2", new("#967c4f", "#786032") },
            { "Wood 4", new("#c1a06b", "#855a35") },
            { "Maple", new("#e1c093", "#b46f3b") },
            { "Leather", new("#cbcbc3", "#c08c14") },
            { "Green 2", new("#ffffdd", "#86a666") },
            { "Marble", new("#7d957f", "#627861") },
            { "Green Plastic", new("#f1f6b3", "#58945d") },
            { "Grey", new("#a8a8a8", "#868686") },
            { "Olive", new("#afa897", "#847d6b") },
            { "Purple", new("#9f90b0", "#7d4a8d") },
            { "Purple 2", new("#e2d7ec", "#9a7eb6") },
            { "Ic", new("#ececec", "#c1c18e") },
        };

        public static readonly List<string> spriteNames = new()
        {
            "Anarcandy",
            "Alpha",
            "California",
            "Cardinal",
            "Cburnett",
            "Chess7",
            "Chessnut",
            "Companion",
            "Dubrovny",
            "Fantasy",
            "Fresca",
            "Gioco",
            "Governor",
            "Horsey",
            "Icpieces",
            "Kosal",
            "Leipzig",
            "Letter",
            "Libra",
            "Maestro",
            "Merida",
            "Pirouetti",
            "Pixel",
            "Reillycraig",
            "Riohacha",
            "Shapes",
            "Spatial",
            "Staunty",
            "Tatiana"
        };

        public static KeyValuePair<string, ColourPair> DefaultTheme = new("Maple", new("#e1c093", "#b46f3b"));
        public static string DefaultPieceSet = spriteNames[1];

        public static string PieceSpriteName = DefaultPieceSet;
        public static ColourPair BoardTheme { get; set; } = DefaultTheme.Value;
        public static bool AutoPromotion = true;
        public static bool BoardFlipsBetweenMoves = false;
    }
}
