/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class LeApproximationAbstractLogicAPI
    {
    }
}*/

namespace LeApproximationLogic
{
    public abstract class LeApproximationAbstractLogicAPI
    {
        protected readonly LeApproximationData.LeApproximationAbstractDataAPI dataAPI;

        public LeApproximationAbstractLogicAPI(LeApproximationData.LeApproximationAbstractDataAPI? dataAPI = null)
        {
            this.dataAPI = dataAPI ?? LeApproximationData.LeApproximationAbstractDataAPI.CreateInstance();
        }

        public static LeApproximationAbstractLogicAPI CreateInstance(LeApproximationData.LeApproximationAbstractDataAPI? dataAPI = null)
        {
            return new LeApproximationLogicAPI(dataAPI ?? LeApproximationData.QuadIntegraAbstractDataAPI.CreateInstance());
        }

        public event EventHandler<LeApproximationData.ComputationDumpEventArgs>? ComputationDump;
        protected void OnComputationDump(object? sender, LeApproximationData.ComputationDumpEventArgs computationDumpEventArgs)
        {
            this.ComputationDump?.Invoke(sender, computationDumpEventArgs);
        }

        public abstract double Integrate<TIntegrator>(int functionIndex, double integrationLeftBound,
                                                      double integrationRightBound, double desiredAccuracy, IntegratorInfo? integratorInfo = null) where TIntegrator : Integrator;

        public (double, double)[] GetNodes(int quadratureNodesNumber, double integrationLeftBound, double integrationRightBound) => this.dataAPI.GetGLQuadratureData(quadratureNodesNumber, integrationLeftBound, integrationRightBound);
        public Func<double, double> Function(int index) => this.dataAPI.GetFunction(index);
        public LeApproximationData.QuadIntegraAbstractDataAPI DataAPI => this.dataAPI;
    }
}
