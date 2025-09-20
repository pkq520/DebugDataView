using EnvDTE;
using static DebugDataViewCore.DataView;

namespace DebugDataViewCore
{
    [Command(PackageIds.DebugDataView)]
    internal sealed class DebugDataView : BaseCommand<DebugDataView>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await DataView.ShowAsync();
        }
    }
}
