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

using System;
using LeApproximationData;
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
        private double integrationAccuracy = 0.01;
        private int quadNodesNumber = 3;
        private int functionIndex = 0;
        /*private int polynomialOrder = 1;
        private double approximationAccuracy = 1;*/
        private double terminationConstant = 1;
        //private double integralValue;
        private Func<double, double> approximation = (x)=>Double.NaN;

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

        /*public void CalculateIntegralSimpson()
        {
            this.integralValue = this.logicAPI.Integrate<LeApproximationLogic.SimpsonIntegrator>(functionIndex, leftBound, rightBound, accuracy);
        }

        public void CalculateIntegralGaussLegendre()
        {
            LeApproximationLogic.IntegratorInfo integratorInfo = new LeApproximationLogic.GaussLegendreIntegratorInfo(this.logicAPI, quadNodesNumber);
            this.integralValue = this.logicAPI.Integrate<LeApproximationLogic.GaussLegendreIntegrator>(functionIndex, leftBound, rightBound, accuracy, integratorInfo);
        }*/

        public void CalculateApproximation_Order_Simpson()
        {
            this.approximation = this.logicAPI.Approximate<LeApproximationLogic.SimpsonIntegrator>(functionIndex, leftBound, rightBound, (int)terminationConstant/*polynomialOrder*/, integrationAccuracy);
        }
        public void CalculateApproximation_Order_GaussLegendre()
        {
            LeApproximationLogic.IntegratorInfo integratorInfo = new LeApproximationLogic.GaussLegendreIntegratorInfo(this.logicAPI, quadNodesNumber);
            this.approximation = this.logicAPI.Approximate<LeApproximationLogic.GaussLegendreIntegrator>(functionIndex, leftBound, rightBound, (int)terminationConstant/*polynomialOrder*/, integrationAccuracy, integratorInfo);
        }

        public void CalculateApproximation_Accuracy_Simpson()
        {
            this.approximation = this.logicAPI.Approximate<LeApproximationLogic.SimpsonIntegrator>(functionIndex, leftBound, rightBound, (double)terminationConstant/*approximationAccuracy*/, integrationAccuracy);
        }
        public void CalculateApproximation_Accuracy_GaussLegendre()
        {
            LeApproximationLogic.IntegratorInfo integratorInfo = new LeApproximationLogic.GaussLegendreIntegratorInfo(this.logicAPI, quadNodesNumber);
            this.approximation = this.logicAPI.Approximate<LeApproximationLogic.GaussLegendreIntegrator>(functionIndex, leftBound, rightBound, (double)terminationConstant/*approximationAccuracy*/, integrationAccuracy, integratorInfo);
        }

        public void ClearApproximation()
        {
            this.approximation = (x) => Double.NaN;
        }

        public Func<double, double> FetchFunction()
        {
            return this.logicAPI.Function(this.functionIndex);
        }

        public event EventHandler<ComputationDumpEventArgs>? ComputationMonitorSignalReceived;
        private void OnComputationMonitorSignalReceived(object? sender, ComputationDumpEventArgs computationDumpEventArgs)
        {
            this.ComputationMonitorSignalReceived?.Invoke(sender, computationDumpEventArgs);
        }


        public double LeftBound { get => leftBound; set => leftBound = value; }
        public double RightBound { get => rightBound; set => rightBound = value; }
        public double IntegrationAccuracy { get => integrationAccuracy; set => integrationAccuracy = value; }
        public int QuadNodesNumber { get => quadNodesNumber; set => quadNodesNumber = value; }
        public int FunctionIndex { get => functionIndex; set => functionIndex = value; }

        /*public int PolynomialOrder { get => polynomialOrder; set => polynomialOrder = value; }
        public double ApproximationAccuracy { get => approximationAccuracy; set => approximationAccuracy = value; }*/

        public double TerminationConstant { get => terminationConstant; set => this.terminationConstant = value; }

        //public double IntegralValue { get => integralValue; }

        public Func<double, double> Approximation { get => approximation; }
    }
}

