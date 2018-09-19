using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Google.Cloud.Vision.V1;

namespace UrduEditor.ViewModel
{
    public class TextExtrationViewModel : ViewModelBase
    {
        object _lock = new object();
        private ICommand _browseInputPathCommand;
        private ICommand _browseOutputPathCommand;
        private ICommand _processCommand;

        private string _inputPath;
        private string _outputPath;
        private int _fileType;
        private bool _mergeIntoOneFile;
        private ObservableCollection<string> _output = new ObservableCollection<string>();

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

        public int FileType
        {
            get { return _fileType; }
            set { _fileType = value; NotifyPropertyChanged(); }
        }

        public bool MergeIntoOneFile
        {
            get { return _mergeIntoOneFile; }
            set { _mergeIntoOneFile = value; NotifyPropertyChanged(); }
        }


        public ObservableCollection<string> Output
        {
            get { return _output; }
            set { _output = value; NotifyPropertyChanged(); }
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

        public ICommand ProcessCommand
        {
            get
            {
                return _processCommand ?? (_processCommand = new CommandHandler(() => Process(), true));
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
        
        public void Process()
        {
            Output.Clear();
            Output.Add("INFO : Starting job");
            if (string.IsNullOrWhiteSpace(InputPath) || !Directory.Exists(InputPath))
            {
                Output.Add("ERROR : Input path doesnt exists");
                return;
            }

            if (string.IsNullOrWhiteSpace(OutputPath))
            {
                if (MergeIntoOneFile)
                {
                    OutputPath = Path.Combine(InputPath, "output.txt");
                    Output.Add($"INFO : No output path specified. Using file {OutputPath}");
                }
                else
                {
                    OutputPath = InputPath;
                    Output.Add($"INFO : No output path specified. Using same folder");
                }
            }
            
            try
            {
                EnsureOutputDestination();
                var filter = (FileType == 0) ? "*.jp*g" : "*.gif";
                var files = Directory.GetFiles(InputPath, filter);

                BlockingCollection<string> filesToProcess = new BlockingCollection<string>();

                CancellationTokenSource cts = new CancellationTokenSource();

                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 10,
                    CancellationToken = cts.Token
                };

                foreach (var file in files)
                {
                    try
                    {
                        filesToProcess.Add(file, cts.Token);
                    }
                    catch (Exception e)
                    {
                        Output.Add($"ERROR : {e.Message}");
                        Output.Add(e.StackTrace);
                    }
                }

                filesToProcess.CompleteAdding();

                Parallel.ForEach(filesToProcess.GetConsumingEnumerable(), parallelOptions, ProcessFile);
            }
            catch (Exception e)
            {
                Output.Add($"ERROR : {e.Message}");
                Output.Add(e.StackTrace);
            }

            Output.Add("INFO : Completed job");
        }

        private void ProcessFile(string filePath)
        {
            var image = Image.FromFile(filePath);
            var client = ImageAnnotatorClient.Create();
            var response = client.DetectDocumentText(image);
            if (response == null || response.Text == null)
            {
                //Output.Add($"WARNING : Unable to process file {filePath}");
                return;
            }

            var text = response.Text;
            SaveToOutput(text, filePath);
        }

        private void SaveToOutput(string text, string filePath)
        {
            if (MergeIntoOneFile)
            {
                lock (_lock)
                {
                    File.AppendAllText(OutputPath, $"{Environment.NewLine} ---------------- {filePath} ----------------- {Environment.NewLine}");
                    File.AppendAllText(OutputPath, text);
                }
            }
            else
            {
                var newfileName = Path.GetFileNameWithoutExtension(filePath);
                File.WriteAllText(Path.Combine(OutputPath, $"{newfileName}.txt"), text);
            }
        }

        private void EnsureOutputDestination()
        {
            if (InputPath == OutputPath) return;
            if (MergeIntoOneFile)
            {
                if (File.Exists(OutputPath))
                {
                    File.Delete(OutputPath);
                }

                File.CreateText(OutputPath);
            }
            else
            {
                if (Directory.Exists(OutputPath))
                {
                    Directory.Delete(OutputPath);
                }

                Directory.CreateDirectory(OutputPath);
            }
        }
    }
}
