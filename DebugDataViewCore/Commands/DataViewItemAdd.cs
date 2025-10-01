using System;
using System.Linq;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using static DataSturctures.StatusEnum;
using Task = System.Threading.Tasks.Task;
namespace DebugDataViewCore.Commands
{
    [Command(PackageIds.DataViewItemAdd)]
    internal sealed class DataViewItemAdd : BaseCommand<DataViewItemAdd>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            if (Package is DebugDataViewCorePackage debugDataViewCore)
            {
                if (debugDataViewCore.Program_LanguageDetection())
                {
                    ToolWindowPane window = await DataView.ShowAsync();
                    string selected = await GetSelectedTextAsync();
                    if (window?.Content is DataViewWindow dataViewWindow)
                    {
                        debugDataViewCore.Init(dataViewWindow);
                        await debugDataViewCore.DataViewItemAddAsync(selected);
                    }
                }
                else
                {
                    string supportProgramLanguage = string.Join(",",
                        Enum.GetValues(typeof(ProgramLanguageEnum))
                            .Cast<ProgramLanguageEnum>()
                            .Where(item => item != ProgramLanguageEnum.Unknow)
                            .Select(item => item.ToString())
                    );
                    await VS.MessageBox.ShowWarningAsync(
                        "The current plugin only supports the following languages:" + 
                        supportProgramLanguage);
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
