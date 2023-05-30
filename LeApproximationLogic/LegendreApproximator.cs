using LeApproximationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationLogic
{
    internal class LegendreApproximator<TIntegrator> : Approximator<TIntegrator> where TIntegrator : Integrator
    {
        private double LegendrePolynomial(double x, int polynomialOrder)
        {
            /*
             * Highest order coefficient of a n-th degree Legendre polynomial can be expressed with a simple formula:
             * a_n = (2n)!/(2^n*(n!^2))            where n is a shorthand for polynomialOrder
             * 
             * Therefore a n-th degree Legendre polynomial can be calculated as
             * P_n(x) = (2n)!/(2^n*(n!^2)) * (x-x_0) * (x-x_1) * (x-x_2) * ... * (x-x_{n-1}) 
             * where x_1, x_2, x_3, ..., x_{n-1} are the roots of P_n(x) loaded from the file legendre.txt which resides in the LeApproximationData subproject 
             */
            if (polynomialOrder == 0) return 1;

            double factorialAccumulator = 1;
            double powerAccumulator = 1;

            for (int i = 1; i <= polynomialOrder; i++)
            {
                factorialAccumulator *= i;
                powerAccumulator *= 2;
            }

            double nFactorial = factorialAccumulator;

            for (int i = polynomialOrder + 1; i <= 2 * polynomialOrder; i++)
            {
                factorialAccumulator *= i;
            }

            double twoNFactorial = factorialAccumulator;

            double legendreHighestCoefficient = twoNFactorial / (powerAccumulator * nFactorial * nFactorial);

            double[] legendreZeroes = this.info.LogicAPI.DataAPI.GetGLQuadratureData(polynomialOrder).Select((node) => node.Item2).ToArray();
            return legendreHighestCoefficient * legendreZeroes.Aggregate(1.0, (prod, root) => prod * (x - root));
        }

        private Func<double, double> StepApproximation(int m)
        {
            /*
             *  The function f(x), being subject to the approximation, can have its (a, b) domain scaled, such that the approximation process is executed over the (-1, 1) interval.
             *  This is achieved by approximating g(x)=f(t(x)) instead, where t=-1+2*(x-a)/(b-a).
             *  After approximating g(x)=f(t(x)) with G(x), we can return the seeked approximation function for f(x) as:
             *  F(x) = G(0.5*(b-a)*x+0.5*(b+a))
            */

            Func<double, double> g = LegendreApproximator<TIntegrator>.FixedDomain(base.f, base.a, base.b);
            Func<double, int, double> integrandFunction = (x, j) =>
            {
                return g(x) * this.LegendrePolynomial(x, j);
            };
            /*
             *  The approximation function G(x) is defined as follows:
             *  G(x) = lambda_0*P_0(x)+ lambda_1*P_1(x) + lambda_2*P_2(x) + ... + lambda_n*P_n(x)          where P_j(x) for j=0,1,2,...,n are the Legendre polynomials
             *  We can calculate the lambda coefficients using the formula below:
             *  lambda_j = (2j+1)/2 * int_{-1}^{1} g(x)*P_j(x)*dx
             */
            double[] lambdaCoefficients = Enumerable.Range(0, m + 1).Select((j) => ((float)(2 * j + 1)) / 2.0f * info.LogicAPI.Integrate<TIntegrator>
            (
                (x) => integrandFunction(x, j), -1, 1, integrationEpsilon, integratorInfo
            )).ToArray();
            /*
             * 
             * Mean squared error:
             *  M^2 = 1/2 * int_{-1}^{1} g(x)^2 dx - ||P_0||a_0 - ||P_1||a_1 - ||P_2||a_2 - ... - ||P_n||a_n
             * where you can calculate ||P_j|| using the formula:
             *  ||P_j|| = 2/(2j+1)
             */
            double mean_error_squared = Math.Abs(0.5 * (info.LogicAPI.Integrate<TIntegrator>((x) => g(x) * g(x), -1, 1, integrationEpsilon, integratorInfo) - Enumerable.Range(0, m + 1).Sum
            (
                (j) => lambdaCoefficients[j] * lambdaCoefficients[j] * ((double)2.0) / (2.0 * j + 1.0)
            )));

            double accuracy = Math.Sqrt(mean_error_squared);

            OnComputationDump("Coeffs: [" + string.Join(", ", lambdaCoefficients) + "]");
            OnComputationDump($"Order: {m} | inaccuracy: {accuracy}");

            this.lastAccuracyAchieved = accuracy;
            Func<double, double> G = (x) => Enumerable.Range(0, m + 1).Sum((j) => lambdaCoefficients[j] * this.LegendrePolynomial(x, j));
            Func<double, double> F = LegendreApproximator<TIntegrator>.RetrieveDomain(G, a, b);
            return F;
        }

        private static Func<double, double> FixedDomain(Func<double, double> f, double a, double b)
        {
            Func<double, double> g = (x) => f
            (
                a + ((double)(x + 1)) / 2.0 * (b - a)
                
                //x
            );
            return g;
        }

        private static Func<double, double> RetrieveDomain(Func<double, double> G, double a, double b)
        {
            Func<double, double> F = (x) => G
            (
               -1 + 2 * (x - a) / (b - a)
               //0.5 * (b - a) * x + 0.5 * (b + a)
               //x
            );
            return F;
        }

        

        public LegendreApproximator(Func<double, double> f, double a, double b, TerminationCondition terminationCondition, LegendreApproximatorInfo info, double integrationEpsilon, IntegratorInfo? integratorInfo = null) : base(f, a, b, terminationCondition, info, integrationEpsilon, integratorInfo) { }

        public override Func<double, double> Approximate()
        {
            if(base.terminationCondition == TerminationCondition.ByOrder)
            {
                return this.StepApproximation(((LegendreApproximatorInfo)info).PolynomialOrder);
            }
            else
            {
                Func<double, double> approximationFunction;
                int polynomialOrder = 1;
                do
                {
                    approximationFunction = this.StepApproximation(polynomialOrder);
                    polynomialOrder++;
                    if (polynomialOrder >= 30)
                    {
                        OnComputationDump("Computation dropped due to huge numerical errors - try increasing integration accuracy!");
                        return approximationFunction;
                    }
                }
                while (this.lastAccuracyAchieved > ((LegendreApproximatorInfo)base.info).ApproximationAccuracy);
                return approximationFunction;
            }
        }

    }
}
