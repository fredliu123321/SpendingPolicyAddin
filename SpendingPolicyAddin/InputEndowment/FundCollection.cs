using System;
using System.Linq;
using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;

namespace SpendingPolicyAddin.InputEndowment {

    [TemplatedSeo(Util.FUND_CAT, Description = CLASS_DESCRIPTION)]
    public class FundCollection : SharpExcelObject {
        private const string CLASS_DESCRIPTION = "A collection of endowment funds";

        public FundCollection(string name, string[] fundIds) : base(name) {
            if (fundIds == null || fundIds.Length <= 0) throw new ArgumentNullException(nameof(fundIds));
            _funds = fundIds.Select(id => ExcelObjectHelper.Get<EndowmentFund>(id)).Where(id => id != null).ToArray();
        }

        public readonly EndowmentFund[] _funds;
        public int Count => _funds.Length;

        public EndowmentFund[] Funds() => _funds;
    }
}
