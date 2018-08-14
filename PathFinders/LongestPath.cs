using Kukac.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Kukac.ai.PathFinder
{
    /// <summary>
    /// Finds longest path between two points. Is not yet implemented. DO NOT USE.
    /// </summary>
    public class LongestPathFinder : PathFinder
    {
        public LongestPathFinder(Adat data, Test snake)
            : base(data, snake) { }

        public override bool SolvePath(Point? from = null, Point? to = null)
        {
            throw new NotImplementedException();
        }
    }
}
