using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using static DebugDataViewCore.DataViewWindowModel;

namespace DebugDataViewCore
{
    public partial class DataViewWindowViewModel : ObservableObject
    {
        /// <summary>
        /// 表达式列表
        /// </summary>
        public ExpressionList ExpressionLists { get; set; } = new();

        /// <summary>
        /// 数据信息列表
        /// </summary>
        public ObservableCollection<DataInfoItem> DataInfoItems { get; set; } = [
            new (description: "Max", value: 0),
            new (description: "Min", value: 0),
            new (description: "Average", value: 0),
            new (description: "Data length", value: 0),
        ];

        /// <summary>
        /// 运行信息
        /// </summary>
        [ObservableProperty]
        private string _runInfoDiaplay = "";
        /// <summary>
        /// 数据区段信息
        /// </summary>
        [ObservableProperty]
        private string _dataIntervalInfoDiaplay = "(-/-)";
        /// <summary>
        /// 标识横轴是否锁定
        /// </summary>
        [ObservableProperty]
        private bool _isHorizontalLock;
        /// <summary>
        /// 标识纵轴是否锁定
        /// </summary>
        [ObservableProperty]
        private bool _isVerticalLock;
        /// <summary>
        /// 标识是否显示峰信息
        /// </summary>
        [ObservableProperty]
        private bool _isPeakInfoDiaplay;

        /// <summary>
        /// 当横轴锁定状态切换时
        /// </summary>
        /// <param name="value">切换后的数值</param>
        partial void OnIsHorizontalLockChanged(bool value)
        {
            Default.Send(
                new ToolBarTrayChanged(value ?
                    ToolBarTrayEnum.HorizontalLock :
                    ToolBarTrayEnum.HorizontalUnlock));
        }

        /// <summary>
        /// 当纵轴锁定状态切换时
        /// </summary>
        /// <param name="value">切换后的数值</param>
        partial void OnIsVerticalLockChanged(bool value)
        {
            Default.Send(
                new ToolBarTrayChanged(value ?
                    ToolBarTrayEnum.VerticalLock :
                    ToolBarTrayEnum.VerticalUnlock));
        }

        /// <summary>
        /// 当峰值信息显示状态切换时
        /// </summary>
        /// <param name="value">切换后的数值</param>
        partial void OnIsPeakInfoDiaplayChanged(bool value)
        {
            Default.Send(
                new ToolBarTrayChanged(value ?
                    ToolBarTrayEnum.PeakInfoDiaplay :
                    ToolBarTrayEnum.PeakInfoUnDiaplay));
        }

        /// <summary>
        /// 自动缩放
        /// </summary>
        [RelayCommand]
        private void AutoScale()
        {
            Default.Send(new ToolBarTrayChanged(ToolBarTrayEnum.AutoScale));
        }

        /// <summary>
        /// 谱图刷新
        /// </summary>
        [RelayCommand]
        private void PlotRefresh()
        {
            Default.Send(new ToolBarTrayChanged(ToolBarTrayEnum.PlotRefresh));
        }

        /// <summary>
        /// 表达式列表清空
        /// </summary>
        [RelayCommand]
        private void ExpressionListsClear()
        {
            ExpressionLists.Items.Clear();
            ExpressionLists.SelectedItem = "";
            foreach (DataInfoItem item in DataInfoItems) item.Value = 0;
            RunInfoDiaplay = "";
            DataIntervalInfoDiaplay = "(-/-)";
        }

        /// <summary>
        /// 数据区间左移
        /// </summary>
        [RelayCommand]
        private void LeftMove()
        {
            Default.Send(new DataIntervalMove(MoveEnum.LeftMove));
        }

        /// <summary>
        /// 数据区间右移
        /// </summary>
        [RelayCommand]
        private void RigthMove()
        {
            Default.Send(new DataIntervalMove(MoveEnum.RightMove));
        }
    }
}
