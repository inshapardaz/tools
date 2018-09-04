using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Inshapardaz.Language.Tools;

namespace UrduEditor
{
    public class DocumentViewModel : INotifyPropertyChanged
    {
        private string _fileName;

        private string _content;

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadDocument(string fileName)
        {
            _fileName = fileName;
            Content = File.ReadAllText(fileName, Encoding.UTF8);
        }

        public void SaveDocument()
        {
            File.WriteAllText(_fileName, Content);
        }

        internal void SpellCheck()
        {
            var mistakes = new SpellChecker().CheckSpellings(Content);
        }

        internal void Cleanup()
        {
            var result = new Cleanup().Clean(Content);
            Content = result;
        }

        public string Content {
            get => _content;
            set
            {
                _content = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Suggesstion> CleanupSuggesstions { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
