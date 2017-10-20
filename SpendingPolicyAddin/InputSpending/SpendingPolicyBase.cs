using SharpExcelAddinBase.ObjectSystem;

namespace SpendingPolicyAddin.InputSpending {
    public abstract class SpendingPolicyBase : SharpExcelObject {
        protected SpendingPolicyBase(string name) : base(name) { }
    }
}
