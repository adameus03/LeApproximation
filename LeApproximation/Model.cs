/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximation
{
    internal class Model
    {
    }
}*/

//using QuadIntegraData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximation
{
    internal class Model
    {
        private double leftBound = -1;
        private double rightBound = 1;
        private double accuracy = 0.01;
        private int quadNodesNumber = 3;
        private int functionIndex = 0;
        private double integralValue;

        LeApproximationLogic.LeApproximationAbstractLogicAPI logicAPI;
        public Model(LeApproximationLogic.LeApproximationAbstractLogicAPI? logicAPI = null)
        {
            this.logicAPI = logicAPI ?? LeApproximationLogic.LeApproximationAbstractLogicAPI.CreateInstance();
            this.logicAPI.ComputationDump += LogicAPI_ComputationDump;
        }

        private void LogicAPI_ComputationDump(object? sender, ComputationDumpEventArgs e)
        {
            OnComputationMonitorSignalReceived(sender, e);
        }

        public void CalculateIntegralSimpson()
        {
            this.integralValue = this.logicAPI.Integrate<LeApproximationLogic.SimpsonIntegrator>(functionIndex, leftBound, rightBound, accuracy);
        }

        public void CalculateIntegralGaussLegendre()
        {
            LeApproximationLogic.IntegratorInfo integratorInfo = new LeApproximationLogic.GaussLegendreIntegratorInfo(this.logicAPI, quadNodesNumber);
            this.integralValue = this.logicAPI.Integrate<LeApproximationLogic.GaussLegendreIntegrator>(functionIndex, leftBound, rightBound, accuracy, integratorInfo);
        }

        public Func<double, double> FetchFunction()
        {
            return this.logicAPI.Function(this.functionIndex);
        }

        /*public (double, double)[] FetchNodesGaussLegendre()
        {
            return this.logicAPI.GetNodes(this.quadNodesNumber, this.leftBound, this.rightBound);
        }*/

        public event EventHandler<ComputationDumpEventArgs>? ComputationMonitorSignalReceived;
        private void OnComputationMonitorSignalReceived(object? sender, ComputationDumpEventArgs computationDumpEventArgs)
        {
            this.ComputationMonitorSignalReceived?.Invoke(sender, computationDumpEventArgs);
        }


        public double LeftBound { get => leftBound; set => leftBound = value; }
        public double RightBound { get => rightBound; set => rightBound = value; }
        public double Accuracy { get => accuracy; set => accuracy = value; }
        public int QuadNodesNumber { get => quadNodesNumber; set => quadNodesNumber = value; }
        public int FunctionIndex { get => functionIndex; set => functionIndex = value; }

        public double IntegralValue { get => integralValue; }
    }
}

