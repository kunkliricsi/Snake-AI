using Kukac.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kukac.ai.Handlers
{
    /// <summary>
    /// Extends the basic DataHandler with a snake body and a config method.
    /// </summary>
    public abstract class BasicHandler : Handlers.DataHandler
    {
        protected Test snake;

        protected readonly Utilities utilities = new Utilities();

        #region Constructors
        public BasicHandler(Adat data, Test snake)
            : base(data)
        {
            utilities.Config(data);
            this.snake = snake;
        }

        public BasicHandler() { }

        public virtual void Config(Adat data, Test snake)
        {
            base.Config(data);
            utilities.Config(data);
            this.snake = snake;
        }
        #endregion


    }
}
