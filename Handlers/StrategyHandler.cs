using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Kukac.enums;
using Kukac.interfaces;
using Kukac.kukac;

namespace Kukac.ai.Handlers
{
    using Strategies;

    /// <summary>
    /// The God object. It's purpose is to handle all end every strategy it gets.
    /// Also functions as a strategy that can be used in other strategies and strategy handlers.
    /// It's decision is equal to the decision of the strategy with the highest priority that had
    /// a valid outcome.
    /// </summary>
    class StrategyHandler : Strategy, Ai
    {
        private readonly Dictionary<Strategy, int> strategyPriorities =
            new Dictionary<Strategy, int>();

        #region Constructors
        public StrategyHandler(
            Adat data,
            Test snake,
            IDictionary<Strategy, int> strategiesWithPriorities)
            : base(data, snake)
        {
            this.strategyPriorities = new Dictionary<Strategy, int>(strategiesWithPriorities);

            foreach (var strategy in strategyPriorities.Keys)
                strategy.Config(data, snake);

            this.finalDirection = Iranyok.LE;
        }

        public void initAdat(Adat adat)
        {
            this.data = (Data)adat;
        }

        public void initKukac(Test kukac)
        {
            this.snake = kukac;
        }
        #endregion

        /// <summary>
        /// Orders strategies by priority.
        /// </summary>
        /// <returns>
        /// A list of the ordered strategies.
        /// </returns>
        private List<Strategy> GetOrderedStrategiesByPriority()
        {
            List<KeyValuePair<Strategy, int>> orderedStrategiesByPriorityPairs =
                strategyPriorities.OrderBy(x => x.Value).ToList();

            List<Strategy> orderedStrategiesByPriorityStrategies = new List<Strategy>();

            foreach (KeyValuePair<Strategy, int> stratPair in orderedStrategiesByPriorityPairs)
                orderedStrategiesByPriorityStrategies.Add(stratPair.Key);

            return orderedStrategiesByPriorityStrategies;
        }

        /// <summary>
        /// Runs every strategy it holds, by the highest priority.
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            List<Strategy> orderedStrategiesByPriority = GetOrderedStrategiesByPriority();

            foreach (Strategy strategy in orderedStrategiesByPriority)
            {
                if (strategy.Run())
                {
                    this.finalDirection = strategy.FinalDirection;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Runs the handler. Returns the evaluated direction it thinks the snake should move in.
        /// </summary>
        /// <returns></returns>
        public Iranyok setIrany()
        {
            Run();
            return finalDirection;
        }
    }
}
