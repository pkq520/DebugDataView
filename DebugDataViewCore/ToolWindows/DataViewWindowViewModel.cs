using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
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

        [ObservableProperty]
        private bool isHorizontalLock;
        [ObservableProperty]
        private bool isVerticalLock;
        [ObservableProperty]
        private bool isPeakInfoDiaplay;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                var propName = e.PropertyName;
                if (propName.Equals(nameof(IsHorizontalLock)))
                {
                    Default.Send(
                        new PlotItemChanged(IsHorizontalLock ?
                            PlotItemEnum.HorizontalLock :
                            PlotItemEnum.HorizontalUnlock));
                }
                if (propName.Equals(nameof(IsVerticalLock)))
                {
                    Default.Send(
                        new PlotItemChanged(IsVerticalLock ?
                            PlotItemEnum.VerticalLock :
                            PlotItemEnum.VerticalUnlock));
                }
                if (propName.Equals(nameof(IsPeakInfoDiaplay)))
                {
                    Default.Send(
                        new PlotItemChanged(IsPeakInfoDiaplay ?
                            PlotItemEnum.PeakInfoDiaplay :
                            PlotItemEnum.PeakInfoUnDiaplay));
                }
            }
        }

        [RelayCommand]
        private void AutoScale()
        {
            Default.Send(new PlotItemChanged(PlotItemEnum.AutoScale));
        }

        [RelayCommand]
        private void PlotRefresh()
        {
            Default.Send(new PlotItemChanged(PlotItemEnum.PlotRefresh));
        }

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
            Default.Send(new DataIntervalMove(MoveEnum.LeftMove));
        }

        [RelayCommand]
        private void RigthMove()
        {
            Default.Send(new DataIntervalMove(MoveEnum.RightMove));
        }
    }
}
