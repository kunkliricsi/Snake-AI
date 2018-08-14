using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Kukac.enums;
using Kukac.interfaces;
using Kukac.kukac;

namespace Kukac.ai.Strategies
{
    /// <summary>
    /// Provides a base for strategies to implement and build on.
    /// Holds the final direction the strategy thinks the snake should go.
    /// </summary>
    public abstract class Strategy : Handlers.BasicHandler
    {
        protected Iranyok finalDirection;

        public Iranyok FinalDirection => finalDirection;

        #region Constructors
        public Strategy(Adat data, Test snake)
            : base(data, snake) { }

        public Strategy() { }

        public override void Config(Adat data, Test snake)
        {
            base.Config(data, snake);
        }
        #endregion

        protected bool SetFinalDirection(Iranyok directionToSet)
        {
            if (directionToSet == null)
                return false;

            this.finalDirection = directionToSet;
            return true;
        }

        /// <summary>
        /// Runs the strategy to determine the final direction.
        /// </summary>
        /// <returns></returns>
        public virtual bool Run()
        {
            if (this.snake.getHossz() == 0)
                return false;

            return true;
        }
    }
}
