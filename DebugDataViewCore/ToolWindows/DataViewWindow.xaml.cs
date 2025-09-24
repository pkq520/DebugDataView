using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using EnvDTE;
using EnvDTE80;
using ScottPlot;
using ScottPlot.Plottable;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using static DebugDataViewCore.DataViewWindowModel;

namespace DebugDataViewCore
{
    public partial class DataViewWindow : UserControl
    {
        private DataViewWindowViewModel ViewModel { get; set; } = new();
        private SignalPlotConst<double> _signalPlotConstBasic;

        private double[] _data = [];

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
            ScottPlot_Basic.Plot.SetAxisLimitsY(0, _data.Max() * 1.1 + 1);
            //ScottPlot_Basic.Plot.XAxis.TickLabelFormat(plotAxis.ScottPlot_Spectral_Xmark);//自定义横纵坐标
            ScottPlot_Basic.Plot.RenderUnlock();
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
            ViewModel.DataInfoItems[0].Value = _data.Max();
            ViewModel.DataInfoItems[1].Value = _data.Min();
            ViewModel.DataInfoItems[2].Value = _data.Average();
            ViewModel.DataInfoItems[3].Value = _data.Length;
            ScottPlot_Basic.Refresh();
        }

        public void PlotClear()
        {
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
            
        }

        public void Close()
        {
            Default.Unregister<ItemChanged>(this);
            Default.Unregister<PlotItemChanged>(this);
        }
    }
}
