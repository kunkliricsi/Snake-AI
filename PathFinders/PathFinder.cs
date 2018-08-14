using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Kukac.ai;
using Kukac.interfaces;
using Kukac.kukac;

namespace Kukac.ai.PathFinder
{
    /// <summary>
    /// Creates a template for other PathFinders. Has basic tools to determine path between two Points.
    /// Used for other classes to inherit from.
    /// </summary>
    public abstract class PathFinder : Handlers.BasicHandler
    {
        protected Point head, food;

        protected Stack<Point> defaultPath = new Stack<Point>();
        public Stack<Point> DefaultPath => defaultPath;

        #region Constructors
        protected PathFinder(Adat data, Test snake)
            : base(data, snake) { }

        protected PathFinder() { }
        #endregion

        /// <summary>
        /// Returns true if previous path was valid. Why am I even commenting this?
        /// </summary>
        protected bool IsPreviousPathValid
        {
            get
            {
                if (defaultPath.Count > 1
                    && utilities.IsPathEmpty(utilities.GetPathWithoutSnake(defaultPath, this.snake))
                    && !utilities.IsNearTail(this.snake)
                    ) return true;

                return false;
            }
        }

        /// <summary>
        /// Updates <paramref name="head"/> and <paramref name="food"/> values. 
        /// Whick provides the start and the end of the path to look for.
        /// </summary>
        /// <param name="head"></param>
        /// <param name="food"></param>
        /// <returns></returns>
        public bool UpdateHeadAndFood(Point? head = null, Point? food = null)
        {
            if (head == null)
            {
                if (this.snake.getHossz() == 0)
                    return false;

                head = this.snake.getFej();
            }

            if (food == null)
                food = this.data.getEtel();

            this.head = (Point)head;
            this.food = (Point)food;

            return true;
        }

        public void ResetDefaultPath()
        {
            defaultPath.Clear();
        }

        /// <summary>
        /// Starts the path finding algorithm
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>
        /// True if it found an optimal path. False otherwise.
        /// </returns>
        public abstract bool SolvePath(Point? from = null, Point? to = null);
    }
}
