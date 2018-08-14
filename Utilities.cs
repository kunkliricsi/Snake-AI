using Kukac.enums;
using Kukac.interfaces;
using Kukac.kukac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Kukac.ai
{
    /// <summary>
    /// Provides Basic Tools for both PathFinders and Strategies. 
    /// Also holds some constant values for them.
    /// </summary>
    /// <remarks> 
    /// Can convert Pathes Vectors and Directions to each other
    /// </remarks>
    public class Utilities : Handlers.DataHandler
    {
        #region Constants
        public const int NEAR_TAIL_THRESHOLD = 2;
        public const int RESET_COUNTER_THRESHOLD = 25;
        #endregion

        #region Constructors
        public Utilities(Adat data)
            : base(data) { }

        public Utilities() { }

        public override void Config(Adat data)
        {
            base.Config(data);
        }
        #endregion

        /// <summary>
        /// Makes a dictionary from another one by returning the elements with the keys the <paramref name="set"/> contains.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="set"></param>
        /// <returns>
        /// The trimmed dictionary
        /// </returns>
        public Dictionary<K, V> TrimDictionaryBySet<K, V>(Dictionary<K, V> dict, ISet<K> set)
        {
            Dictionary<K, V> trimmedDictionary = new Dictionary<K, V>();
            foreach (K key in set)
                trimmedDictionary.Add(key, dict[key]);

            return trimmedDictionary;
        }

        #region Path/Vector/Direction converter utilities
        public Iranyok ConvertVectorToDirection(Vector directionVector)
        {
            if (directionVector.X == -1)
                return Iranyok.BAL;
            else if (directionVector.X == 1)
                return Iranyok.JOBB;
            else if (directionVector.Y == -1)
                return Iranyok.FEL;
            else if (directionVector.Y == 1)
                return Iranyok.LE;

            return null;
        }

        public Vector ConvertDirectionToVector(Iranyok direction)
        {
            Vector toReturn = new Vector();

            toReturn.X += direction.getXm();
            toReturn.Y += direction.getYm();

            return toReturn;
        }

        public Vector GetDirectionInverse(Iranyok direction, bool flipSides = false)
        {
            int flipper = (flipSides ? -1 : 1);

            Vector directionInverse = new Vector(
                direction.getYm() * flipper,
                direction.getXm() * flipper);

            return directionInverse;
        }
        #endregion

        /// <summary>
        /// Finds the <paramref name="head"/> in <paramref name="shortestPath"/>. Gets the direction the <paramref name="head"/> is in.
        /// </summary>
        /// <param name="shortestPath"></param>
        /// <param name="head"></param>
        /// <returns>
        /// The direction.
        /// </returns>
        public Iranyok DetermineDirection(Stack<Point> shortestPath, Point head)
        {
            if (!shortestPath.Contains(head))
                return null;
            else if (shortestPath.Count <= 1)
                return null;

            while (head != shortestPath.Pop()) ;

            Vector directionVector = shortestPath.Peek() - head;

            return ConvertVectorToDirection(directionVector);
        }

        /// <summary>
        /// Gets a Point in the direction of <paramref name="from"/> - <paramref name="by"/>.
        /// <paramref name="byHowMuch"/> sets how far should the Point be from <paramref name="from"/>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="by"></param>
        /// <param name="byHowMuch"></param>
        /// <returns></returns>
        public Point GetContinuedPointOf(Point from, Point by, int byHowMuch)
        {
            Vector direction = from - by;

            return from + direction * byHowMuch;
        }

        /// <summary>
        /// Give it a <paramref name="path"/> with a <paramref name="snake"/> and it removes all <paramref name="snake"/> parts from the path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="snake"></param>
        /// <returns>
        /// The path which doesn't contain the snake.
        /// </returns>
        public HashSet<Point> GetPathWithoutSnake(Stack<Point> path, Test snake)
        {
            HashSet<Point> pathWithoutSnake = new HashSet<Point>(path);

            for (int i = 0; i < snake.getHossz(); i++)
                pathWithoutSnake.Remove((Point)snake.getTestResz(i));

            return pathWithoutSnake;
        }

        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The estimated distance between the two parameters.
        /// </returns>
        public int EstimateCostBetween(Point a, Point b)
        {
            return (int)(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
        }

        /// <summary>
        /// Build a path from <paramref name="current"/> to <paramref name="end"/> by the minimum distance,
        /// which <paramref name="cameFrom"/> provides.
        /// </summary>
        /// <param name="cameFrom"></param>
        /// <param name="current"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Stack<Point> BuildPathFromCurrentToEnd(Dictionary<Point, Point?> cameFrom, Point current, Point end)
        {
            Stack<Point> stackToReturn = new Stack<Point>();
            stackToReturn.Push(current);

            Point node = current;
            while (!node.Equals(end))
            {
                node = (Point)cameFrom[node];
                stackToReturn.Push(node);
            }

            return stackToReturn;
        }

        /// <summary>
        /// I don't know if I should explain these functions or not. I mean they are pretty self explanatory, right?
        /// </summary>
        /// <param name="point"></param>
        /// <param name="elements"></param>
        /// <returns>
        /// If given <paramref name="point"/> does not equal any given <paramref name="elements"/>
        /// </returns>
        public bool IsPointNotElement(Point point, params PalyaElemek[] elements)
        {
            foreach (var element in elements)
            {
                if (data.getPalyaElem((int)point.X, (int)point.Y) == element)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if given <paramref name="path"/> only contains Empty or Food MapElements.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsPathEmpty(HashSet<Point> path)
        {
            foreach (Point node in path)
            {
                PalyaElemek block = data.getPalyaElem((int)node.X, (int)node.Y);
                if (block == PalyaElemek.FAL || block == PalyaElemek.TEST)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the given <paramref name="snake"/> has any snake tails close to it.
        /// Closeness is determined by a class constant <c>NEAR_TAIL_THRESHOLD</c>
        /// </summary>
        /// <param name="snake"></param>
        /// <returns></returns>
        public bool IsNearTail(Test snake)
        {
            if (snake.getHossz() <= NEAR_TAIL_THRESHOLD)
                return false;

            List<Point> tails = new List<Point>();
            foreach (var snakeToCheck in data.getKukacok())
            {
                if (snakeToCheck.getHossz() > 0
                    && EstimateCostBetween(snake.getFej(), snakeToCheck.getVege()) <= NEAR_TAIL_THRESHOLD
                    ) return true;
            }

            return false;
        }
    }
}
