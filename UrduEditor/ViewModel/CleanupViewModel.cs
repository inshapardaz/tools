using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace UrduEditor.ViewModel
{
    public class CleanupViewModel : ViewModelBase
    {
        private ICommand _openCommand;
        private ICommand _openFolderCommand;
        private ICommand _saveCommand;
        private ICommand _joinLinesCommand;
        private ICommand _cleanupCommand;
        private ICommand _cleanupFolderCommand;
        private ICommand _checkupCommand;

        private string _inputFolder;

        public string InputFolder
        {
            get { return _inputFolder; }
            set { _inputFolder = value; NotifyPropertyChanged(); }
        }

        public ICommand OpenCommand => _openCommand ?? (_openCommand = new CommandHandler(() => Open(), true));
        public ICommand OpenFolderCommand => _openFolderCommand ?? (_openFolderCommand = new CommandHandler(() => OpenFolder(), true));

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(() => Save(), true));
        public ICommand JoinLinesCommand => _joinLinesCommand ?? (_joinLinesCommand = new CommandHandler(() => JoinLines(), true));
        public ICommand CleanupCommand => _cleanupCommand ?? (_cleanupCommand = new CommandHandler(() => Cleanup(), true));
        public ICommand CleanupFolderCommand => _cleanupFolderCommand ?? (_cleanupFolderCommand = new CommandHandler(() => CleanupFolder(), true));
        public ICommand CheckupCommand => _checkupCommand ?? (_checkupCommand = new CommandHandler(() => Checkup(), true));


        private DocumentViewModel _document = new DocumentViewModel();

        public DocumentViewModel Document
        {
            get { return _document; }
            set { _document = value; NotifyPropertyChanged();  }
        }

        private void OpenFolder()
        {
            using (var folderSelection = new FolderBrowserDialog())
            {
                if (folderSelection.ShowDialog() == DialogResult.OK)
                {
                    InputFolder = folderSelection.SelectedPath;
                }
            }
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


        private void CleanupFolder()
        {
            if (string.IsNullOrWhiteSpace(InputFolder) || !Directory.Exists(InputFolder))
            {
                MessageBox.Show("Invalid folder. Please provide a valid input folder");
            }

            var files = Directory.GetFiles(InputFolder, "*.txt");
            foreach (var file in files)
            {
                var doc = new DocumentViewModel();
                doc.LoadDocument(file);
                doc.JoinLines();
                doc.Cleanup(true);
                doc.SaveDocument();
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
