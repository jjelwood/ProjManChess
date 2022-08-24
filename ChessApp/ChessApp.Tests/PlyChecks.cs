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

        }

        public IEnumerable<Game> PossibleStates(Game currentState)
        {
            var result = new List<Game>();
            foreach (IMove move in Game.AllMovesForPlayer(currentState.Board, currentState.PlayerToMove))
            {
                result.Add(new(ChessBoard.SetMove(currentState.Board, move)) { Moves = currentState.Moves + 1 });
            }
            return result;
        }

        public int PossibleStatesAfterDepth()
        {
            return 0;
        }
    }
}
