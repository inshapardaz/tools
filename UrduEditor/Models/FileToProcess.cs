using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UrduEditor.Models
{
    public class FileToProcess : INotifyPropertyChanged
    {
        private string _path;
        private string _fileName;
        private bool _selected;

        public event PropertyChangedEventHandler PropertyChanged;

        public FileToProcess(string path)
        {
            Path = path;
            FileName = System.IO.Path.GetFileName(path);
        }

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                NotifyPropertyChanged();
            }
        }

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                NotifyPropertyChanged();
            }
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
