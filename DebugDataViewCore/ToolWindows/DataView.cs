using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Imaging;

namespace DebugDataViewCore
{
    public class DataView : BaseToolWindow<DataView>
    {
        public override string GetTitle(int toolWindowId) => "DataView";

        public override Type PaneType => typeof(Pane);

        public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            return Task.FromResult<FrameworkElement>(new DataViewControl());
        }

        [Guid("dd42af87-f6f3-41e4-9bbe-4d52b4f29702")]
        internal class Pane : ToolkitToolWindowPane
        {
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
            }
        }
    }
}
