using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Task = System.Threading.Tasks.Task;
namespace DebugDataViewCore.Commands
{
    [Command(PackageIds.DataViewItemAdd)]
    internal sealed class DataViewItemAdd : BaseCommand<DataViewItemAdd>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            ToolWindowPane window = await DataView.ShowAsync();
            string selected = await GetSelectedTextAsync();
            if (Package is DebugDataViewCorePackage debugDataViewCore)
            {
                if (window?.Content is DataViewWindow dataViewWindow)
                {
                    debugDataViewCore.Init(dataViewWindow);
                    await debugDataViewCore.DataViewItemAddAsync(selected);
                }
            }
        }

        /// <summary>
        /// 获取当前选中的文本
        /// </summary>
        /// <returns>返回异步任务</returns>
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
