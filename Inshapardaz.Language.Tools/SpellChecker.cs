using System;
using System.Collections.Generic;

namespace Inshapardaz.Language.Tools
{
    public class SpellChecker
    {
        public IEnumerable<SpellingMistake> CheckSpellings(string content)
        {
            var mistakes = new List<SpellingMistake>();
            var tokens = new Tokenizer().Tokenize(content);
            foreach (var token in tokens)
            {

            }

            return mistakes;
        }
    }

    public class SpellingMistake
    {
        public string OriginalText { get; set; }

        public int PositionInText { get; set; }

        /// <summary>
        /// List of replacement suggesstions order by score
        /// </summary>
        public IEnumerable<string> Suggestion { get; set; }
    }
}
