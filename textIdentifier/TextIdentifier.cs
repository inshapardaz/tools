using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;

namespace textIdentifier
{
    public class TextIdentifier
    {
        object _lock = new object();

        private string InputPath { get; set; }
        private string OutputPath { get; set; }
        private bool MergeIntoOneFile { get; set; }
        private string FileType { get; set; }

        public TextIdentifier(string inputPath, string outputPath, bool mergeIntoOneFile = false, string fileType = "")
        {
            InputPath = inputPath;
            OutputPath = outputPath;
            MergeIntoOneFile = mergeIntoOneFile;
            FileType = fileType;
        }
        public void Process()
        {
            Console.WriteLine($"INFO : Starting job for folder {InputPath}");
            if (string.IsNullOrWhiteSpace(InputPath) || !Directory.Exists(InputPath))
            {
                Console.WriteLine("ERROR : Input path doesnt exists");
                return;
            }

            if (string.IsNullOrWhiteSpace(OutputPath))
            {
                if (MergeIntoOneFile)
                {
                    OutputPath = Path.Combine(InputPath, "output.txt");
                    Console.WriteLine($"INFO : No output path specified. Using file {OutputPath}");
                }
                else
                {
                    OutputPath = InputPath;
                    Console.WriteLine($"INFO : No output path specified. Using same folder");
                }
            }
            
            try
            {
                EnsureOutputDestination();
                var filter = (string.IsNullOrWhiteSpace(FileType) ? "*.jp*g" : $"*.{FileType}");
                var files = Directory.GetFiles(InputPath, filter);

                Console.WriteLine($"{files.Length} files found.");
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
                        Console.WriteLine($"ERROR : {e.Message}");
                        Console.WriteLine(e.StackTrace);
                    }
                }

                filesToProcess.CompleteAdding();

                Parallel.ForEach(filesToProcess.GetConsumingEnumerable(), parallelOptions, ProcessFile);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR : {e.Message}");
                Console.WriteLine(e.StackTrace);
            }

            Console.WriteLine("INFO : Completed job");
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
            SaveToOutput(filePath, text);
        }

        private void SaveToOutput(string filePath, string text)
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