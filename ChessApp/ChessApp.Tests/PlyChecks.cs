using ChessApp.Business;
using System;
using System.Collections.Generic;
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
        public void TestDepth(int depth, int expectedNumberOfBoards)
        {
            Assert.Equal(depth, expectedNumberOfBoards);
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

        public static int PossibleStatesAfterDepth()
        {
            return 0;
        }
    }
}
