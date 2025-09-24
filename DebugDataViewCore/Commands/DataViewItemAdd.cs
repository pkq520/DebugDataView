using System.Threading;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.TextManager.Interop;
using static DebugDataViewCore.DataView;
namespace DebugDataViewCore.Commands
{
    [Command(PackageIds.DataViewItemAdd)]
    internal sealed class DataViewItemAdd : BaseCommand<DataViewItemAdd>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await DataView.ShowAsync();
            string selected = await GetSelectedTextAsync();
            if (Package is DebugDataViewCorePackage debugDataViewCore)
            {
                ToolWindowPane window = await debugDataViewCore.ShowToolWindowAsync(typeof(Pane), 0, true, debugDataViewCore.CancelToken);
                if (window?.Content is DataViewWindow dataViewWindow)
                {
                    debugDataViewCore.Init(dataViewWindow);
                    await debugDataViewCore.DataViewItemAdd(selected);
                }
            }
        }

        private async Task<string> GetSelectedTextAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            IVsTextManager txtMgr = await VS.GetServiceAsync<SVsTextManager, IVsTextManager>();
            if (txtMgr != null)
            {
                txtMgr.GetActiveView(1, null, out IVsTextView vTextView);
                vTextView.GetSelectedText(out string selectedText);
                return selectedText;
            }
            return "";
        }
    }
}
