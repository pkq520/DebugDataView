using System.Collections.Generic;
using DataSturctures;
using DebugDataView.Support;
using FunctionalService;
using Microsoft.Win32;
using ScottPlot.Plottable;
using static DataSturctures.Extensions.StructExtensions;

namespace DebugDataView
{
    public partial class DataViewWindow
    {
        /// <summary>
        /// ViewModel实例化
        /// </summary>
        private DataViewWindowViewModel _viewModel { get; set; } = new();
        /// <summary>
        /// 图表句柄
        /// </summary>
        private SignalPlotConst<double> _signalPlotConstBasic;
        /// <summary>
        /// 数学服务
        /// </summary>
        private readonly IMathService _mathService = new MathService();
        /// <summary>
        /// 图表服务
        /// </summary>
        private readonly IScottPlotService _scottPlotService = new ScottPlotService();
        /// <summary>
        /// 文件导出服务
        /// </summary>
        private readonly IFileExportService _fileExportService = new FileExportService();
        /// <summary>
        /// 文件存储窗口句柄
        /// </summary>
        private readonly SaveFileDialog _saveFileDialog = new();
        /// <summary>
        /// 显示的数据
        /// </summary>
        private double[] _data = [];
        /// <summary>
        /// 峰点个数上限
        /// </summary>
        private readonly int _peakPointNum = 5;
        /// <summary>
        /// 自定义点个数上限
        /// </summary>
        private readonly int _customPointNum = 5;

        /// <summary>
        /// 横轴映射
        /// </summary>
        private readonly ILinearParameter _linearParameter = new LinearParameter(1, 0);

        /// <summary>
        /// 峰点信息
        /// </summary>
        private List<PointD> _pointPeaks = [];
    }
}
