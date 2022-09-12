using ChessApp.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Tests
{
    public class PlyChecks
    {
        [Theory]
        [InlineData(1, 20)]
        [InlineData(2, 400)]
        [InlineData(3, 8_902)]
        [InlineData(4, 197_281)]
        [InlineData(5, 4_865_609)]
        [InlineData(6, 119_060_324)]
        public void TestDepth(int depth, int expectedNumberOfBoards)
        {
            var game = new Game(new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8));
            Assert.Equal(expectedNumberOfBoards, PossibleStatesAfterDepth(depth, game, depth));
        }

        [Theory]
        [InlineData(1, 44)]
        [InlineData(2, 1_486)]
        [InlineData(3, 62_379)]
        [InlineData(4, 2_103_487)]
        public void TestDepth2(int depth, int expectedNumberOfBoards)
        {
            var game = new Game(new("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R", 8, 8));
            game.Board[new Tile(0, 5)]!.Moves = 1;
            game.Board[new Tile(2, 2)]!.Moves = 1;
            Assert.Equal(expectedNumberOfBoards, PossibleStatesAfterDepth(depth, game, depth));
        }

        public static IEnumerable<Game> PossibleStates(Game currentState)
        {
            var result = new List<Game>();
            foreach (IMove move in currentState.AllMovesForPlayer(currentState.PlayerToMove))
            {
                result.Add(new(ChessBoard.SetMove(currentState.Board, move)) { Moves = currentState.Moves + 1 });
            }
            return result;
        }

        public static int PossibleStatesAfterDepth(int depth, Game currentState, int startDepth)
        {
            if (depth == 0)
            {
                return 1;
            }

            var result = 0;
            foreach (IMove move in currentState.AllMovesForPlayer(currentState.PlayerToMove))
            {
                var state = new Game(ChessBoard.SetMove(currentState.Board, move)) { Moves = currentState.Moves + 1 };
                result += PossibleStatesAfterDepth(depth - 1, state, startDepth);
            }
            return result;
        }
    }
}
