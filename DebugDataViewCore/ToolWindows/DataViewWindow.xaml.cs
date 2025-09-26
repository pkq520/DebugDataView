using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DebugDataViewCore.Support;
using EnvDTE;
using EnvDTE80;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Win32;
using ScottPlot;
using ScottPlot.Plottable;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using static DebugDataViewCore.DataViewWindowModel;
using static DebugDataViewCore.Support.DataStructures;

namespace DebugDataViewCore
{
    public partial class DataViewWindow : UserControl
    {
        private DataViewWindowViewModel ViewModel { get; set; } = new();
        private SignalPlotConst<double> _signalPlotConstBasic;
        private readonly IMathService _mathService = new MathService();
        private readonly IScottPlotService _scottPlotService = new ScottPlotService();
        private readonly IFileExportService _fileExportService = new FileExportService();
        private readonly SaveFileDialog saveFileDialog = new();
        private double[] _data = [];
        private readonly int _peakPointNum = 5;
        private readonly int _customPointNum = 5;

        private ILinearParameter _linearParameter = new LinearParameterClass(1, 0);
        private List<PointD> _pointPeaks = new();

        public DataViewWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;

            ScottPlot_Basic.Configuration.LockVerticalAxis = true;
            ScottPlot_Basic.Plot.XAxis.SetZoomInLimit(20);//限制放大时横轴的最小间距
            ScottPlot_Basic.Plot.Layout(left: 90, right: 60, bottom: 70, top: 45, padding: -15);
            ScottPlot_Basic.Plot.XAxis.TickLabelStyle(fontSize: 14);
            ScottPlot_Basic.Plot.YAxis.TickLabelStyle(fontSize: 14);
            ScottPlot_Basic.Refresh();
            ScottPlot_Basic.Configuration.RightClickDragZoom = false;
            ScottPlot_Basic.Configuration.RightClickDragZoomFromMouseDown = false;
            ScottPlot_Basic.Configuration.DoubleClickBenchmark = false;
            //ScottPlot_Basic.RightClicked -= ScottPlot_Basic.DefaultRightClickEvent;

            Default.Register<ItemChanged>(this, ItemChangedReceiver);
            Default.Register<PlotItemChanged>(this, PlotItemChangedReceiver);

            _scottPlotService.Init(_peakPointNum, _customPointNum);
            _scottPlotService.CustomPoint_Init(ScottPlot_Basic);
            _scottPlotService.PeakPoint_Init(ScottPlot_Basic);
        }

        public void AddExpression(string expressionString)
        {
            if (!ViewModel.ExpressionLists.Items.Contains(expressionString))
                ViewModel.ExpressionLists.Items.Add(expressionString);
            ViewModel.ExpressionLists.SelectedItem = expressionString;
        }

        public void AddPlotData(double[] data)
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

