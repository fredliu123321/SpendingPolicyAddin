using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.MathHelper;

namespace SpendingPolicyAddin.InputInvestment {
    public abstract class InvestPerformanceBase : SharpExcelObject {
        protected InvestPerformanceBase(string name) : base(name) { }

        public abstract int Dimension { get; }
        public abstract double[] Means { get; }
        public abstract double[] Volatilities { get; }
        public abstract double[,] InnerCorr { get; }
        public abstract double[] MarketCorr { get; }

        public abstract double GetReturn(double time, double[] rnds);
    }

    [TemplatedSeo(Util.INVEST_CAT)]
    public class SingleVarInvestment : InvestPerformanceBase {
        public SingleVarInvestment(string name, double correlation, double mean, double sigma) : base(name) {
            this.Rho = correlation;
            this.Mean = mean;
            this.Sigma = sigma;
        }
        public override int Dimension => 1;
        public override double[] Means => new[] {this.Mean};
        public override double[] Volatilities => new[] {this.Sigma};
        public override double[,] InnerCorr => new double[,] {{1}};
        public override double[] MarketCorr => new[] {this.Rho};
        public double Mean { get; set; }
        public double Rho { get; set; }
        public double Sigma { get; set; }

        public override double GetReturn(double time, double[] rnds) => rnds[0];
    }
}
