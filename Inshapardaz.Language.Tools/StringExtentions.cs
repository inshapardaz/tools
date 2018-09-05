﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Inshapardaz.Language.Tools
{
    public static class StringExtentions
    {
        public static string TrimSpecialCharacters(this string input)
        {
            return input.Trim(' ', '\'', '"', '[', ']', '(', ')', ',', '۔', '.', '‌', '،');
        }

        public static string RemoveMovements(this string input)
        {
            var result = new StringBuilder();
            foreach (var c in input)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        public static string[] SplitIntoSentences(this string input)
        {
            return input.Split('?', '۔', '.', '\n');
        }

        public static string[] SplitIntoWords(this string input)
        {
            return input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] PreserveSpecialCharacters(this string[] input)
        {
            char[] separators = { '?', '۔', '؟', '.', ',', '!', ':', ';', '\'', '/', '"', '&', '(', ')', '[', ']', '{', '}' };
            var result = new List<string>();
            foreach (var word in input)
            {
                var w = string.Empty;
                foreach (var c in word)
                {
                    // found separator
                    if (separators.Contains(c))
                    {
                        if (!string.IsNullOrWhiteSpace(w))
                        {
                            result.Add(w);
                        }

                        result.Add($"{c}");
                        w = "";
                    }
                    else
                    {
                        w += c;
                    }
                }

                if (!string.IsNullOrWhiteSpace(w))
                {
                    result.Add(w);
                }
            }

            return result.ToArray();
        }
    }
}
