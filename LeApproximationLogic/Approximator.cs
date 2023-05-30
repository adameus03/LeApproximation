using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal abstract class Approximator<TIntegrator> where TIntegrator : Integrator
    {

        protected Func<double, double> f;
        protected double a;
        protected double b;
        protected TerminationCondition terminationCondition;
        protected ApproximatorInfo info;
        protected double integrationEpsilon;
        protected IntegratorInfo? integratorInfo;
        protected double lastAccuracyAchieved = Double.NaN;

        public Approximator(Func<double, double> f, double a, double b, TerminationCondition terminationCondition, ApproximatorInfo approximatorInfo, double integrationEpsilon, IntegratorInfo? integratorInfo = null) {
            this.f = f;
            this.a = a;
            this.b = b;
            this.terminationCondition = terminationCondition;
            this.info = approximatorInfo;
            this.integrationEpsilon = integrationEpsilon;
            this.integratorInfo = integratorInfo;
        }

        public static Approximator<TIntegrator> CreateInstance(Func<double, double> f, double a, double b, TerminationCondition terminationCondition, ApproximatorInfo approximatorInfo, double integrationEpsilon, IntegratorInfo? integratorInfo = null)
        {
            return new LegendreApproximator<TIntegrator>(f, a, b, terminationCondition, (LegendreApproximatorInfo)approximatorInfo, integrationEpsilon, integratorInfo);
        }

        public enum TerminationCondition { ByOrder, ByAccuracy }
        public abstract Func<double, double> Approximate();

        public event EventHandler<LeApproximationData.ComputationDumpEventArgs>? ComputationDump;
        protected void OnComputationDump(string line)
        {
            this.ComputationDump?.Invoke(this, new LeApproximationData.ComputationDumpEventArgs(line));
        }

        public double LastAccuracyAchieved => this.lastAccuracyAchieved;


    }
}
