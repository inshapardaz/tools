using System.Windows.Controls;
using UrduEditor.ViewModel;

namespace UrduEditor.Controls
{
    /// <summary>
    /// Interaction logic for PdfToImage.xaml
    /// </summary>
    public partial class PdfToImage : UserControl
    {
        PdfToImageViewModel _viewModel = new PdfToImageViewModel();

        public PdfToImage()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
        }
    }
}
