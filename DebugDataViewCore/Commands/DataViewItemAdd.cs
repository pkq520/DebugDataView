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
        /// <summary>
        /// 添加数据源字段
        /// </summary>
        /// <param name="e">未知</param>
        /// <returns>返回该任务</returns>
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (Package is DebugDataViewCorePackage debugDataViewCore)
            {
                if (!debugDataViewCore.ProgramLanguageDetection())
                {
                    string supportProgramLanguage = string.Join(",",
                        Enum.GetValues(typeof(ProgramLanguageEnum))
                            .Cast<ProgramLanguageEnum>()
                            .Where(item => item != ProgramLanguageEnum.Unknow)
                            .Select(item => item.ToString())
                    );
                    await VS.MessageBox.ShowWarningAsync(
                        "The current plugin only supports the following languages:" + Environment.NewLine +
                        supportProgramLanguage);
                    return;
                }
                string selected = await GetSelectedTextAsync();
                if (await debugDataViewCore.ValueTypeDetectionAsync(selected) == false)
                {
                    string supportValueType = string.Join(",",
                        Enum.GetValues(typeof(SupportValueTypeEnum))
                            .Cast<SupportValueTypeEnum>()
                            .Select(item => item.ToString())
                    );
                    await VS.MessageBox.ShowWarningAsync(
                        "The current plugin only supports the following valueType:" + Environment.NewLine +
                        supportValueType);
                    return;
                }

                ToolWindowPane window = await DataView.ShowAsync();
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
