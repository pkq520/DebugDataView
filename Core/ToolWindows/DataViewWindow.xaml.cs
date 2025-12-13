using System.Windows;
using System.Windows.Controls;

namespace DebugDataView
{
    public partial class DataViewWindow
    {
        public DataViewWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
            DataViewLoaded();
        }
    }
}
