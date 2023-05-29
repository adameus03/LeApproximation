/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class GaussLegendreIntegratorInfo
    {
    }
}*/

using LeApproximationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    public class GaussLegendreIntegratorInfo : IntegratorInfo
    {
        private int quadratureNodesNumber;
        public GaussLegendreIntegratorInfo(LeApproximationAbstractLogicAPI logicAPI, int quadratureNodesNumber) : base(logicAPI)
        {
            this.quadratureNodesNumber = quadratureNodesNumber;
        }

        public int QuadratureNodesNumber => this.quadratureNodesNumber;
    }
}

