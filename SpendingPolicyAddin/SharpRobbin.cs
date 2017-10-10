using System.Runtime.InteropServices;
using ExcelDna.Integration.CustomUI;
using SharpExcelAddinBase.Ribbon;

namespace SpendingPolicyAddin {
    [ComVisible(true)]
    public class SharpRobbin : ExcelRibbon, ISharpRibbon {
        public override string GetCustomUI(string ribbonId) => SharpRibbonHelper.GetUi();
        public void SubAbout(IRibbonControl control) => SharpRibbonHelper.SubAbout();
        public void SubShowCacheWin(IRibbonControl control) => SharpRibbonHelper.SubShowCacheWin();
        public void SubRunSub(IRibbonControl control) => SharpRibbonHelper.SubRunSub();
        public void SubClearSelection(IRibbonControl control) => SharpRibbonHelper.SubClearSelection();
        public void SubInsertUdfTemp(IRibbonControl control) => SharpRibbonHelper.SubInsertUdfTemp(control);
        public void SubInsertSubTemp(IRibbonControl control) => SharpRibbonHelper.SubInsertSubTemp(control);
        public void SubInsertSeoTemp(IRibbonControl control) => SharpRibbonHelper.SubInsertSeoTemp(control);
        public void SubInsertSeoMethodTemp(IRibbonControl control) => SharpRibbonHelper.SubInsertSeoMethodTemp(control);
    }
}
