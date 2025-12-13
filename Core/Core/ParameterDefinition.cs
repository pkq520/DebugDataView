using System.Threading;
using DataSturctures;
using EnvDTE80;
using static DataSturctures.StatusEnum;

namespace DebugDataView
{
    public partial class DebugDataViewPackage
    {
        /// <summary>
        /// 用于获取debug相关资源
        /// </summary>
        private DTE2 _dte;
        /// <summary>
        /// debug事件集合
        /// </summary>
        private EnvDTE.DebuggerEvents _dbgEvents;
        /// <summary>
        /// 取消令牌
        /// </summary>
        private CancellationToken _cancelToken;
        /// <summary>
        /// 数据显示窗口句柄
        /// </summary>
        private DataViewWindow _dataViewWindow;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly DebugDataStructures _debugData = new();
    }
}
