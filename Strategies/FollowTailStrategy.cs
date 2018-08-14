using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Kukac.enums;
using Kukac.interfaces;

namespace Kukac.ai.Strategies
{
    /// <summary>
    /// Strategy that makes the snake try to follow its own tail like how a dog would play with its.
    /// </summary>
    public class FollowTailStrategy : Strategy
    {
        PathFinder.ShortestPathFinder shortestPathFinder;

        #region Constructors
        public FollowTailStrategy(Adat data, Test snake)
            : base(data, snake)
        {
            shortestPathFinder = new PathFinder.ShortestPathFinder(data, snake);
        }

        public FollowTailStrategy() { }

        public override void Config(Adat data, Test snake)
        {
            base.Config(data, snake);
            shortestPathFinder = new PathFinder.ShortestPathFinder(data, snake);
        }
        #endregion

        /// <summary>
        /// Gets the point of the snake's tail and then shifts it in the direction,
        /// the tail was set in.
        /// </summary>
        /// <returns>
        /// The point you are looking for.
        /// </returns>
        private Point GetLastPartOfSnakePlusOne()
        {
            Point lastPart = this.snake.getVege();
            Point? lastPartMinusOne = this.snake.getTestResz(this.snake.getHossz() - 2);

            return utilities.GetContinuedPointOf(lastPart, (Point)lastPartMinusOne, 15);
        }

        /// <summary>
        /// Finds the last known position of snake's tail, and then tries to move it in that direction.
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            if (!base.Run())
                return false;

            shortestPathFinder.ResetDefaultPath();

            if (this.snake.getHossz() >= 2)
            {
                Point lastPartOfSnakePlusOne = GetLastPartOfSnakePlusOne();

                if (shortestPathFinder.SolvePath(this.snake.getFej(), lastPartOfSnakePlusOne))
                    if (SetFinalDirection(utilities.DetermineDirection(shortestPathFinder.DefaultPath, this.snake.getFej())))
                        return true;
            }

            return false;
        }
    }
}
