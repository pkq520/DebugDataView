using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EnvDTE;
using EnvDTE80;
using static DebugDataViewCore.DataViewWindowModel;

namespace DebugDataViewCore
{
    public partial class DataViewControl : UserControl
    {
        private DataViewWindowViewModel _viewModel { get; set; } = new();

        public DataViewControl()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        public void AddExpression(string expressionString)
        {
            if (!_viewModel.ExpressionLists.Items.Contains(expressionString))
                _viewModel.ExpressionLists.Items.Add(expressionString);
            _viewModel.ExpressionLists.SelectedItem = expressionString;
        }

        public void Refresh(double[] data)
        {
            _viewModel.DataInfoItems[0].Value = data.Max();
            _viewModel.DataInfoItems[1].Value = data.Min();
            _viewModel.DataInfoItems[2].Value = data.Average();

            WpfPlot1.Plot.Clear();
            WpfPlot1.Plot.AddSignal(data);
            WpfPlot1.Refresh();
        }

        public void PlotClear()
        {
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
    }
}
