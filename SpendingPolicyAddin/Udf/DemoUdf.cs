using System;
using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.Util;
using SpendingPolicyAddin.Seo;
using static System.Math;
using static SharpHelper.Simulation.NormalDist;

namespace SpendingPolicyAddin.Udf {
    [TemplatedUdfProvider]
    public static class BsmModel {
        [TemplatedUdf("BlackScholes", Description = "Calculate price by BSM")]
        public static double EuroOptionPricing(
            [ParaText("Option to be calculated"), SeoType(typeof(StockOption))] SharpExcelObject option,
            [ParaText("Risk free rate to be used"), DblRange(-1, 1)] double r) {
            var op = option.To<EuroStockOption>();
            var stock = op.Underlying;
            var s = stock.Price;
            var q = stock.Dividend;
            var σ = stock.Sigma;
            var k = op.Strike;
            var T = op.Maturity;

            var d1 = (Log(s / k) + T * (r - q + σ * σ / 2)) / (σ * Sqrt(T));
            var d2 = (Log(s / k) + T * (r - q - σ * σ / 2)) / (σ * Sqrt(T));

            switch (op.OptionType) {
                case OptionType.Call:
                    return Exp(-q * T) * s * NormDist(d1) - Exp(-r * T) * k * NormDist(d2);
                case OptionType.Put:
                    return -Exp(-q * T) * s * NormDist(-d1) + Exp(-r * T) * k * NormDist(-d2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
