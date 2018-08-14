using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Kukac.enums;
using Kukac.interfaces;

namespace Kukac.ai.PathFinder
{
    /// <summary>
    /// Provides tools for finding the absolute shortest path between two points.
    /// </summary>
    public class ShortestPathFinder : PathFinder
    {
        #region Arrays storing discoverable and already discovered nodes
        protected HashSet<Point> blockedNodes = new HashSet<Point>();
        protected HashSet<Point> nodesToDiscover = new HashSet<Point>();
        #endregion

        #region Algorithm Node Attributes
        protected Dictionary<Point, int> gScore = new Dictionary<Point, int>();
        protected Dictionary<Point, int> fScore = new Dictionary<Point, int>();
        protected Dictionary<Point, Point?> cameFrom = new Dictionary<Point, Point?>();
        #endregion

        #region Constructors
        public ShortestPathFinder() { }

        public ShortestPathFinder(Adat data, Test snake)
            : base(data, snake)
        {
            this.resetCounter = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Used for resetting the path finder, so that it will calculate everything again.
        /// </summary>
        protected int resetCounter;
        private bool IsAtResetMax
        {
            get
            {
                if (data.getKukacDarab() == 1)
                    return false;

                if (resetCounter == Utilities.RESET_COUNTER_THRESHOLD)
                {
                    resetCounter = 0;
                    return true;
                }
                else
                    resetCounter++;

                return false;
            }
        }
        #endregion

        /// <summary>
        /// Finds node with the lowest Fscore.
        /// </summary>
        /// <returns>
        /// The Point with the lowest Fscore.
        /// </returns>
        private Point GetLowestFScore()
        {
            Dictionary<Point, int> discoverableOnlyFScore = 
                utilities.TrimDictionaryBySet(fScore, nodesToDiscover);

            KeyValuePair<Point, int> lowest = discoverableOnlyFScore.First();
            foreach (KeyValuePair<Point, int> score in discoverableOnlyFScore)
            {
                if (score.Value < lowest.Value)
                    lowest = score;
            }

            return lowest.Key;
        }
        
        #region Initializers
        private void ResetArrays()
        {
            blockedNodes.Clear();
            gScore.Clear();
            fScore.Clear();
            cameFrom.Clear();
            nodesToDiscover.Clear();
        }

        private void InitializeArrays()
        {
            ResetArrays();

            for (int x = 0; x <= data.getPalyaMeret().Width; x++)
                for (int y = 0; y <= data.getPalyaMeret().Height; y++)
                {
                    Point toAdd = new Point(x, y);
                    if (data.getPalyaElem(x, y) == PalyaElemek.FAL || 
                        data.getPalyaElem(x, y) == PalyaElemek.TEST)
                    {
                        blockedNodes.Add(toAdd);
                    }
                    else
                    {
                        gScore.Add(toAdd, int.MaxValue);
                        fScore.Add(toAdd, int.MaxValue);
                        cameFrom.Add(toAdd, null);
                    }
                }

            gScore[head] = 0;
            fScore[head] = utilities.EstimateCostBetween(head, food);
        }
        #endregion

        #region The A* algorithm
        /// <summary>
        /// Gets <see cref name="cameFrom"/> <see cref name="fScore"/> <see cref name="gScore"/> 
        /// for each <paramref name="neighbor"/> of <paramref name="current"/>.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="neighbor"></param>
        private void UpdateNeighborData(Point current, Point neighbor)
        {
            if (gScore[current] + 1 < gScore[neighbor])
            {
                cameFrom[neighbor] = current;
                gScore[neighbor] = gScore[current] + 1;
                fScore[neighbor] = gScore[neighbor] + utilities.EstimateCostBetween(neighbor, food);
            }
        }

        /// <summary>
        /// Adds nodes to the discoverable list if they are not blocked.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="neighbor"></param>
        private void AddNeighborIfAvailable(Point current, Point neighbor)
        {
            if (blockedNodes.Contains(neighbor))
                return;

            if (!nodesToDiscover.Contains(neighbor))
                nodesToDiscover.Add(neighbor);

            UpdateNeighborData(current, neighbor);
        }

        /// <summary>
        /// Checks nodes around <paramref name="current"/> point. Discovers their neighbors if they are not blocked.
        /// </summary>
        /// <param name="current"></param>
        private void DiscoverNeighborsOf(Point current)
        {
            bool isX = true;

            for (int flip = 0; flip <= 1; flip++)
            {
                for (int move = -1; move <=1; move += 2)
                {
                    Point neighbor = new Point(current.X + (isX ? move : 0),
                                                current.Y + (isX ? 0 : move));

                    AddNeighborIfAvailable(current, neighbor);
                }

                isX = !isX;
            }
        }

        /// <summary>
        /// Adds or Removes Nodes from both the Discoverable and the Blocked lists.
        /// </summary>
        /// <param name="nodeToSwap"></param>
        /// <param name="isDiscoverable"></param>
        private void SetNodeDiscoverability(Point nodeToSwap, bool isDiscoverable)
        {
            if (isDiscoverable)
            {
                nodesToDiscover.Add(nodeToSwap);
                blockedNodes.Remove(nodeToSwap);
            }
            else
            {
                nodesToDiscover.Remove(nodeToSwap);
                blockedNodes.Add(nodeToSwap);
            }
        }

        /// <summary>
        /// Implements the A* algorithm, which is used for finding the shortest path
        /// between two points.
        /// </summary>
        /// <param name="shortestPath"></param>
        /// <returns>
        /// True if it found an optimal shortest path, false otherwise.
        /// </returns>
        private bool AstarAlgorithm(ref Stack<Point> shortestPath)
        {
            while (nodesToDiscover.Count > 0)
            {
                Point current = GetLowestFScore();

                if (current.Equals(food))
                {
                    shortestPath = new Stack<Point>(
                        utilities.BuildPathFromCurrentToEnd(cameFrom, current, this.head)
                        .Reverse());

                    return true;
                }

                SetNodeDiscoverability(current, false);

                DiscoverNeighborsOf(current);
            }

            return false;
        }
        #endregion
        
        public override bool SolvePath(Point? from = null, Point? to = null)
        {
            if (!UpdateHeadAndFood(from, to))
                return false;
            
            if (!IsPreviousPathValid || IsAtResetMax)
            {
                InitializeArrays();
                defaultPath.Clear();

                SetNodeDiscoverability(head, true);

                return AstarAlgorithm(ref defaultPath);
            }

            return true;
        }
    }
}
