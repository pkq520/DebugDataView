using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.TextManager.Interop;
namespace DebugDataViewCore.Commands
{
    [Command(PackageIds.DataViewAddItem)]
    internal sealed class DataViewAddItem : BaseCommand<DataViewAddItem>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            string selected = await GetSelectedTextAsync();
            if (Package is DebugDataViewCorePackage debugDataViewCore) debugDataViewCore.DataViewAddItem(selected);
        }

        private async Task<string> GetSelectedTextAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var txtMgr = await VS.GetServiceAsync<SVsTextManager, IVsTextManager>();
            txtMgr.GetActiveView(1, null, out IVsTextView vTextView);

            vTextView.GetSelection(out int startLine, out int startCol, out int endLine, out int endCol);
            vTextView.GetSelectedText(out string selectedText);

            return selectedText;
        }
    }
}
