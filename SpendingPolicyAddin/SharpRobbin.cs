using System.Runtime.InteropServices;
using ExcelDna.Integration.CustomUI;
using SharpExcelAddinBase.Ribbon;

namespace SpendingPolicyAddin {
    [ComVisible(true)]
    public class SharpRobbin : ExcelRibbon, ISharpDefaultUi {
        public override string GetCustomUI(string ribbonId) => SharpDefaultUi.GetUi();

        public void SubAbout(IRibbonControl control) => SharpDefaultUi.SubAbout();
        public void SubShowCacheWin(IRibbonControl control) => SharpDefaultUi.SubShowCacheWin();
        public void SubInsertUdfTemp(IRibbonControl control) => SharpDefaultUi.SubInsertUdfTemp(control);
        public void SubInsertSubTemp(IRibbonControl control) => SharpDefaultUi.SubInsertSubTemp(control);
        public void SubInsertSeoTemp(IRibbonControl control) => SharpDefaultUi.SubInsertSeoTemp(control);
        public void SubInsertSeoMethodTemp(IRibbonControl control) => SharpDefaultUi.SubInsertSeoMethodTemp(control);
        public void SubRunSub(IRibbonControl control) => SharpDefaultUi.SubRunSub();
        public void SubClearSelection(IRibbonControl control) => SharpDefaultUi.SubClearSelection();
        public void SubFitSelection(IRibbonControl control) => SharpDefaultUi.SubFitSelection();
        public void SubFormatSelection(IRibbonControl control) => SharpDefaultUi.SubFormatSelection();
        public void SubAddObjSelection(IRibbonControl control) => SharpDefaultUi.SubAddObjSelection(control);
        public void SubClearCache(IRibbonControl control) => SharpDefaultUi.SubClearCache();
    }
}
