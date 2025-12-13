using System;
using System.Drawing;
using System.Linq;

namespace DebugDataView
{
    public partial class DataViewWindow
    {
        /// <summary>
        /// 添加表达式
        /// </summary>
        /// <param name="expression">被添加的表达式</param>
        public void AddExpression(string expression)
        {
            if (!_viewModel.ExpressionLists.Items.Contains(expression))
                _viewModel.ExpressionLists.Items.Add(expression);
            _viewModel.ExpressionLists.SelectedItem = expression;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">被添加的数据</param>
        public void AddData(double[] data)
        {
            _data = data;
            int dataLength = _data.Length;
            ScottPlot_Basic.Plot.RenderLock();
            if (_signalPlotConstBasic == null || (_signalPlotConstBasic != null && _signalPlotConstBasic.PointCount != dataLength))
            {
                if (_signalPlotConstBasic != null) ScottPlot_Basic.Plot.Clear();
                _signalPlotConstBasic = ScottPlot_Basic.Plot.AddSignalConst(_data, 1, Color.CadetBlue);
                ScottPlot_Basic.Plot.XAxis.SetBoundary(0, dataLength);//设置横轴可查看范围
            }
            else _signalPlotConstBasic.Update(_data);

            _signalPlotConstBasic.FillBelow(Color.CadetBlue, Color.Transparent, 0.4);
            ScottPlot_Basic.Plot.AxisAuto();
            //ScottPlot_Basic.Plot.SetAxisLimitsY(0, _data.Max() * 1.1 + 1);
            //ScottPlot_Basic.Plot.XAxis.TickLabelFormat();//自定义横纵坐标
            ScottPlot_Basic.Plot.RenderUnlock();

            if (_viewModel.IsPeakInfoDiaplay)
                _pointPeaks = _mathService.Peaks_Calculate(_data, _peakPointNum, 10, 5);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            _viewModel.DataInfoItems[0].Value = Math.Round(_data.Max(), 3);
            _viewModel.DataInfoItems[1].Value = Math.Round(_data.Min(), 3);
            _viewModel.DataInfoItems[2].Value = Math.Round(_data.Average(), 3);
            _viewModel.DataInfoItems[3].Value = _data.Length;
            _scottPlotService.CustomPoint_Clear(ScottPlot_Basic);
            if (_viewModel.IsPeakInfoDiaplay)
                _scottPlotService.PeakPoint_Display(ScottPlot_Basic, _pointPeaks, _linearParameter);
            ScottPlot_Basic.Plot.AxisAuto();
            ScottPlot_Basic.Refresh();
        }
    }
}
