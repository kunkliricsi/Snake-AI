using Kukac.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kukac.ai.Strategies
{
    /// <summary>
    /// Basic strategy that implements the shortestPath PathFinder.
    /// The snake basically tries to get the food on the shortest path possible.
    /// This strategy itself showed a better result than the given "Dumb" AI.
    /// </summary>
    class GreedyStrategy : Strategy
    {
        protected PathFinder.ShortestPathFinder shortestPathFinder =
            new PathFinder.ShortestPathFinder();

        #region Constructors
        public GreedyStrategy(Adat data, Test snake)
            : base(data, snake)
        {
            shortestPathFinder.Config(data, snake);
        }

        public GreedyStrategy() { }

        public override void Config(Adat data, Test snake)
        {
            base.Config(data, snake);
            shortestPathFinder.Config(data, snake);
        }
        #endregion

        /// <summary>
        /// Runs a shortestPath algorithm from the snakes head to the food.
        /// Tries to move the snak in that direction.
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            if (shortestPathFinder.SolvePath())
                if (SetFinalDirection(utilities.DetermineDirection(shortestPathFinder.DefaultPath, this.snake.getFej())))
                    return true;

            return false;
        }
    }
}
