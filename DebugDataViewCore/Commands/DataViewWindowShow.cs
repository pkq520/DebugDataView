using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace DebugDataViewCore
{
    [Command(PackageIds.DataViewWindowShow)]
    internal sealed class DataViewWindowShow : BaseCommand<DataViewWindowShow>
    {
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
