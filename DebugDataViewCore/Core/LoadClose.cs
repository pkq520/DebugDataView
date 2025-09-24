using CommunityToolkit.Mvvm.Messaging;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        public void Init(DataViewWindow dataViewWindow)
        {
            _dataViewWindow = dataViewWindow;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Default.Unregister<ItemChanged>(this);
                Default.Unregister<DataIntervalMove>(this);
                if (_dte != null) _dbgEvents.OnEnterBreakMode -= OnBreakMode;
                _dataViewWindow.Close();
            }
            base.Dispose(disposing);
        }
    }
}
