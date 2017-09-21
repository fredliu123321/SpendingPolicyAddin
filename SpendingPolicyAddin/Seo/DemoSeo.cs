using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;

namespace SpendingPolicyAddin.Seo
{
    [TemplatedSeoMethodProvider("Two"), TemplatedSeo("Two")]
    public class TwoNumbers : SharpExcelObject
    {
        public double Num1 { get; }
        public double Num2 { get; }

        public TwoNumbers(string name, double num1, double num2) : base(name)
        {
            this.Num1 = num1;
            this.Num2 = num2;
        }

        [TemplatedSeoMethod]
        public double SumOfThree(double num3)
        {
            return this.Num1 + this.Num2 + num3;
        }
    }
}
