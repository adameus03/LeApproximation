/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class GaussLegendreIntegrator
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
    public class GaussLegendreIntegrator : Integrator
    {
        GaussLegendreIntegratorInfo integratorInfo;
        private GaussLegendreIntegrator(GaussLegendreIntegratorInfo integratorInfo) : base((x) => 0, 0, 0, 0)
        {
            this.integratorInfo = integratorInfo;
        }
        public GaussLegendreIntegrator(Func<double, double> f, double a, double b, double epsilon, GaussLegendreIntegratorInfo integratorInfo) : base(f, a, b, epsilon)
        {
            this.integratorInfo = integratorInfo;
        }
        protected override double Kernel(double a, double b)
        {
            double sum = 0;
            (double, double)[] data = integratorInfo.LogicAPI.DataAPI.GetGLQuadratureData(integratorInfo.QuadratureNodesNumber/*, a, b*/);
            double h = 0.5 * (b - a);
            for (int i = 0; i < integratorInfo.QuadratureNodesNumber; i++)
            {
                double node = a + h * (data[i].Item2 + 1);
                sum += data[i].Item1 * base.f(node);
            }
            return h * sum;
        }
    }
}

