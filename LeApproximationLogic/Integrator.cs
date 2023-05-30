/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class Integrator
    {
    }
}*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    public abstract class Integrator
    {
        protected double a;
        protected double b;
        protected double epsilon;
        protected Func<double, double> f;

        public Integrator(Func<double, double> f, double a, double b, double epsilon)
        {
            this.f = f;
            this.a = a;
            this.b = b;
            this.epsilon = epsilon;
        }
        protected abstract double Kernel(double a, double b);
        public double Integrate()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            //OnComputationDump("++++++++++++++++++++++++++");

            double integral = Kernel(a, b);
            //OnComputationDump($"Subintervals=1; sum={integral}");

            int subintervals = 1;
            int iters = 0;
            double progress;
            do
            {
                iters++;
                subintervals *= 2;
                double sum = 0;
                double spacing = (this.b - this.a) / subintervals;

                for (int i = 0; i < subintervals; i++)
                {
                    double contribution = Kernel(a + i * spacing, a + (i + 1) * spacing);
                    sum += contribution;
                }
                //OnComputationDump($"Subintervals=2^{iters}; sum={sum}");
                progress = Math.Abs(integral - sum);
                integral = sum;
            }
            while (progress > this.epsilon);
            //OnComputationDump("--------------------------");
            watch.Stop();
            //OnComputationDump($"I: {iters}, T: {((double)watch.Elapsed.TotalMicroseconds) / 1000.0} ms");
            return integral;
        }

        public event EventHandler<LeApproximationData.ComputationDumpEventArgs>? ComputationDump;
        private void OnComputationDump(string line)
        {
            this.ComputationDump?.Invoke(this, new LeApproximationData.ComputationDumpEventArgs(line));
        }
    }
}

