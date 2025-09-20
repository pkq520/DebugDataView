using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EnvDTE;
using EnvDTE80;

namespace DebugDataViewCore
{
    public partial class DataViewControl : UserControl
    {
        public DataViewControl()
        {
            InitializeComponent();
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
        }

        public void Clear()
        {
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
    }
}
