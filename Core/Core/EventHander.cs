using System.Threading.Tasks;
using EnvDTE;

namespace DebugDataView
{
    public partial class DebugDataViewPackage
    {
        /// <summary>
        /// Debug模式时执行的事件
        /// </summary>
        /// <param name="reason">debug原因</param>
        /// <param name="executionAction">debug执行操作</param>
        private void OnBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            Task.Run(async () => await DataViewRefreshAsync(), _cancelToken);
        }
    }
}
