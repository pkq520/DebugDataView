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
    public class SelectedItemChangedMessage(string value) : ValueChangedMessage<string>(value) { }

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
                WeakReferenceMessenger.Default.Send(new SelectedItemChangedMessage(value));
            }
        }

        /// <summary>
        /// 数据信息
        /// </summary>
        public partial class DataInfoItem : ObservableObject
        {
            [ObservableProperty]
            private string description;

            [ObservableProperty]
            private double value;
        }
    }
}
