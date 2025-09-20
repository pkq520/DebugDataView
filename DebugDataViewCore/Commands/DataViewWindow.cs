using EnvDTE;
using static DebugDataViewCore.DataView;

namespace DebugDataViewCore
{
    [Command(PackageIds.DataViewWindow)]
    internal sealed class DataViewWindow : BaseCommand<DataViewWindow>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await DataView.ShowAsync();
        }
    }
}
