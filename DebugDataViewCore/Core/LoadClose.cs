using CommunityToolkit.Mvvm.Messaging;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dataViewWindow">数据展示窗口</param>
        public void Init(DataViewWindow dataViewWindow)
        {
            _dataViewWindow = dataViewWindow;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        /// <param name="disposing">标识是否释放当前实例的资源</param>
        protected override void Dispose(bool disposing)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (disposing)
            {
                Default.Unregister<ExpressionItemChanged>(this);
                Default.Unregister<DataIntervalMove>(this);
                if (_dte != null) _dbgEvents.OnEnterBreakMode -= OnBreakMode;
                _dataViewWindow?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
