using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class LegendreApproximatorInfo: ApproximatorInfo
    {
        private int polynomialOrder = 0;
        private double approximationAccuracy = Double.NaN;
        public LegendreApproximatorInfo(LeApproximationAbstractLogicAPI logicAPI) : base(logicAPI) { }
        public LegendreApproximatorInfo(LeApproximationAbstractLogicAPI logicAPI, int polynomialOrder) : base(logicAPI) {
            this.polynomialOrder = polynomialOrder;
        }
        public LegendreApproximatorInfo(LeApproximationAbstractLogicAPI logicAPI, double approximationAccuracy) : base(logicAPI) {
            this.approximationAccuracy = approximationAccuracy;
        }

        public int PolynomialOrder => this.polynomialOrder;
        public double ApproximationAccuracy => this.approximationAccuracy;

    }
}
