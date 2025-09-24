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
            new DataInfoItem { Description = "Max", Value = 0 },
            new DataInfoItem { Description = "Min", Value = 0 },
            new DataInfoItem { Description = "Average", Value = 0 },
            new DataInfoItem { Description = "Data length", Value = 0 },
        ];

        [ObservableProperty]
        private string _infoDiaplay = "";
        [ObservableProperty]
        private string _intervalinfoDiaplay = "(-/-)";

        //public 

        [RelayCommand]
        private void ExpressionListsClear()
        {
            ExpressionLists.Items.Clear();
            ExpressionLists.SelectedItem = "";
            for (int i = 0; i < DataInfoItems.Count; i++) DataInfoItems[i].Value = 0;
            InfoDiaplay = "";
            IntervalinfoDiaplay = "(-/-)";
        }

        [RelayCommand]
        private void LeftMove()
        {
            WeakReferenceMessenger.Default.Send(new DataIntervalMove(MoveEnum.LeftMove));
        }

        [RelayCommand]
        private void RigthMove()
        {
            WeakReferenceMessenger.Default.Send(new DataIntervalMove(MoveEnum.RightMove));
        }
    }
}
