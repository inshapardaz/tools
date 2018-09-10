using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace UrduEditor.ViewModel
{
    public class InsertBookViewModel : ViewModelBase
    {

        private string _inputFolder;
        private string _bookName;
        private int _authorId;
        private string _connectionString = "Server=.;Database=Inshapardaz;Trusted_Connection=True;";
        private ICommand _selectInputFolderCommand;
        private ICommand _processCommand;

        public string InputFolder
        {
            get { return _inputFolder; }
            set { _inputFolder = value; NotifyPropertyChanged(); }
        }

        public string BookName  
        {
            get { return _bookName; }
            set { _bookName = value; NotifyPropertyChanged(); }
        }

        public int AuthorId
        {
            get { return _authorId; }
            set { _authorId = value; NotifyPropertyChanged(); }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; NotifyPropertyChanged(); }
        }


        public ICommand SelectInputFolderCommand => _selectInputFolderCommand ?? (_selectInputFolderCommand = new CommandHandler(() => SelectInputFolder(), true));
        public ICommand ProcessCommand => _processCommand ?? (_processCommand = new CommandHandler(() => Process(), true));

        private void SelectInputFolder()
        {
            using (var folderSelection = new FolderBrowserDialog())
            {
                if (folderSelection.ShowDialog() == DialogResult.OK)
                {
                    InputFolder = folderSelection.SelectedPath;
                }
            }
        }

        private void Process()
        {
            if (string.IsNullOrWhiteSpace(InputFolder) || !Directory.Exists(InputFolder))
            {
                MessageBox.Show("Please provide a valid input folder.");
                return;
            }

            if (string.IsNullOrWhiteSpace(BookName))
            {
                MessageBox.Show("Please provide a valid input folder.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                MessageBox.Show("Please provide a valid connection string.");
                return;
            }

            var images = Directory.GetFiles(InputFolder, "*.jp*g");

            using (var connection = new SqlConnection(ConnectionString))
            {
                var bookCommand = connection.CreateCommand();
                bookCommand.CommandText = "INSERT INTO [Library].[Book] ([Title], [AuthorId], [IsPublic],[Language],[Status]) VALUES (@title, @authorId, 0, 2, 1); SELECT SCOPE_IDENTITY();";
                bookCommand.CommandType = System.Data.CommandType.Text;
                bookCommand.Parameters.Add(new SqlParameter("@title", BookName));
                bookCommand.Parameters.Add(new SqlParameter("@authorId", AuthorId));

                connection.Open();
                var bookId = bookCommand.ExecuteScalar();

                int i = 0;
                foreach (var page in images.OrderBy(f => f))
                {
                    i++;
                    var fileName = Path.GetFileNameWithoutExtension(page);
                    var textfile = Path.Combine(InputFolder, $"{fileName}.txt");

                    if (!File.Exists(textfile) || !File.Exists(page))
                    {
                        continue;
                    }

                    var pageCommand = connection.CreateCommand();
                    pageCommand.CommandText = "INSERT INTO [Library].[BookPage] ([BookId], [PageNumber], [Contents], [Text]) VALUES (@bookId, @pageNumber, @contents, @text)";
                    pageCommand.CommandType = System.Data.CommandType.Text;
                    pageCommand.Parameters.Add(new SqlParameter("@bookId", bookId));
                    pageCommand.Parameters.Add(new SqlParameter("@pageNumber", i));
                    pageCommand.Parameters.Add(new SqlParameter("@text", File.ReadAllText(textfile)));
                    pageCommand.Parameters.Add(new SqlParameter("@contents", File.ReadAllBytes(page)));

                    pageCommand.ExecuteNonQuery();
                }

            }
        }
    }
}
