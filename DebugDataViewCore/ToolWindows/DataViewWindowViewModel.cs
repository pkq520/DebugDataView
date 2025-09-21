using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using static DebugDataViewCore.DataViewWindowModel;

namespace DebugDataViewCore
{
    public partial class DataViewWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Expression列表
        /// </summary>
        public ExpressionList ExpressionLists { get; set; } = new();

        /// <summary>
        /// 数据信息
        /// </summary>
        public ObservableCollection<DataInfoItem> DataInfoItems { get; set; } = [
            new DataInfoItem { Description = "最大值", Value = 0 },
            new DataInfoItem { Description = "最小值", Value = 0 },
            new DataInfoItem { Description = "平均值", Value = 0 },
        ];

        [RelayCommand]
        private void ExpressionListsClear()
        {
            ExpressionLists.Items.Clear();
            ExpressionLists.SelectedItem = "";

            DataInfoItems[0].Value = 0;
            DataInfoItems[1].Value = 0;
            DataInfoItems[2].Value = 0;
        }
    }
}
