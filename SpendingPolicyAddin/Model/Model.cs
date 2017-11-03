using System;
using System.Diagnostics;
using System.Linq;
using SharpExcelAddinBase.DataObject;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.MathHelper;
using SharpHelper.Simulation;
using SharpHelper.Util;
using SpendingPolicyAddin.InputEndowment;
using SpendingPolicyAddin.InputInvestment;
using SpendingPolicyAddin.InputSpending;

namespace SpendingPolicyAddin.Model {

    public struct PathDot {
        public double MarketReturn;
        public double InvestmentReturn;
        public double AdjustSpending;
        public double AdjInvestmentReturn;
        public double[] FundReturn;
        public double[] Spendings;
        public double[] FundValueAfter;
    }

    [TemplatedSeo(Util.MODEL_CAT), TemplatedSeoMethodProvider(Util.MODEL_CAT)]
    public class Model : SimulationSeoBase<PathDot[]> {
        public Model(string name,
                     int count,
                     FundCollection funds,
                     MarketReturnBase market,
                     InvestPerformanceBase invest,
                     SpendingPolicyBase spending,
                     AdjustSpendingBase adjustSpending = null) : base(name, count) {
            this.Funds = funds;
            this.Invest = invest;
            this.Spending = spending;
            this.Market = market;
            this.AdjustSpending = adjustSpending;

            var dim = 1 + invest.Dimension; // TODO dimension
            var miu = new double[dim];
            var sigma = new double[dim];
            var corr = LinearAlgebraHelper.Identity(dim);

            // TODO build sigma
            sigma[0] = this.Market.Sigma;
            this.Invest.Dimension.For(i => { sigma[i + 1] = this.Invest.Volatilities[i]; });

            // TODO build miu
            miu[0] = this.Market.Mean;
            this.Invest.Dimension.For(i => { miu[i + 1] = this.Invest.Means[i]; });

            // TODO build corr
            corr.Paste(this.Invest.InnerCorr, 1, 1)
                .PasteCol(this.Invest.MarketCorr, 1);

            var sigmaM = sigma.Diag();
            var cov = sigmaM.Mul(corr.Symmetric()).Mul(sigmaM);

            Console.WriteLine(sigma.JoinStr());
            Console.WriteLine(miu.JoinStr());
            Console.WriteLine(sigmaM.MatrixToString());
            Console.WriteLine(corr.MatrixToString());
            Console.WriteLine(cov.MatrixToString());

            _norm = new MultiNormalSampleGenerator(dim, cov, miu);
        }

        public FundCollection Funds { get; set; }
        public MarketReturnBase Market { get; set; }
        public InvestPerformanceBase Invest { get; set; }
        public SpendingPolicyBase Spending { get; set; }
        public AdjustSpendingBase AdjustSpending { get; set; }

        public readonly MultiNormalSampleGenerator _norm;

        public int _year = 5;


        [TemplatedSeoMethod]
        public double TestGen(int year = 10) {
            _year = year;
            var watch = Stopwatch.StartNew();
            var path = GenerateSample();
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public override PathDot[] GenerateSample() {
            var dots = _year * Util.FREQ + 1;
            var path = new PathDot[dots];
            var rndss = _norm.GenerateSamplesBySam(dots);

            var invest = this.Invest;
            var adjSpendingPolicy = this.AdjustSpending;
            var funds = this.Funds.Funds();
            var spending = this.Spending;
            var curValue = funds.Select(f => f.HistoricalValues[0]).ToArray();

            for (var i = 0; i < dots; i++) {
                var t = i * Util.DT;
                var rnds = rndss[i];
                var dot = new PathDot();

                // randoms
                var mktRnd = rnds[0];
                var invRnd = rnds.SubByLen(1, invest.Dimension).ToArray();
                var adjRnd = rnds.SubByLen(invest.Dimension, adjSpendingPolicy.Dimension).ToArray();

                // market return
                var marketReturn = this.Market.Return(t, mktRnd);
                dot.MarketReturn = marketReturn;

                // investment return
                var returnRate = invest.GetReturn(t, invRnd);
                var totalReturn = returnRate * Util.DT * curValue.Sum();
                dot.InvestmentReturn = returnRate;

                // - adj spending
                var adjSpending = adjSpendingPolicy.AdjustSpending(t, adjRnd);
                dot.AdjustSpending = adjSpending;
                var adjReturn = totalReturn - adjSpending;
                var adjReturnRate = adjReturn * Util.FREQ / curValue.Sum();
                dot.AdjInvestmentReturn = adjReturnRate;

                // return per fund
                var returns = curValue.Multiply(adjReturnRate * Util.DT);
                dot.FundReturn = returns;

                var spendings = new double[funds.Length];
                // - spending
                if (i % Util.FREQ == 0) {
                    spendings = funds.Select((f, fi) => spending.CalcSpending(t, f, returns[fi])).ToArray();
                    dot.Spendings = spendings;
                }

                // after value update
                var newValues = curValue.Plus(returns).Minus(spendings);
                curValue = newValues;
                dot.FundValueAfter = newValues;
                for (var fi = 0; fi < funds.Length; fi++) {
                    Console.WriteLine($"Update value at {t}: {newValues[fi]}");
                    funds[fi].HistoricalValues[t] = newValues[fi];
                }

                path[i] = dot;
            }
            return path;
        }
    }
}
