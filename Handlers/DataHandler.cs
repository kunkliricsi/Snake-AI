using Kukac.interfaces;
using Kukac.kukac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kukac.ai.Handlers
{
    /// <summary>
    /// Provides a basid Dataholder. Used for other classes to inherit from.
    /// </summary>
    public abstract class DataHandler
    {
        protected Data data;

        #region Constructors
        public DataHandler(Adat data)
        {
            this.data = (Data)data;
        }

        public DataHandler() { }

        public virtual void Config(Adat data)
        {
            this.data = (Data)data;
        }
        #endregion
    }
}
