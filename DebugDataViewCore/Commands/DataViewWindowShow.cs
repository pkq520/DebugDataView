using System.Threading;
using EnvDTE;
using static DebugDataViewCore.DataView;

namespace DebugDataViewCore
{
    [Command(PackageIds.DataViewWindowShow)]
    internal sealed class DataViewWindowShow : BaseCommand<DataViewWindowShow>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await DataView.ShowAsync();
            if (Package is DebugDataViewCorePackage debugDataViewCore)
            {
                ToolWindowPane window = await debugDataViewCore.ShowToolWindowAsync(typeof(Pane), 0, true, debugDataViewCore.CancelToken);
                if (window?.Content is DataViewWindow dataViewWindow) debugDataViewCore.Init(dataViewWindow);
            }
        }
    }
}
