using System;
using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;
using SpendingPolicyAddin.InputEndowment;

namespace SpendingPolicyAddin.InputSpending {
    public abstract class SpendingPolicyBase : SharpExcelObject {
        protected SpendingPolicyBase(string name) : base(name) { }
        public abstract double CalcSpending(double t, EndowmentFund fund, double investIncome);
    }

    [TemplatedSeo(Util.SPENDING_CAT), TemplatedSeoMethodProvider(Util.SPENDING_CAT)]
    public class MovingAvgPolicy : SpendingPolicyBase {
        public MovingAvgPolicy(string name, double rate, int quarter, double lag) : base(name) {
            this.Rate = rate;
            this.Quarter = quarter;
            this.Lag = lag;
        }

        public int Quarter { get; }
        public double Lag { get; }
        public double Rate { get; }

        [TemplatedSeoMethod]
        public override double CalcSpending(double t, EndowmentFund fund, double investIncome) {
            //Console.WriteLine(t);
            var cur = fund.HistoricalValues[t - Util.DT];
            if (cur <= fund.Corpus) return 0;
            var avg = fund.MovingAverage(t, this.Quarter, this.Lag);
            var spending = avg * this.Rate;
            return cur + investIncome - spending <= fund.Corpus ? investIncome : spending;
        }
    }
}
