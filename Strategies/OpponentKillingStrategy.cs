using Kukac.enums;
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
    /// Strategy that tries to kill its opponents. 
    /// It hungers blood and non other but snake blood
    /// </summary>
    public class OpponentKillingStrategy : Strategy
    {
        public OpponentKillingStrategy(Adat data, Test snake)
            : base(data, snake) { }

        public override bool Run()
        {
            throw new NotImplementedException();
        }
    }
}
