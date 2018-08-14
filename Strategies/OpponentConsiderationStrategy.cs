using Kukac.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kukac.ai.Strategies
{
    /// <summary>
    /// Is like the GreedyStrategy but checks if other snakes would reach the food first.
    /// Only goes for the ones it knows for sure it can get.
    /// </summary>
    class OpponentConsiderationStrategy : Strategy
    {
        protected Dictionary<types.Kukac, int> opponentSnakesEstimateStepsToFood =
            new Dictionary<types.Kukac, int>();

        protected Strategy strategy;

        #region Constructors
        public OpponentConsiderationStrategy(Adat data, Test snake, Strategy strategy)
            : base(data, snake)
        {
            this.strategy = strategy;
            this.strategy.Config(data, snake);
        }

        public OpponentConsiderationStrategy(Strategy strategy)
        {
            this.strategy = strategy;
        }

        public override void Config(Adat data, Test snake)
        {
            base.Config(data, snake);
            this.strategy.Config(data, snake);
        }
        #endregion

        /// <summary>
        /// Adds all enemy snakes to its list
        /// </summary>
        private void AddEnemySnakes()
        {
            opponentSnakesEstimateStepsToFood.Clear();

            foreach (var opponent in data.getKukacok())
            {
                if (opponent.getHossz() != 0)
                    opponentSnakesEstimateStepsToFood.Add(opponent, int.MaxValue);
            }

            opponentSnakesEstimateStepsToFood.Remove((types.Kukac)this.snake);
        }

        /// <summary>
        /// Tries to come up with the answers to life and our existense as humans.
        /// Finds only the shortest pathes for each enemy snake :(
        /// </summary>
        private void EstimateOpponentsShortestPath()
        {
            AddEnemySnakes();
            
            foreach (var opponent in opponentSnakesEstimateStepsToFood.Keys.ToList())
            {
                opponentSnakesEstimateStepsToFood[opponent] =
                    utilities.EstimateCostBetween(opponent.getFej(), data.getEtel());
            }
        }

        /// <summary>
        /// Checks which opponent will reach the food first.
        /// </summary>
        /// <returns>
        /// The steps it takes for the quickest snake to take to the food.
        /// </returns>
        private int GetShortestPathOpponent()
        {
            EstimateOpponentsShortestPath();

            if (opponentSnakesEstimateStepsToFood.Count == 0)
                return int.MaxValue;

            int shortestPath = opponentSnakesEstimateStepsToFood.First().Value;

            foreach (var opponentPath in opponentSnakesEstimateStepsToFood.Values)
            {
                if (opponentPath <= shortestPath)
                    shortestPath = opponentPath;
            }

            return shortestPath;
        }

        /// <summary>
        /// Checks if, by its best calculation, Which snake will reach the food first.
        /// Will it be us or them?
        /// </summary>
        /// <returns>
        /// True if some other snake reaches our food first.
        /// </returns>
        private bool OtherSnakesReachFoodFirst()
        {
            return (utilities.EstimateCostBetween(this.snake.getFej(), this.data.getEtel()) >= GetShortestPathOpponent());
        }

        /// <summary>
        /// Does nothing if other snakes would reach food first.
        /// Else works as a <see cref name="GreedyStrategy"/>.
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            if (!OtherSnakesReachFoodFirst())
                if (strategy.Run())
                {
                    this.finalDirection = strategy.FinalDirection;
                    return true;
                }

            return false;
        }
    }
}
