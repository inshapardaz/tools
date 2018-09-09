using System.Windows.Controls;
using UrduEditor.ViewModel;

namespace UrduEditor.Controls
{
    public partial class TextExtraction : UserControl
    {
        TextExtrationViewModel _viewModel = new TextExtrationViewModel(); 
        public TextExtraction()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }
    }
}
