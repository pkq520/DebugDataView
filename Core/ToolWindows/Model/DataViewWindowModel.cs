using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DebugDataView
{
    /// <summary>
    /// 表达式切换事件
    /// </summary>
    /// <param name="value">发送的数据</param>
    public class ExpressionItemChanged(string value) : ValueChangedMessage<string>(value);
    /// <summary>
    /// 数据区段移动事件
    /// </summary>
    /// <param name="value">发送的数据</param>
    public class DataIntervalMove(MoveEnum value) : ValueChangedMessage<MoveEnum>(value);
    /// <summary>
    /// 图表工具托盘切换事件
    /// </summary>
    /// <param name="value"></param>
    public class ToolBarTrayChanged(ToolBarTrayEnum value) : ValueChangedMessage<ToolBarTrayEnum>(value);

    public enum MoveEnum
    {
        /// <summary>
        /// 左移
        /// </summary>
        LeftMove,
        /// <summary>
        /// 右移
        /// </summary>
        RightMove
    }

    public enum ToolBarTrayEnum
    {
        /// <summary>
        /// 横轴锁定
        /// </summary>
        HorizontalLock,
        /// <summary>
        /// 横轴解锁
        /// </summary>
        HorizontalUnlock,
        /// <summary>
        /// 纵轴锁定
        /// </summary>
        VerticalLock,
        /// <summary>
        /// 纵轴解锁
        /// </summary>
        VerticalUnlock,
        /// <summary>
        /// 峰信息展示
        /// </summary>
        PeakInfoDiaplay,
        /// <summary>
        /// 峰信息不展示
        /// </summary>
        PeakInfoUnDiaplay,
        /// <summary>
        /// 自动缩放
        /// </summary>
        AutoScale,
        /// <summary>
        /// 图表刷新
        /// </summary>
        PlotRefresh,
    }

    /// <summary>
    /// Model
    /// </summary>
    public partial class DataViewWindowModel
    {
        /// <summary>
        /// 表达式列表
        /// </summary>
        public partial class ExpressionList : ObservableObject
        {
            /// <summary>
            /// 当前选中的表达式
            /// </summary>
            [ObservableProperty]
            private string _selectedItem = "";

            /// <summary>
            /// 表达式列表
            /// </summary>
            [ObservableProperty]
            private ObservableCollection<string> _items = [];

            /// <summary>
            /// 当选中表达式切换时
            /// </summary>
            /// <param name="value">切换后的数值</param>
            partial void OnSelectedItemChanged(string value)
            {
                WeakReferenceMessenger.Default.Send(new ExpressionItemChanged(value));
            }
        }

        /// <summary>
        /// 数据信息
        /// </summary>
        public partial class DataInfoItem : ObservableObject
        {
            /// <summary>
            /// 条目名称
            /// </summary>
            [ObservableProperty]
            private string _description;
            /// <summary>
            /// 条目数值
            /// </summary>
            [ObservableProperty]
            private double _value;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="description">条目名称</param>
            /// <param name="value">条目数值</param>
            public DataInfoItem(string description, double value)
            {
                Description = description;
                Value = value;
            }
        }
    }
}
