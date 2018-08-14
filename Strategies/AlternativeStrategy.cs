using Kukac.enums;
using Kukac.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Kukac.ai.Strategies
{
    /// <summary>
    /// The most primitive Strategy anyone could code.
    /// It tries to move the snake in the direction it can move.
    /// </summary>
    public class AlternativeStrategy : Strategy
    {
        #region Constructors
        public AlternativeStrategy() { }

        public AlternativeStrategy(Adat data, Test snake)
            : base(data, snake) { }
        #endregion

        /// <summary>
        /// Goes through every possible direction. Goes with the one the snake can go.
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            if (!base.Run())
                return false;

            Iranyok[] directions = { Iranyok.BAL, Iranyok.LE, Iranyok.FEL, Iranyok.JOBB };
            
            foreach (var direction in directions)
            {
                Vector toMove = utilities.ConvertDirectionToVector(direction);

                Point pointToMove = this.snake.getFej() + toMove;

                if (utilities.IsPointNotElement(pointToMove, PalyaElemek.FAL, PalyaElemek.TEST))
                {
                    SetFinalDirection(direction);
                    return true;
                }
            }

            return false;
        }
    }
}
