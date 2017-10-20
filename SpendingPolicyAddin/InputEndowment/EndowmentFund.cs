using ExcelDna.Integration;
using SharpExcelAddinBase.ObjectSystem;
using SharpExcelAddinBase.TemplateFunction;

namespace SpendingPolicyAddin.InputEndowment {

    [TemplatedSeo(Util.FUND_CAT, Description = CLASS_DESCRIPTION)]
    public class EndowmentFund : SharpExcelObject {

        // TODO argument descriptions

        private const string CLASS_DESCRIPTION = "An endowment fund";
        private const string NEW_DESCRIPTION = "Initial a new endowment fund entry";
        private const string NAME_ARG_DESCRIPTION = "Unique name of the new endowment fund";


        public EndowmentFund(string name) : base(name) {
            // TODO .ctor method
        }

        // TODO definition for each Endowment Fund

        [ExcelFunction(NEW_DESCRIPTION, Category = Util.EX_FUND_CAT, IsMacroType = true, IsVolatile = true)]
        public static string newEndowmentFund([ExcelArgument(NAME_ARG_DESCRIPTION)] string name) {

            // TODO Excel function to create new Endowment Fund
            var fund = new EndowmentFund(name);

            return ExcelObjectHelper.Register(name, fund);
        }
    }
}
