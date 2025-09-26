using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DebugDataViewCore
{
    public class ItemChanged(string value) : ValueChangedMessage<string>(value);
    public class DataIntervalMove(MoveEnum value) : ValueChangedMessage<MoveEnum>(value);
    public class PlotItemChanged(PlotItemEnum value) : ValueChangedMessage<PlotItemEnum>(value);

    public enum MoveEnum
    {
        LeftMove,
        RightMove
    }

    public enum PlotItemEnum
    {
        HorizontalLock,
        HorizontalUnlock,
        VerticalLock,
        VerticalUnlock,
        PeakInfoDiaplay,
        PeakInfoUnDiaplay,
        AutoScale,
        PlotRefresh,
    }

    public partial class DataViewWindowModel
    {
        /// <summary>
        /// Expression列表
        /// </summary>
        public partial class ExpressionList : ObservableObject
        {
            [ObservableProperty]
            private string _selectedItem = "";

            [ObservableProperty]
            private ObservableCollection<string> _items = [];

            partial void OnSelectedItemChanged(string value)
            {
                WeakReferenceMessenger.Default.Send(new ItemChanged(value));
            }
        }

        /// <summary>
        /// 数据信息
        /// </summary>
        public partial class DataInfoItem : ObservableObject
        {
            [ObservableProperty]
            private string _description;

            [ObservableProperty]
            private double _value;
        }
    }
}
