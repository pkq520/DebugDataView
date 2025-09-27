using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace DebugDataViewCore
{
    public partial class DataViewWindow
    {
        /// <summary>
        /// 窗口装载时触发的事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">相关信息</param>
        private void DataView_Loaded(object sender, RoutedEventArgs e)
        {
            ScottPlot_Basic.Plot.XAxis.SetZoomInLimit(20);//限制放大时横轴的最小间距
            ScottPlot_Basic.Plot.Layout(left: 90, right: 60, bottom: 70, top: 45, padding: -15);
            ScottPlot_Basic.Plot.XAxis.TickLabelStyle(fontSize: 14);
            ScottPlot_Basic.Plot.YAxis.TickLabelStyle(fontSize: 14);
            ScottPlot_Basic.Refresh();
            ScottPlot_Basic.Configuration.RightClickDragZoom = false;
            ScottPlot_Basic.Configuration.RightClickDragZoomFromMouseDown = false;
            ScottPlot_Basic.Configuration.DoubleClickBenchmark = false;
            ScottPlot_Basic.RightClicked -= ScottPlot_Basic.DefaultRightClickEvent;

            Default.Register<ExpressionItemChanged>(this, ExpressionItemChangedReceiver);
            Default.Register<ToolBarTrayChanged>(this, ToolBarTrayChangedReceiver);

            _scottPlotService.Init(_peakPointNum, _customPointNum);
            _scottPlotService.CustomPoint_Init(ScottPlot_Basic);
            _scottPlotService.PeakPoint_Init(ScottPlot_Basic);
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            Default.Unregister<ExpressionItemChanged>(this);
            Default.Unregister<ToolBarTrayChanged>(this);
        }

        /// <summary>
        /// 选中表达式变化而触发的事件
        /// </summary>
        /// <param name="recipient">发送方</param>
        /// <param name="handler">传递的信息</param>
        private void ExpressionItemChangedReceiver(object recipient, ExpressionItemChanged handler)
        {
            if (handler.Value == "") PlotClear();
        }

        /// <summary>
        /// 图表显示清空
        /// </summary>
        public void PlotClear()
        {
            _scottPlotService.PeakPoint_Clear(ScottPlot_Basic);
            _scottPlotService.CustomPoint_Clear(ScottPlot_Basic);
            _data = null;
            ScottPlot_Basic.Plot.Clear();
            ScottPlot_Basic.Refresh();
        }
    }
}
