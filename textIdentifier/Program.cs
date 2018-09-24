using System;
using Microsoft.Extensions.CommandLineUtils;

namespace textIdentifier
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "Inshapardaz Google OCR Worker";
            app.Description = "Convert an entire folder of images to text";

            app.HelpOption("-?|-h|--help");

            var input = app.Option("-i|--input",
                    "Input path",
                    CommandOptionType.SingleValue);

            var output = app.Option("-o|--output",
                    "Output path",
                    CommandOptionType.SingleValue);

            var format = app.Option("-f|--format",
                    "Format of files to process",
                    CommandOptionType.SingleValue);

            var merge = app.Option("-m|--merge",
                    "Merge output to one file",
                    CommandOptionType.NoValue);

            app.OnExecute(() => {
                new TextIdentifier(input.Value(), output.Value(), true, format.Value()).Process();
                return 0;
            });

            app.Execute(args);
        }
    }
}
