using System.Windows;
using System.Windows.Controls;

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
            VS.MessageBox.Show("DataViewControl", "Button clicked");
        }
    }
}
