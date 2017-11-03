using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;

namespace SpendingPolicyAddin.Model {

    public abstract class MarketReturnBase : SharpExcelObject {
        public MarketReturnBase(string name, double mean, double sigma) : base(name) {
            this.Mean = mean;
            this.Sigma = sigma;
        }

        public double Mean { get; }
        public double Sigma { get; }

        public abstract double Return(double t, double rnd);
    }

    [TemplatedSeo(Util.MODEL_CAT)]
    public class SimpleMarketReturn : MarketReturnBase {
        public SimpleMarketReturn(string name, double mean, double sigma) : base(name, mean, sigma) { }
        public override double Return(double t, double rnd) => rnd;
    }
}