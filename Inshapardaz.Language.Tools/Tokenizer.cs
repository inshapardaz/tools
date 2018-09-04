using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inshapardaz.Language.Tools
{
    public class Tokenizer
    {
        private char[] splitcharacters = new char[] {
            ' ', ',', '۔', '.', '?', '؟', '-', '\'', '\"', '”', '“', ':', '!', '\n'
        };

        private char[] ignoreCharacters = new char[] {
            '\r'
        };
        public IEnumerable<Token> Tokenize(string input)
        {
            List<Token> tokens = new List<Token>();
            Token token = null;
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (ignoreCharacters.Contains(c))
                {
                    continue;
                }

                if (splitcharacters.Contains(c))
                {
                    if (token != null)
                    {
                        token.EndPosition = i;
                        tokens.Add(token);
                        token = null;
                    }
                }
                else
                {
                    if (token == null)
                    {
                        token = new Token();
                        token.StartPosition = i;
                    }
                    token.Content += c;
                }
            }

            if (token != null)
                tokens.Add(token);

            return tokens;

        }
    }

    public class Token
    {
        public string Content { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
    }
}
