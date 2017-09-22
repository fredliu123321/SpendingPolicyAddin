using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.Util;

namespace SpendingPolicyAddin.Seo
{

    [TemplatedSeo("Underlying")]
    public class Stock : SharpExcelObject
    {
        public Stock(string name, double price, double mu, double sigma, double dividend) : base(name)
        {
            this.Price = price;
            this.Mu = mu;
            this.Sigma = sigma;
            this.Dividend = dividend;
        }

        public double Price { get; }
        public double Mu { get; }
        public double Sigma { get; }
        public double Dividend { get; }
    }

    public class StockOption : SharpExcelObject
    {
        public StockOption(string name, double strike, double maturity, SharpExcelObject underlying) : base(name)
        {
            this.Strike = strike;
            this.Maturity = maturity;
            this.Underlying = underlying.To<Stock>();
        }

        public double Strike { get; }
        public double Maturity { get; }
        public Stock Underlying { get; }
    }

    [TemplatedSeo("Derivatives"), TemplatedSeoMethodProvider("Derivatives")]
    public class EuroCallOption : StockOption
    {
        public EuroCallOption(string name, double strike, double maturity, SharpExcelObject underlying) :
            base(name, strike, maturity, underlying)
        { }

        [TemplatedSeoMethod]
        public object CheckUnderlying()
        {
            return this.Underlying;
        }
    }
}
