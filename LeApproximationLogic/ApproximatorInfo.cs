using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal abstract class ApproximatorInfo
    {
        protected LeApproximationAbstractLogicAPI logicAPI;
        public ApproximatorInfo(LeApproximationAbstractLogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
        }
        public LeApproximationAbstractLogicAPI LogicAPI => this.logicAPI;

    }
}
