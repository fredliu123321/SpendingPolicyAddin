using System;
using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;

namespace SpendingPolicyAddin.InputSpending {
    public abstract class AdjustSpendingBase : SharpExcelObject {
        protected AdjustSpendingBase(string name) : base(name) { }
        public abstract int Dimension { get; }
        public abstract double AdjustSpending(double t, double[] rnds = null);
    }

    [TemplatedSeo(Util.SPENDING_CAT)]
    public class ConstantAdjSpending : AdjustSpendingBase {
        public ConstantAdjSpending(string name, double spending, int year = 1) : base(name) {
            this.Spending = spending;
            this.Year = year;
        }

        public override int Dimension => 0;

        public double Spending { get; }
        public int Year { get; }

        public override double AdjustSpending(double t, double[] rnds = null) {
            if (t != Math.Floor(t)) return 0;
            return t >= this.Year ? 0 : this.Spending;
        }
    }
}
