/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class SimpsonIntegrator
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
    public class SimpsonIntegrator : Integrator
    {
        private SimpsonIntegrator() : base((x) => 0, 0, 0, 0) { }
        public SimpsonIntegrator(Func<double, double> f, double a, double b, double epsilon) : base(f, a, b, epsilon) { }
        protected override double Kernel(double a, double b)
        {
            double f0 = base.f(a);
            double f1 = base.f(0.5 * (a + b));
            double f2 = base.f(b);
            double h = 0.5 * (b - a);

            return (f0 + 4 * f1 + f2) * h / 3.0;
        }
    }
}

