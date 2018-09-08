using System;
using System.Collections.Generic;
using System.Linq;

namespace Inshapardaz.Language.Tools
{
    public class SpellChecker
    {
        private char[] urduChars = new char[]
        {
            'آ',
            'ا',
            'ب',
            'پ',
            'ت',
            'ٹ',
            'ث',
            'ج',
            'چ',
            'ح',
            'خ',
            'د',
            'ڈ',
            'ذ',
            'ر',
            'ڑ',
            'ز',
            'ژ',
            'س',
            'ش',
            'ص',
            'ض',
            'ط',
            'ظ',
            'ع',
            'غ',
            'ف',
            'ق',
            'ک',
            'گ',
            'ل',
            'م',
            'ن',
            'و',
            'ہ',
            'ء',
            'ی',
            'ئ',
            'ؤ',
            ' ',
            '‌'
        };

        DictonaryProvider _dictionary = new DictonaryProvider();

        public IEnumerable<SpellingMistake> CheckSpellings(string content, bool findCorrections = true)
        {
            var mistakes = new List<SpellingMistake>();
            var tokens = new Tokenizer().Tokenize(content);
            foreach (var token in tokens)
            {
                var sanitisedWord = token.Content.TrimSpecialCharacters().RemoveMovements();
                if (!_dictionary.CheckWord(token.Content))
                {
                    mistakes.Add(new SpellingMistake { OriginalText = token.Content, StartPosition = token.StartPosition, EndPosition = token.EndPosition });
                }
            }

            if (findCorrections)
            {
                foreach (var mistake in mistakes)
                {
                    mistake.Suggestion = FindCorrectOptions(mistake.OriginalText);
                }
            }

            return mistakes;
        }

        public IEnumerable<string> FindCorrectOptions(string word)
        {
            var replacements = new List<string>();

            var options = new List<string>();

            // drop characters from list
            for (int i = 0; i < word.Length; i++)
            {
                var ch = word[i];

                if (!Array.Exists(urduChars, c => c == ch))
                {
                    continue;
                }

                var newWord = new List<char>(word);
                newWord.RemoveAt(i);
                options.Add(new string(newWord.ToArray()));
            }

            // Methos 1 :: replace characters in the word by other words
            for (int i = 0; i < word.Length; i++)
            {
                var ch = word[i];

                if (!Array.Exists(urduChars, c => c == ch))
                {
                    continue;
                }

                foreach (var c in urduChars)
                {
                    var newWord = word.ToCharArray();
                    newWord[i] = c;
                    options.Add(new string(newWord));
                }
            }

            // append character at each location of the word
            for (int i = 0; i < word.Length; i++)
            {
                var ch = word[i];

                if (!Array.Exists(urduChars, c => c == ch))
                {
                    continue;
                }

                foreach (var c in urduChars)
                {
                    var newWord = new List<char>(word);
                    newWord.Insert(i, c);
                    options.Add(new string(newWord.ToArray()));
                }
            }

            // re-arrangecharacters from list
            for (int i = 0; i < word.Length - 1; i++)
            {
                var ch = word[i];

                if (!Array.Exists(urduChars, c => c == ch))
                {
                    continue;
                }

                var newWord = word.ToCharArray();
                char temp = newWord[i];
                newWord[i] = newWord[i + 1];
                newWord[i + 1] = temp;
                options.Add(new string(newWord));
            }

            var uniqueOptions = options.Distinct();

            var result = new List<string>();
            foreach (var option in uniqueOptions)
            {
                if(_dictionary.CheckWord(option))
                {
                    result.Add(option);
                }
            }

            result.Sort();
            return result;
        }
    }

    public class SpellingMistake
    {
        public string OriginalText { get; set; }

        public int StartPosition { get; set; }
        public int EndPosition { get; set; }

        /// <summary>
        /// List of replacement suggesstions order by score
        /// </summary>
        public IEnumerable<string> Suggestion { get; set; }
    }
}
