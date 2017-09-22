using ExcelDna.Integration;
using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.Util;
using SpendingPolicyAddin.Seo;
using static System.Math;
using static SharpHelper.Simulation.NormalDist;

namespace SpendingPolicyAddin.Udf
{
    [TemplatedUdfProvider]
    public static class BsmModel
    {
        [ExcelFunction, TemplatedUdf("BlackScholes")]
        public static double EuroCallPricing(SharpExcelObject option, double r)
        {
            var call = option.To<EuroCallOption>();
            var stock = call.Underlying;
            var s = stock.Price;
            var q = stock.Dividend;
            var σ = stock.Sigma;
            var k = call.Strike;
            var T = call.Maturity;

            var d1 = (Log(s / k) + T * (r - q + σ * σ / 2)) / (σ * Sqrt(T));
            var d2 = (Log(s / k) + T * (r - q - σ * σ / 2)) / (σ * Sqrt(T));
            var n1 = NormDist(d1);
            var n2 = NormDist(d2);
            return Exp(-q * T) * s * n1 - Exp(-r * T) * k * n2;
        }
    }
}
