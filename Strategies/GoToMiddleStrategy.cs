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
    /// Strategy that tries to move to the middle of the map.
    /// Useful because its more likely to reach spawned food faster from the middle,
    /// than from anywhere else.
    /// </summary>
    class GoToMiddleStrategy : Strategy
    {
        private PathFinder.ShortestPathFinder shortestPathFinder;
        private Point closestPointToMiddle;

        #region Constructors
        public GoToMiddleStrategy(Adat data, Test snake)
            : base(data, snake)
        {
            shortestPathFinder = new PathFinder.ShortestPathFinder(data, snake);
            SetMiddlePoint();
        }

        public GoToMiddleStrategy() { }

        public override void Config(Adat data, Test snake)
        {
            base.Config(data, snake);
            shortestPathFinder = new PathFinder.ShortestPathFinder(data, snake);
            SetMiddlePoint();
        }
        #endregion

        /// <summary>
        /// Sets <see cref name="closestPointToMiddle"/> to the middle of the map.
        /// </summary>
        private void SetMiddlePoint()
        {
            Size map = data.getPalyaMeret();
            closestPointToMiddle = new Point((int)(map.Width / 2), (int)(map.Height / 2));
        }

        /// <summary>
        /// Check whether the snake is near the calculated closests point
        /// </summary>
        /// <returns>
        /// True if the snake is near, false otherwise.
        /// </returns>
        private bool IsSnakeNearMiddlePoint()
        {
            Point middleOfSnake = (Point)this.snake.getTestResz((int)this.snake.getHossz()/2);
            int deltaX = (int)Math.Abs(middleOfSnake.X - closestPointToMiddle.X);
            int deltaY = (int)Math.Abs(middleOfSnake.Y - closestPointToMiddle.Y);

            if (deltaX < this.snake.getHossz() / 4 && deltaY < this.snake.getHossz() / 4)
                return true;

            return false;
        }

        /// <summary>
        /// Sets <see cref name="closestPointToMiddle"/> to the 
        /// closest available point it can safely get to the middle.
        /// </summary>
        /// <returns>
        /// True if it found the point it was looking for.
        /// </returns>
        private bool IsClosestMiddlePointAvailable()
        {
            SetMiddlePoint();

            int direction = -1;
            for (int outerLevel = 1; outerLevel < data.getPalyaMeret().Width/8; outerLevel++)
            {
                for (int y = -1; y <= 1; y += 2)
                    for (int i = 0; i < outerLevel; i++)
                    {
                        closestPointToMiddle.Offset((y == -1 ? direction : 0),
                                                        (y == 1 ? direction : 0));

                        if (utilities.IsPointNotElement(closestPointToMiddle, PalyaElemek.TEST)
                            && shortestPathFinder.SolvePath(null, closestPointToMiddle)
                            ) return true;
                    }

                direction *= -1;
            }

            return false;
        }

        /// <summary>
        /// Tries to move the snake to the closest point it can get to the middle if it's not already there.
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            if (!base.Run())
                return false;

            if (IsSnakeNearMiddlePoint())
                return false;
            
            if (IsClosestMiddlePointAvailable())
            {
                    if (SetFinalDirection(utilities.DetermineDirection(shortestPathFinder.DefaultPath, this.snake.getFej())))
                        return true;
            }

            return false;
        }
    }
}
