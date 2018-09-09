using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using IronPdf;

namespace UrduEditor.ViewModel
{
    public class PdfToImageViewModel : ViewModelBase
    {
        private ICommand _openPdfCommand;
        private ICommand _selectFolderCommand;
        private ICommand _convertCommand;

        private bool _busy;
        private string _outputPath;
        private string _pdfFilePath;

        public string PdfFilePath
        {
            get
            {
                return _pdfFilePath;
            }
            set
            {
                _pdfFilePath = value;
                NotifyPropertyChanged();
            }
        }

        public string OutputPath
        {
            get
            {
                return _outputPath;
            }

            set
            {
                _outputPath = value;
                NotifyPropertyChanged();
            }
        }


        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
                NotifyPropertyChanged();
            }
        }
        
        public ICommand OpenPdfCommand
        {
            get
            {
                return _openPdfCommand ?? (_openPdfCommand = new CommandHandler(() => OpenPdf(), true));
            }
        }
        public ICommand SelectOutputFolderCommand
        {
            get
            {
                return _selectFolderCommand ?? (_selectFolderCommand = new CommandHandler(() => SelectOutputFolder(), true));
            }
        }

        public ICommand ConvertCommand
        {
            get
            {
                return _convertCommand ?? (_convertCommand = new CommandHandler(() => ConvertPdfToImage(), true));
            }
        }

        public void OpenPdf()
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PdfFilePath = openFile.FileName;
            }
        }
        
        

        public void SelectOutputFolder()
        {
            using (var folderSelection = new FolderBrowserDialog())
            {
                if (folderSelection.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    OutputPath = folderSelection.SelectedPath;
                }
            }
        }


        public void ConvertPdfToImage()
        {
            if (!File.Exists(PdfFilePath))
            {
                MessageBox.Show("Invalid pdf file selected. Please select a valid file for conversion");
                return;
            }

            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }

            Busy = true;
            var pdf = PdfDocument.FromFile(PdfFilePath);
            //Extract all pages to a folder as image files
            try
            {
                pdf.RasterizeToImageFiles($"{OutputPath}\\*.jpg", ImageType.Jpeg);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to extract images. Make sure the PDF is not protected {Environment.NewLine}{ex.Message}");
            }

            Busy = false;
        }
    }
}
