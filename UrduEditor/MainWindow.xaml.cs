using System.IO;
using System.Windows;
using Inshapardaz.Language.Tools;
using Microsoft.Win32;

namespace UrduEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DocumentViewModel document = new DocumentViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this.document;
        }
        
        private void OnCleanup(object sender, RoutedEventArgs e)
        {
            document.Cleanup();            
        }

        private void OnSpellCheck(object sender, RoutedEventArgs e)
        {
            document.SpellCheck();
        }

        private void OnOpen(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                document.LoadDocument(openFile.FileName);
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            document.SaveDocument();
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