            if (ViewModel.IsPeakInfoDiaplay) 
                _pointPeaks = _mathService.Peaks_Calculate(_signalPlotConstBasic.Ys, _peakPointNum, 10, 5);
        }

        public void ShowInfo(string info)
        {
            ViewModel.InfoDiaplay = info;
        }

        public void ShowIntervalinfo(string info)
        {
            ViewModel.IntervalinfoDiaplay = info;
        }

        public void Refresh()
        {
            ViewModel.DataInfoItems[0].Value = Math.Round(_data.Max(), 3);
            ViewModel.DataInfoItems[1].Value = Math.Round(_data.Min(), 3);
            ViewModel.DataInfoItems[2].Value = Math.Round(_data.Average(), 3);
            ViewModel.DataInfoItems[3].Value = _data.Length;
            _scottPlotService.CustomPoint_Clear(ScottPlot_Basic);
            if (ViewModel.IsPeakInfoDiaplay)
                _scottPlotService.PeakPoint_Display(ScottPlot_Basic, _pointPeaks, _linearParameter);
            ScottPlot_Basic.Plot.AxisAuto();
            ScottPlot_Basic.Refresh();
        }

        public void PlotClear()
        {
            _scottPlotService.PeakPoint_Clear(ScottPlot_Basic);
            _scottPlotService.CustomPoint_Clear(ScottPlot_Basic);
            _signalPlotConstBasic = null;
            ScottPlot_Basic.Plot.Clear();
            ScottPlot_Basic.Refresh();
        }

        private void ItemChangedReceiver(object recipient, ItemChanged handler)
        {
            if (handler.Value == "") PlotClear();
        }

        private void PlotItemChangedReceiver(object recipient, PlotItemChanged handler)
        {
            switch (handler.Value)
            {
                case PlotItemEnum.HorizontalLock: 
                    ScottPlot_Basic.Configuration.LockHorizontalAxis = true;
                    break;
                case PlotItemEnum.HorizontalUnlock:
                    ScottPlot_Basic.Configuration.LockHorizontalAxis = false;
                    break;
                case PlotItemEnum.VerticalLock:
                    ScottPlot_Basic.Configuration.LockVerticalAxis = true;
                    break;
                case PlotItemEnum.VerticalUnlock:
                    ScottPlot_Basic.Configuration.LockVerticalAxis = false;
                    break;
                case PlotItemEnum.PeakInfoDiaplay:
                    {
                        List<PointD> pointPeaks = _mathService.Peaks_Calculate(_signalPlotConstBasic.Ys, _peakPointNum, 10, 5);
                        _scottPlotService.PeakPoint_Display(ScottPlot_Basic, pointPeaks, _linearParameter);
                        ScottPlot_Basic.Refresh();
                    }
                    break;
                case PlotItemEnum.PeakInfoUnDiaplay:
                    {
                        _scottPlotService.PeakPoint_Clear(ScottPlot_Basic);
                        ScottPlot_Basic.Refresh();
                    }
                    break;
                case PlotItemEnum.AutoScale:
                    {
                        ScottPlot_Basic.Plot.AxisAuto();
                        ScottPlot_Basic.Refresh();
                    }
                break;
                case PlotItemEnum.PlotRefresh:
                    ScottPlot_Basic.Refresh();
                    break;
            }
        }

        public void Close()
        {
            Default.Unregister<ItemChanged>(this);
            Default.Unregister<PlotItemChanged>(this);
        }

        private void ScottPlot_Basic_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_signalPlotConstBasic != null &&
                e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                (double mouseCoordX, double _) = ScottPlot_Basic.GetMouseCoordinates();
                (double pointX, double pointY, int _) = _signalPlotConstBasic.GetPointNearestX(mouseCoordX);

                if (_scottPlotService.CustomPointRepeat_Check(pointX, pointY) == false)//如果没有重复的点
                {
                    int vacancyIndex = _scottPlotService.CustomPointVacancy_Check();
                    if (vacancyIndex != -1) _scottPlotService.CustomPoint_Add(ScottPlot_Basic, vacancyIndex, pointX, pointY, _linearParameter);
                    else _scottPlotService.CustomPoint_MoveAdd(pointX, pointY, _linearParameter);
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (_signalPlotConstBasic != null && _signalPlotConstBasic.Ys.Length != 0)
            {
                if (sender.Equals(ExportToFile))
                {
                    DateTime dateTime = DateTime.Now;
                    saveFileDialog.Title = "请选择存储目录";
                    saveFileDialog.FileName =
                        $"{ViewModel.ExpressionLists.SelectedItem} " +
                        $"{ViewModel.IntervalinfoDiaplay} " +
                        $"{dateTime:yyyy-MM-dd HH-mm-ss-fff}";
                    saveFileDialog.Filter = "数据文件(*.csv)|*.csv";
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        DebugDataStructures debugData = new DebugDataStructures()
                        {
                            Information = "",
                            SaveTime = dateTime,
                            Data = _data
                        };
                        _fileExportService.FileCsv_Export(saveFileDialog.FileName, debugData);
                    }
                }
                if (sender.Equals(ExportToPicture))
                {
                    DateTime dateTime = DateTime.Now;
                    saveFileDialog.Title = "请选择存储目录";
                    saveFileDialog.FileName =
                        $"{ViewModel.ExpressionLists.SelectedItem} " +
                        $"{ViewModel.IntervalinfoDiaplay} " +
                        $"{dateTime:yyyy-MM-dd HH-mm-ss-fff}";
                    saveFileDialog.Filter = 
                        "PNG(*.PNG)|*.PNG|JPEG(*.JPEG)|*.JPEG|JPG(*.JPG)|*.JPG|" +
                        "TIF(*.TIF)|*.TIF|TIFF(*.TIFF)|*.TIFF|BMP(*.BMP)|*.BMP";
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == true)
                        ScottPlot_Basic.Plot.SaveFig(saveFileDialog.FileName);
                }
                if (sender.Equals(ExportToClipboard))
                    Clipboard.SetImage(WpfPlot.BmpImageFromBmp(ScottPlot_Basic.Plot.Render()));
            }
        }
    }
}
