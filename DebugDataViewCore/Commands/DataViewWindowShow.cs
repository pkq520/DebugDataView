using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace DebugDataViewCore
{
    [Command(PackageIds.DataViewWindowShow)]
    internal sealed class DataViewWindowShow : BaseCommand<DataViewWindowShow>
    {
        /// <summary>
        /// 显示数据视图窗口
        /// </summary>
        /// <param name="e">未知</param>
        /// <returns>返回该任务</returns>
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            ToolWindowPane window = await DataView.ShowAsync();
            if (Package is DebugDataViewCorePackage debugDataViewCore)
            {
                if (window?.Content is DataViewWindow dataViewWindow) debugDataViewCore.Init(dataViewWindow);
            }
        }
    }
}
