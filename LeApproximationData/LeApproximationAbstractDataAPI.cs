/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationData
{
    internal class LeApproximationAbstractDataAPI
    {
    }
}*/

namespace LeApproximationData
{
    public abstract class LeApproximationAbstractDataAPI
    {
        public LeApproximationAbstractDataAPI() { }
        public static LeApproximationAbstractDataAPI CreateInstance()
        {
            return new LeApproximationDataAPI();
        }

        public abstract Func<double, double> GetFunction(int index);
        public abstract (double, double)[] GetGLQuadratureData(int quadratureNodesNumber);
    }
}
