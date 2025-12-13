using System;
using System.Collections.Generic;
using System.Windows;
using DataSturctures;
using ScottPlot;
using static DataSturctures.Extensions.StructExtensions;

namespace DebugDataViewCore
{
    public partial class DataViewWindow
    {
        /// <summary>
        /// 图表工具托盘变化触发的事件
        /// </summary>
        /// <param name="recipient">发送者</param>
        /// <param name="handler">发送的信息</param>
        private void ToolBarTrayChangedReceiver(object recipient, ToolBarTrayChanged handler)
        {
            switch (handler.Value)
            {
                case ToolBarTrayEnum.HorizontalLock:
                    ScottPlot_Basic.Configuration.LockHorizontalAxis = true;
                    break;
                case ToolBarTrayEnum.HorizontalUnlock:
                    ScottPlot_Basic.Configuration.LockHorizontalAxis = false;
                    break;
                case ToolBarTrayEnum.VerticalLock:
                    ScottPlot_Basic.Configuration.LockVerticalAxis = true;
                    break;
                case ToolBarTrayEnum.VerticalUnlock:
                    ScottPlot_Basic.Configuration.LockVerticalAxis = false;
                    break;
                case ToolBarTrayEnum.PeakInfoDiaplay:
                    {
                        List<PointD> pointPeaks = _mathService.Peaks_Calculate(_data, _peakPointNum, 10, 5);
                        _scottPlotService.PeakPoint_Display(ScottPlot_Basic, pointPeaks, _linearParameter);
                        ScottPlot_Basic.Refresh();
                    }
                    break;
                case ToolBarTrayEnum.PeakInfoUnDiaplay:
                    {
                        _scottPlotService.PeakPoint_Clear(ScottPlot_Basic);
                        ScottPlot_Basic.Refresh();
                    }
                    break;
                case ToolBarTrayEnum.AutoScale:
                    {
                        ScottPlot_Basic.Plot.AxisAuto();
                        ScottPlot_Basic.Refresh();
                    }
                    break;
                case ToolBarTrayEnum.PlotRefresh:
                    ScottPlot_Basic.Refresh();
                    break;
            }
        }

        /// <summary>
        /// 数据导出触发的事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">发送的信息</param>
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (_data != null && _data.Length != 0)
            {
                if (sender.Equals(ExportToFile))
                {
                    DateTime dateTime = DateTime.Now;
                    _saveFileDialog.Title = "请选择存储目录";
                    _saveFileDialog.FileName =
                        $"{_viewModel.ExpressionLists.SelectedItem} " +
                        $"{_viewModel.DataIntervalInfoDiaplay} " +
                        $"{dateTime:yyyy-MM-dd HH-mm-ss-fff}";
                    _saveFileDialog.Filter = "数据文件(*.csv)|*.csv";
                    _saveFileDialog.RestoreDirectory = true;
                    if (_saveFileDialog.ShowDialog() == true)
                    {
                        var debugData = new DebugDataSaveStructures()
                        {
                            Information = "",
                            SaveTime = dateTime,
                            Data = _data
                        };
                        _fileExportService.FileCsv_Export(_saveFileDialog.FileName, debugData);
                    }
                }
                if (sender.Equals(ExportToPicture))
                {
                    DateTime dateTime = DateTime.Now;
                    _saveFileDialog.Title = "请选择存储目录";
                    _saveFileDialog.FileName =
                        $"{_viewModel.ExpressionLists.SelectedItem} " +
                        $"{_viewModel.DataIntervalInfoDiaplay} " +
                        $"{dateTime:yyyy-MM-dd HH-mm-ss-fff}";
                    _saveFileDialog.Filter =
                        "PNG(*.PNG)|*.PNG|JPEG(*.JPEG)|*.JPEG|JPG(*.JPG)|*.JPG|" +
                        "TIF(*.TIF)|*.TIF|TIFF(*.TIFF)|*.TIFF|BMP(*.BMP)|*.BMP";
                    _saveFileDialog.RestoreDirectory = true;
                    if (_saveFileDialog.ShowDialog() == true)
                        ScottPlot_Basic.Plot.SaveFig(_saveFileDialog.FileName);
                }
                if (sender.Equals(ExportToClipboard))
                    Clipboard.SetImage(WpfPlot.BmpImageFromBmp(ScottPlot_Basic.Plot.Render()));
            }
        }
    }
}
