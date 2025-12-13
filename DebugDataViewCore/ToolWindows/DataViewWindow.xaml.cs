using System.Windows;
using System.Windows.Controls;

namespace DebugDataViewCore
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
