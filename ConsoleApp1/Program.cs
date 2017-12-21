using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Accord.Statistics.Analysis;
using Accord.Statistics.Models.Regression;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix<double> testMatrix = new DenseMatrix(3, 3) {[0, 0] = 70, [0, 1] = 10, [1, 0] = 65, [1, 1] = 20, [2, 0] = 75, [2, 1] = 15};
            var testVector = new DenseVector(new[] {150d, 140, 130});

            //var nextTest = Fit.MultiDim(testMatrix.ToRowArrays(), testVector.ToArray(), false, DirectRegressionMethod.Svd);

            //var test = MultipleRegression.DirectMethod(new DenseMatrix(2, 2) {[0, 0] = 70, [0, 1] = 0.1, [1, 0] = 65, [1, 1] = 0.2}, new DenseVector(new []{150d, 140}), DirectRegressionMethod.Svd);
            //testMatrix = testMatrix.Multiply(4);

            string[] indipendenNames = {"Lenght", "Speed"};
            string target = "time";

            DescriptiveAnalysis da = new DescriptiveAnalysis(testMatrix.ToArray(), indipendenNames);
            da.Compute();

            LogisticRegressionAnalysis lra = new LogisticRegressionAnalysis(testMatrix.ToRowArrays(), testVector.ToArray(), indipendenNames, target);
            lra.Compute();

            MultipleLinearRegressionAnalysis mlra = new MultipleLinearRegressionAnalysis(testMatrix.ToRowArrays(), testVector.ToArray(), indipendenNames, target);
            mlra.Compute();
            //var test = Interpolate.Common(new double[] {6, 60}, new[] {0.7, 0.06});

            //var temp = test.Interpolate(50);
        }
    }
}
