using System;
using System.Collections.Generic;
using System.Linq;
using SharpExcelAddinBase.ObjectSystem;
using SharpHelper.MathHelper;
using SharpHelper.Util;
using EArg = ExcelDna.Integration.ExcelArgumentAttribute;
using EFunc = ExcelDna.Integration.ExcelFunctionAttribute;
using TSeo = SharpExcelAddinBase.TemplateFunction.TemplatedSeoAttribute;
using TSeoM = SharpExcelAddinBase.TemplateFunction.TemplatedSeoMethodAttribute;
using TSeoMP = SharpExcelAddinBase.TemplateFunction.TemplatedSeoMethodProviderAttribute;

namespace SpendingPolicyAddin.InputEndowment {

    [TSeo(Util.FUND_CAT, Description = CLASS_DESCRIPTION), TSeoMP(Util.FUND_CAT)]
    public class EndowmentFund : SharpExcelObject {

        // TODO argument descriptions

        private const string CLASS_DESCRIPTION = "An endowment fund";
        private const string NEW_DESCRIPTION = "Initial a new endowment fund entry";
        private const string NAME_ARG_DESCRIPTION = "Unique name of the new endowment fund";
        private const string TIMES_ARG_DESCRIPTION = "History time-points of the new endowment fund";
        private const string VALUES_ARG_DESCRIPTION = "History values of the new endowment fund";
        private const string CORPUS_ARG_DESCRIPTION = "Corpus value of the new endowment fund";


        public EndowmentFund(string name, double corpus, double[] values, double[] times = null) : base(name) {

            var len = values.Length;
            times = times ?? len.For(i => len - i - 2d).Multiply(-Util.DT);

            HistoricalValues = new Dictionary<double, double>();
            for (var i = 0; i < values.Length; i++) HistoricalValues[times[i]] = values[i];
            this.Corpus = corpus;
            // TODO .ctor method
        }

        // TODO definition for each Endowment Fund
        public int Hash => HistoricalValues.GetHashCode();

        public readonly Dictionary<double, double> HistoricalValues;

        public double Corpus { get; }
        public double OldestHistory => HistoricalValues.Keys.Min();


        [EFunc(NEW_DESCRIPTION, Category = Util.EX_FUND_CAT, IsMacroType = true, IsVolatile = true)]
        public static string NewEndowmentFund([EArg(NAME_ARG_DESCRIPTION)] string name,
                                              [EArg(CORPUS_ARG_DESCRIPTION)] double corpus,
                                              [EArg(VALUES_ARG_DESCRIPTION)] double[] values,
                                              [EArg(TIMES_ARG_DESCRIPTION)] double[] times = null) {

            // TODO Excel function to create new Endowment Fund
            var fund = new EndowmentFund(name, corpus, values, times);

            return ExcelObjectHelper.Register(name, fund);
        }

        [TSeoM]
        public double MovingAverage(double t, [IntRange(1)] int quarter = 8, [DoubleRange(0)] double lag = 1) {
            if (lag < 0) throw new ArgumentOutOfRangeException(nameof(lag), "Lag cannot be negative");
            var endT = t - lag;
            var startT = Math.Max(endT - (quarter - 1) * Util.DT, this.OldestHistory);
            var values = new List<double>();
            for (var th = startT; th <= endT; th += Util.DT)
                if (HistoricalValues.TryGetValue(th, out var v))
                    values.Add(v);
                else Console.WriteLine($"Key not found {th}");
            return values.Any() ? values.Average() : default;
        }
    }
}
