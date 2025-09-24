using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        private async Task ShowInfo(string info)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            _dataViewWindow.ShowInfo(info);
        }

        private async Task ShowIntervalinfo(string info)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            _dataViewWindow.ShowIntervalinfo(info);
        }
    }
}
