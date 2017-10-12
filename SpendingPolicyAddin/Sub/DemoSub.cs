using System;
using SharpExcelAddinBase.TemplateFunction;
using SharpHelper.Util;

namespace SpendingPolicyAddin.Sub {
    [TemplatedSubProvider]
    public static class TempSub {

        [TemplatedSub("Solve", Description = "Solve a monotone equation")]
        public static void BiSearch(
            [ParaText("Independent variable")] Action<object> x,
            [ParaText("Dependent variable")] Func<double> y,
            [ParaText("Target value for y to reach")] double target,
            [ParaText("Lower bound"), DoubleRange] double lower = 0,
            [ParaText("Upper bound"), DoubleRange] double upper = 1) {
            if (upper < lower) {
                x("Error: upper bound must be larger than lower bound");
                return;
            }
            x(upper);
            var vupper = y() - target;
            if (vupper == 0) return;
            x(lower);
            var vlower = y() - target;
            if (vlower == 0) return;
            if (vupper * vlower > 0) {
                x("Error: bad range of x");
                return;
            }
            var guess = (lower + upper) * 0.5;
            while (upper - lower > MathHelper.TOL) {
                x(guess);
                var vguess = y() - target;
                if (vguess == 0) return;
                if (vguess * vupper < 0)
                    lower = guess;
                else {
                    upper = guess;
                    vupper = vguess;
                }
                guess = (lower + upper) * 0.5;
            }
            x(guess);
        }
    }
}