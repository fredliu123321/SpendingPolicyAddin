using ExcelDna.Integration;
using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.Util;
using SpendingPolicyAddin.Seo;

namespace SpendingPolicyAddin.Udf
{
    [TemplatedUdfProvider]
    public static class SumOfThreeHelper
    {
        [ExcelFunction, TemplatedUdf("Two")]
        public static double SumOfThreeStatic(SharpExcelObject seo, double num3) {
            var tn = seo.To<TwoNumbers>();
            return tn.Num1 + tn.Num2 + num3;
        }
    }
}
