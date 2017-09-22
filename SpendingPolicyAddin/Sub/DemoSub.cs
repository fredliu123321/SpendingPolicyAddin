using System;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.Util;

namespace SpendingPolicyAddin.Sub
{
    [TemplatedSubProvider]
    public static class TempSub
    {

        [TemplatedSub("Solve")]
        public static void BiSearch(Action<object> x, Func<object> y, double target, double lower = 0,
                                    double upper = 1)
        {
            double GetY() => Convert.ToDouble(y());


            if (upper < lower)
            {
                x("Error: upper bound must be larger than lower bound");
                return;
            }
            x(upper);
            var vupper = GetY() - target;
            if (vupper == 0) return;
            x(lower);
            var vlower = GetY() - target;
            if (vlower == 0) return;
            if (vupper * vlower > 0)
            {
                x("Error: bad range of x");
                return;
            }
            var guess = (lower + upper) * 0.5;
            while (upper - lower > MathHelper.TOL)
            {
                x(guess);
                var vguess = GetY() - target;
                if (vguess == 0) return;
                if (vguess * vupper < 0)
                    lower = guess;
                else
                {
                    upper = guess;
                    vupper = vguess;
                }
                guess = (lower + upper) * 0.5;
            }
            x(guess);
        }
    }
}