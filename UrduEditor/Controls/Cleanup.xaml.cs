using System.Windows.Controls;
using Inshapardaz.Language.Tools;
using UrduEditor.ViewModel;

namespace UrduEditor.Controls
{
    /// <summary>
    /// Interaction logic for Cleanup.xaml
    /// </summary>
    public partial class Cleanup : UserControl
    {
        CleanupViewModel _viewModel = new CleanupViewModel();
        public Cleanup()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void OnSelectedSuggession(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is Suggesstion)
                {
                    var selectedSuggession = e.AddedItems[0] as Suggesstion;
                    rtText.Focus();
                    rtText.CaretIndex = selectedSuggession.Position;
                    rtText.ScrollToEnd();
                    rtText.Select(selectedSuggession.Position, 1);
                }
                else if (e.AddedItems[0] is SpellingMistake)
                {
                    var selectedMistake = e.AddedItems[0] as SpellingMistake;
                    rtText.Focus();
                    rtText.CaretIndex = selectedMistake.StartPosition;
                    rtText.ScrollToEnd();
                    rtText.Select(selectedMistake.StartPosition, selectedMistake.EndPosition - selectedMistake.StartPosition);
                }
            }
        }
    }
}
