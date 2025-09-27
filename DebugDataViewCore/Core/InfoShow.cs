using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        /// <summary>
        /// 展示运行信息
        /// </summary>
        /// <param name="info">显示的信息</param>
        /// <returns>返回异步任务</returns>
        private async Task ShowRunInfoAsync(string info)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            _dataViewWindow.ShowRunInfo(info);
        }

        /// <summary>
        /// 展示数据划分信息
        /// </summary>
        /// <param name="info">显示的信息</param>
        /// <returns>返回异步任务</returns>
        private async Task ShowDataIntervalinfoAsync(string info)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            _dataViewWindow.ShowDataIntervalinfo(info);
        }
    }
}
