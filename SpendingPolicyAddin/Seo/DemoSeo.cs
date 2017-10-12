using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.Util;

namespace SpendingPolicyAddin.Seo {

    [TemplatedSeo("Underlying", Description = "Stock prompt")]
    public class Stock : SharpExcelObject {
        public Stock(string name,
                     [ParaText("Current stock price"), DoubleRange(0)] double price,
                     [ParaText("Mean return")] double mu,
                     [ParaText("Volatility"), DoubleRange(0, 1)] double sigma,
                     [ParaText("Dividend yield"), DoubleRange(0, 1)] double dividend) : base(name) {
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

    public enum OptionType {
        Call,
        Put
    }

    [TemplatedSeoMethodProvider("Derivatives")]
    public class StockOption : SharpExcelObject {
        public StockOption(string name, double strike, double maturity, Stock underlying,
                           OptionType optionType) : base(name) {
            this.Strike = strike;
            this.Maturity = maturity;
            this.OptionType = optionType;
            this.Underlying = underlying;//.To<Stock>();
        }

        public double Strike { get; }
        public double Maturity { get; }
        public Stock Underlying { get; }
        public OptionType OptionType { get; }

        [TemplatedSeoMethod(Description = "Show the underlying of a option")]
        public object CheckUnderlying() {
            return this.Underlying;
        }
    }

    [TemplatedSeo("Derivatives", Description = "European option")]
    public class EuroStockOption : StockOption {
        public EuroStockOption(string name,
                               [ParaText("Strike price"), DoubleRange(0)] double strike,
                               [ParaText("Time to maturity"), DoubleRange(0)] double maturity,
                               [ParaText("Underlying")] Stock underlying,
                               [ParaText("Option type")] OptionType optionType) :
            base(name, strike, maturity, underlying, optionType) { }
    }

   
}
