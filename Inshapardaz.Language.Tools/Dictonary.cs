using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Inshapardaz.Language.Tools
{
    public class DictonaryProvider
    {
        private static string indexPath;

        static DictonaryProvider()
        {
            var assembly = typeof(DictonaryProvider).Assembly;
            indexPath = Path.Combine(new DirectoryInfo(assembly.Location).Parent.FullName, "index");

            Stream resource = assembly.GetManifestResourceStream("Inshapardaz.Language.Tools.Data.dictionary01.csv");
            TextReader reader = new StreamReader(resource);
            CsvParserOptions csvParserOptions = new CsvParserOptions(false, ',');
            CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
            WordMapping csvMapper = new WordMapping();
            CsvParser<Word> csvParser = new CsvParser<Word>(csvParserOptions, csvMapper);

            var words = csvParser
                .ReadFromString(csvReaderOptions, reader.ReadToEnd())
                .ToList();

            InitialiseLucene(words);
        }

        private static void InitialiseLucene(List<CsvMappingResult<Word>> words)
        {
            if (System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.Delete(indexPath, true);
            }

            var directory = FSDirectory.Open(indexPath);

            Analyzer analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            using (IndexWriter indexWriter = new IndexWriter(directory, new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer)))
            {
                foreach (var word in words)
                {
                    var doc = new Document
                    {
                        new TextField("title", word.Result.Title, Field.Store.YES),
                        new TextField("titleWithMovements", word.Result.TitleWithMovements, Field.Store.YES)
                    };

                    indexWriter.AddDocument(doc);
                }
            }
        }

        /*private static BuildIndex(IEnumerable<Word> words)
        {
            foreach (var word in words)
            {
                Document doc = new Document();
                doc.Add(new Field("Title", word.Title, 
                    Field.Store.YES,
                    Field.Index.ANALYZED));
                doc.Add(new Field("LineText",
                    word.TitleWithMovements,
                    Field.Store.YES,
                    Field.Index.NOT_ANALYZED));
                writer.AddDocument(doc);
            }
            writer.Optimize();
            writer.Flush();
            writer.Close();
            luceneIndexDirectory.Close();
        }*/


        public bool CheckWord(string word)
        {
            Query query = new WildcardQuery(new Term("title", word));
            var directory = FSDirectory.Open(indexPath);
            using (var reader = DirectoryReader.Open(directory))
            {
                var searcher = new IndexSearcher(reader);
                var sorter = new Sort(new SortField("title", SortFieldType.STRING));
                var hits = searcher.Search(query, new FieldValueFilter("title"), int.MaxValue, sorter, true, true);

                return hits.TotalHits > 0;
            }
        }

        private class Word
        {
            public string Title { get; set; }
            public string TitleWithMovements { get; set; }
        }

        private class WordMapping : CsvMapping<Word>
        {
            public WordMapping()
            {
                MapProperty(0, x => x.Title);
                MapProperty(1, x => x.TitleWithMovements);
            }
        }
    }
}
