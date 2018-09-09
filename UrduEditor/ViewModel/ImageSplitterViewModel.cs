using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UrduEditor.Models;

namespace UrduEditor.ViewModel
{
    public class ImageSplitterViewModel : ViewModelBase
    {
        private FileToProcess _selectedFile;
        private BitmapImage _selectedImage;

        private ICommand _browseInputPathCommand;
        private ICommand _browseOutputPathCommand;
        private ICommand _loadImagesCommand;
        private ICommand _processCommand;

        private ICommand _selectAllCommand;
        private ICommand _unselectAllCommand;

        private string _inputPath;
        private string _outputPath;
        private bool _rtl;


        private ObservableCollection<FileToProcess> _files = new ObservableCollection<FileToProcess>();

        public ObservableCollection<FileToProcess> Files
        {
            get { return _files; }
            set { _files = value; NotifyPropertyChanged();
            }
        }

        public string InputPath
        {
            get { return _inputPath; }
            set { _inputPath = value; NotifyPropertyChanged(); }
        }

        public string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; NotifyPropertyChanged(); }
        }

        public bool RightToRLeft
        {
            get { return _rtl; }
            set { _rtl = value; NotifyPropertyChanged(); }
        }

        public FileToProcess SelectedFile
        {
            get { return _selectedFile; }
            set {
                _selectedFile = value;
                NotifyPropertyChanged();
                if (_selectedFile != null)
                {
                    SelectedImage = new BitmapImage(new Uri(_selectedFile.Path));
                }
                else
                {
                    SelectedImage = null;
                }
            }
        }

        public BitmapImage SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; NotifyPropertyChanged(); }
        }


        public ICommand BrowseInputPathCommand
        {
            get
            {
                return _browseInputPathCommand ?? (_browseInputPathCommand = new CommandHandler(() => SelectInputFolder(), true));
            }
        }
        public ICommand BrowseOutputPathCommand
        {
            get
            {
                return _browseOutputPathCommand ?? (_browseOutputPathCommand = new CommandHandler(() => SelectOutputFolder(), true));
            }
        }

        public ICommand LoadImagesCommand
        {
            get
            {
                return _loadImagesCommand ?? (_loadImagesCommand = new CommandHandler(() => LoadImages(), true));
            }
        }

        public ICommand ProcessCommand
        {
            get
            {
                return _processCommand ?? (_processCommand = new CommandHandler(() => Process(), true));
            }
        }

        public ICommand SelectAllCommand
        {
            get
            {
                return _selectAllCommand ?? (_selectAllCommand = new CommandHandler(() => SelectAll(), true));
            }
        }

        public ICommand UnselectAllCommand
        {
            get
            {
                return _unselectAllCommand ?? (_unselectAllCommand = new CommandHandler(() => UnselectAll(), true));
            }
        }

        public void SelectInputFolder()
        {
            using (var folderSelection = new FolderBrowserDialog())
            {
                if (folderSelection.ShowDialog() == DialogResult.OK)
                {
                    InputPath = folderSelection.SelectedPath;
                }
            }
        }

        public void SelectOutputFolder()
        {
            using (var folderSelection = new FolderBrowserDialog())
            {
                if (folderSelection.ShowDialog() == DialogResult.OK)
                {
                    OutputPath = folderSelection.SelectedPath;
                }
            }
        }

        public void LoadImages()
        {
            if (!string.IsNullOrWhiteSpace(InputPath))
            {
                try
                {
                    var files = Directory.GetFiles(InputPath, "*.jp*g");
                    Files.Clear();
                    foreach (var file in files)
                    {
                        Files.Add(new FileToProcess(file));
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to read directory/files");
                }
            }
            
        }

        public void SelectAll()
        {
            foreach (var file in Files)
            {
                file.Selected = true;
            }
        }

        public void UnselectAll()
        {
            foreach (var file in Files)
            {
                file.Selected = false;
            }
        }

        public void Process()
        {
            int fileIndex = 0;
            int count = 0;
            foreach (var item in Files)
            {
                count++;
                if (!item.Selected)
                {
                    fileIndex++;
                    File.Copy(item.Path, CreateDestinationFile(item.Path, fileIndex));
                    continue;
                }

                var filePath = item.Path;
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var extension = Path.GetExtension(filePath);

                string firstFileName, secondFileName;

                if (!RightToRLeft)
                {
                    fileIndex++;
                    firstFileName = CreateDestinationFile(filePath, fileIndex);
                    fileIndex++;
                    secondFileName = CreateDestinationFile(filePath, fileIndex);
                }
                else
                {
                    fileIndex++;
                    secondFileName = CreateDestinationFile(filePath, fileIndex);
                    fileIndex++;
                    firstFileName = CreateDestinationFile(filePath, fileIndex);
                }

                var originalImage = new Bitmap(filePath);
                var rect = new Rectangle(0, 0, originalImage.Width / 2, originalImage.Height);
                var firstHalf = originalImage.Clone(rect, originalImage.PixelFormat);
                firstHalf.Save(firstFileName);
                rect = new Rectangle(originalImage.Width / 2, 0, originalImage.Width / 2, originalImage.Height);
                var secondHalf = originalImage.Clone(rect, originalImage.PixelFormat);
                secondHalf.Save(secondFileName);
            }
        }

        private string CreateDestinationFile(string path, int fileIndex)
        {
            var fileName = new FileInfo(path).Directory.Name;
            var extension = Path.GetExtension(path);
            return Path.Combine(OutputPath, $"{fileName}-{fileIndex}{extension}");
        }

    }
}
