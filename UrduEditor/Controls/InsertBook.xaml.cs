using System.Windows.Controls;
using UrduEditor.ViewModel;

namespace UrduEditor.Controls
{
    public partial class InsertBook : UserControl
    {
        InsertBookViewModel _viewModel = new InsertBookViewModel();

        public InsertBook()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }
    }
}
