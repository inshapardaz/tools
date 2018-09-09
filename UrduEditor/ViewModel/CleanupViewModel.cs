using System.Windows.Forms;
using System.Windows.Input;

namespace UrduEditor.ViewModel
{
    public class CleanupViewModel : ViewModelBase
    {
        private ICommand _openCommand;
        private ICommand _saveCommand;
        private ICommand _joinLinesCommand;
        private ICommand _cleanupCommand;
        private ICommand _checkupCommand;

        public ICommand OpenCommand => _openCommand ?? (_openCommand = new CommandHandler(() => Open(), true));
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(() => Save(), true));
        public ICommand JoinLinesCommand => _joinLinesCommand ?? (_joinLinesCommand = new CommandHandler(() => JoinLines(), true));
        public ICommand CleanupCommand => _cleanupCommand ?? (_cleanupCommand = new CommandHandler(() => Cleanup(), true));
        public ICommand CheckupCommand => _checkupCommand ?? (_checkupCommand = new CommandHandler(() => Checkup(), true));


        private DocumentViewModel _document = new DocumentViewModel();

        public DocumentViewModel Document
        {
            get { return _document; }
            set { _document = value; NotifyPropertyChanged();  }
        }

        private void Open()
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Document.LoadDocument(openFile.FileName);
            }
        }

        private void Save()
        {
            Document.SaveDocument();
        }

        private void JoinLines()
        {
            Document.JoinLines();
        }

        private void Cleanup()
        {
            Document.Cleanup(true);
        }

        private void Checkup()
        {
            Document.Cleanup();
        }
    }
}
