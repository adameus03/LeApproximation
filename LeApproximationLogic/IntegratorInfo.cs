/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class IntegratorInfo
    {
    }
}*/

using LeApproximationLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    public abstract class IntegratorInfo
    {
        protected LeApproximationAbstractLogicAPI logicAPI;
        public IntegratorInfo(LeApproximationAbstractLogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
        }
        public LeApproximationAbstractLogicAPI LogicAPI => this.logicAPI;
    }
}

