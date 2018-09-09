using System.Windows.Controls;
using UrduEditor.ViewModel;

namespace UrduEditor.Controls
{
    /// <summary>
    /// Interaction logic for ImageSplitter.xaml
    /// </summary>
    public partial class ImageSplitter : UserControl
    {
        ImageSplitterViewModel _viewModel = new ImageSplitterViewModel();

        public ImageSplitter()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
        }
    }
}
