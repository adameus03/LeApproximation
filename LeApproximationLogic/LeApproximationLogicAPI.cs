/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class LeApproximationLogicAPI
    {
    }
}*/

using LeApproximationData;
using LeApproximationLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class LeApproximationLogicAPI : LeApproximationAbstractLogicAPI
    {
        private double lastApproximationError = 0;
        public LeApproximationLogicAPI(LeApproximationData.LeApproximationAbstractDataAPI dataAPI) : base(dataAPI) { }


        public override Func<double, double> Approximate<TIntegrator>(int functionIndex, double approximationLeftBound, double approximationRightBound, int polynomialOrder, double integrationAccuracy, IntegratorInfo? integratorInfo = null)
        {
            /*
             * Highest order coefficient of a n-th degree Legendre polynomial can be expressed with a simple formula:
             * a_n = (2n)!/(2^n*(n!^2))            where n is a shorthand for polynomialOrder
             * 
             * Therefore a n-th degree Legendre polynomial can be calculated as
             * P_n(x) = (2n)!/(2^n*(n!^2)) * (x-x_0) * (x-x_1) * (x-x_2) * ... * (x-x_{n-1}) 
             * where x_1, x_2, x_3, ..., x_{n-1} are the roots of P_n(x) loaded from the file legendre.txt which resides in the LeApproximationData subproject 
             */

            /*double factorialAccumulator = 1;
            double powerAccumulator = 1;

            for(int i=1; i<=polynomialOrder; i++)
            {
                factorialAccumulator *= i;
                powerAccumulator *= 2;
            }

            double nFactorial = factorialAccumulator;

            for(int i=polynomialOrder+1; i<=2*polynomialOrder; i++)
            {
                factorialAccumulator *= i;
            }

            double twoNFactorial = factorialAccumulator;

            double legendreHighestCoefficient = twoNFactorial / (powerAccumulator * nFactorial * nFactorial);



            Func<double, int, double> legendrePolynomial = (x, j) =>
            {
                if (j == 0) return 1;
                if (j == 1) return x;
                if (j == 2) return 1.5 * x * x - 0.5;
                if (j == 3) return 2.5 * x * x * x - 1.5 * x;
                //else if (j == 1) return x;
                double[] legendreZeroes = this.dataAPI.GetGLQuadratureData(j).Select((node) => node.Item2).ToArray();
                return legendreHighestCoefficient * legendreZeroes.Aggregate(1.0, (prod, root) => prod * (x - root));
            };*/

            /*
             *  The function f(x), being subject to the approximation, can have its (a, b) domain scaled, such that the approximation process is executed over the (-1, 1) interval.
             *  This is achieved by approximating g(x)=f(t(x)) instead, where t=-1+2*(x-a)/(b-a).
             *  After approximating g(x)=f(t(x)) with G(x), we can return the seeked approximation function for f(x) as:
             *  F(x) = G(0.5*(b-a)*x+0.5*(b+a))
            */

            /* Func<double, double> f = base.dataAPI.GetFunction(functionIndex);
             Func<double, double> g = (x) => f
             (
                 //0.5 * (approximationRightBound - approximationLeftBound) * x + 0.5 * (approximationRightBound + approximationLeftBound)
                 x
             );
             double test = legendrePolynomial(0.5, 1);
             Func<double, int, double> integrandFunction = (x, j) => {
                 return g(x) * legendrePolynomial(x, j); 
             };
             //double test = integrandFunction()*/

            /*
             *  The approximation function G(x) is defined as follows:
             *  G(x) = lambda_0*P_0(x)+ lambda_1*P_1(x) + lambda_2*P_2(x) + ... + lambda_n*P_n(x)          where P_j(x) for j=0,1,2,...,n are the Legendre polynomials
             *  We can calculate the lambda coefficients using the formula below:
             *  lambda_j = (2j+1)/2 * int_{-1}^{1} g(x)*P_j(x)*dx
             */

            /*double[] lambdaCoefficients = Enumerable.Range(0, polynomialOrder + 1).Select((j) => ((float)(2 * j + 1)) / 2.0f * this.Integrate<TIntegrator>((x) => integrandFunction(x, j), -1, 1, integrationAccuracy, integratorInfo)).ToArray();*/

            /*
             * 
             * Mean squared error:
             *  M^2 = 1/2 * int_{-1}^{1} g(x)^2 dx - ||P_0||a_0 - ||P_1||a_1 - ||P_2||a_2 - ... - ||P_n||a_n
             * where you can calculate ||P_j|| using the formula:
             *  ||P_j|| = 2/(2j+1)
             */

            //double difference_arg_1 = 

            /*double mean_error_squared = Math.Abs(0.5*(this.Integrate<TIntegrator>((x) => g(x) * g(x), -1, 1, integrationAccuracy, integratorInfo) - Enumerable.Range(0, polynomialOrder + 1).Sum
            (
                (j) => lambdaCoefficients[j] * lambdaCoefficients[j] * ((double)2.0) / (2.0 * j + 1.0)
            )));*/

            /*double accuracy = Math.Sqrt(mean_error_squared);

            OnComputationDump(this, new ComputationDumpEventArgs("Coeffs: [" + string.Join(", ", lambdaCoefficients) + "]"));
            OnComputationDump(this, new ComputationDumpEventArgs($"Order: {polynomialOrder} | inaccuracy: {accuracy}"));
            this.lastApproximationError = accuracy;
            return (x) => Enumerable.Range(0, polynomialOrder + 1).Sum((j) => lambdaCoefficients[j] * legendrePolynomial
            (
                //-1 + 2 * (x - approximationLeftBound) / (approximationRightBound - approximationLeftBound), j
               //approximationLeftBound+((double)(x+1))/2.0*(approximationRightBound-approximationLeftBound), j
               x, j
            ));*/

            Approximator<TIntegrator> approximator = Approximator<TIntegrator>.CreateInstance(this.dataAPI.GetFunction(functionIndex), approximationLeftBound, approximationRightBound, Approximator<TIntegrator>.TerminationCondition.ByOrder, new LegendreApproximatorInfo(this, polynomialOrder), integrationAccuracy, integratorInfo);
            approximator.ComputationDump += Approximator_ComputationDump;
            return approximator.Approximate();
        }

        private void Approximator_ComputationDump(object? sender, ComputationDumpEventArgs e)
        {
            base.OnComputationDump(sender, e);
        }

        public override Func<double, double> Approximate<TIntegrator>(int functionIndex, double approximationLeftBound, double approximationRightBound, double approximationAccuracy, double integrationAccuracy, IntegratorInfo? integratorInfo = null)
        {
            /*Func<double, double> approximationFunction;
            int polynomialOrder = 1;
            do
            {
                approximationFunction = this.Approximate<TIntegrator>(functionIndex, approximationLeftBound, approximationRightBound, polynomialOrder, integrationAccuracy, integratorInfo);
                polynomialOrder++;
                if (this.lastApproximationError >= 15000) { 
                    OnComputationDump(this, new ComputationDumpEventArgs("Computation dropped due to huge numerical errors - try increasing integration accuracy!"));
                    return approximationFunction;
                }
            }
            while (this.lastApproximationError > approximationAccuracy);
            return approximationFunction;*/

            Approximator<TIntegrator> approximator = Approximator<TIntegrator>.CreateInstance(this.dataAPI.GetFunction(functionIndex), approximationLeftBound, approximationRightBound, Approximator<TIntegrator>.TerminationCondition.ByAccuracy, new LegendreApproximatorInfo(this, approximationAccuracy), integrationAccuracy, integratorInfo);
            approximator.ComputationDump += Approximator_ComputationDump;
            return approximator.Approximate();
        }

        public override double Integrate<TIntegrator>(Func<double, double> function, double integrationLeftBound, double integrationRightBound, double desiredAccuracy, IntegratorInfo? integratorInfo = null)
        {
            Integrator integrator;
            if (typeof(SimpsonIntegrator) == typeof(TIntegrator))
            {
                integrator = new SimpsonIntegrator(function, integrationLeftBound, integrationRightBound, desiredAccuracy);
            }
            else if (typeof(GaussLegendreIntegrator) == typeof(TIntegrator))
            {
                if (integratorInfo == null) throw new NullReferenceException();
                else integrator = new GaussLegendreIntegrator(function, integrationLeftBound, integrationRightBound, desiredAccuracy, (GaussLegendreIntegratorInfo)integratorInfo);
            }
            else throw new TypeAccessException();

            integrator.ComputationDump += Integrator_ComputationDump;
            return integrator.Integrate();
        }
        public override double Integrate<TIntegrator>(int functionIndex, double integrationLeftBound, double integrationRightBound, double desiredAccuracy, IntegratorInfo? integratorInfo = null)
        {
            /*Integrator integrator;
            if (typeof(SimpsonIntegrator) == typeof(TIntegrator))
            {
                integrator = new SimpsonIntegrator(base.dataAPI.GetFunction(functionIndex), integrationLeftBound, integrationRightBound, desiredAccuracy);
            }
            else if (typeof(GaussLegendreIntegrator) == typeof(TIntegrator))
            {
                if (integratorInfo == null) throw new NullReferenceException();
                else integrator = new GaussLegendreIntegrator(base.dataAPI.GetFunction(functionIndex), integrationLeftBound, integrationRightBound, desiredAccuracy, (GaussLegendreIntegratorInfo)integratorInfo);
            }
            else throw new TypeAccessException();

            integrator.ComputationDump += Integrator_ComputationDump;
            return integrator.Integrate();*/
            return this.Integrate<TIntegrator>(base.dataAPI.GetFunction(functionIndex), integrationLeftBound, integrationRightBound, desiredAccuracy, integratorInfo);
        }

        private void Integrator_ComputationDump(object? sender, ComputationDumpEventArgs e)
        {
            base.OnComputationDump(sender, e);
        }
    }
}

