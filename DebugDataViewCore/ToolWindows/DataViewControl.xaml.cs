using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using EnvDTE;
using EnvDTE80;

namespace DebugDataViewCore
{
    public class DataViewModel
    {
        public ObservableCollection<DataRowItem> Items { get; set; } = [
            new DataRowItem { Description = "最大值", Value = 0 },
            new DataRowItem { Description = "最小值", Value = 0 },
        ];
    }

    public partial class DataRowItem : ObservableObject
    {
        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private double value;
        public double Value
        {
            get => value;
            set => SetProperty(ref this.value, value);
        }
    }

    public partial class DataViewControl : UserControl
    {
        private DataViewModel _viewModel { get; set; } = new();

        public DataViewControl()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //VS.MessageBox.Show("DataViewControl", "Button clicked");
            WpfPlot1.Plot.Clear();
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            WpfPlot1.Plot.AddScatter(dataX, dataY);
            WpfPlot1.Refresh();
        }

        public void Refresh(double[] dataY)
        {
            WpfPlot1.Plot.Clear();
            WpfPlot1.Plot.AddSignal(dataY);
            WpfPlot1.Refresh();

            _viewModel.Items[0].Value = dataY.Max();
            _viewModel.Items[1].Value = dataY.Min();
        }

        public void Clear()
        {
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
    }
}
