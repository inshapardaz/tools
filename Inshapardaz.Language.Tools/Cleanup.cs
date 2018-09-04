﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Inshapardaz.Language.Tools
{
    public class Cleanup
    {
        public string Clean(string content)
        {
            var result = new StringBuilder();
            var lines = content.Split(new[] { '\r' });

            foreach (var line in lines)
            {
                result.AppendLine(CleanLine(line).CorrectedString);
            }

            return result.ToString().Trim();
        }

        private CleanupResult CleanLine(string line)
        {
            StringBuilder sb = new StringBuilder(line.Length);
            var suggessions = new List<Suggesstion>();
            char lastChar = char.MinValue;
            char secondLastChar = char.MinValue;
            bool inDoubleQuote = false;
            int index = -1;
            foreach (var ch in line)
            {
                index++;
                char c = ch;
                lastChar = sb.LastCharacter();
                secondLastChar = sb.SecondLastCharacter();
                if (c == '”' || c == '“')
                {
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.ReplaceCorrectQuotes } );
                    c = '\"';
                }

                // remove double space
                if (c == ' ' && lastChar == ' ')
                {
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.DoubleSpace });
                    continue;
                }

                #region Commas
                if (c == '،' && lastChar == ' ')
                {
                    sb.ReplaceLastCharacter(c);
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.CommaPreceededBySpace });
                    continue;
                }
                
                if (c == '،' && lastChar == '،')
                {
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.DoubleComma });
                    continue;
                }

                if (c != '،' && lastChar == '،' && c != ' ')
                {
                    sb.Append(" ");
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.NoSpaceAfterComma });
                }
                #endregion Comma

                #region Exclaimation
                if (c == '!' && lastChar == ' ')
                {
                    sb.ReplaceLastCharacter(c);
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.ExclaimationPreceededBySpace });
                    continue;
                }

                
                if (c != '!' && lastChar == '!'&& c != ' ')
                {
                    sb.Append(' ');
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.NoSpaceAfterExclaimation });
                }
                #endregion Exclaimation

                #region Question Mark
                if (c == '؟' && lastChar == ' ')
                {
                    sb.ReplaceLastCharacter(c);
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.QuestionPreceededBySpace });
                    continue;
                }
                
                #endregion Question Mark

                #region Full stop
                if (c == '۔' && lastChar == ' ')
                {
                    sb.ReplaceLastCharacter(c);
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.FullStopPreceededBySpace });
                    continue;
                }

                #endregion Full stop

                #region Double Quotes

                // Starting quote
                if (!inDoubleQuote && c == '\"' && lastChar != ' ' && sb.Length > 0)
                {
                    sb.Append(' ');
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.NoSpaceBeforeQuote });
                }

                //ending quote
                if (inDoubleQuote && c == '\"' && lastChar == '۔')
                {
                    sb.ReplaceLastCharacter(c);
                    c = '۔';
                    sb.Append(c);
                    inDoubleQuote = !inDoubleQuote;
                    continue;
                }

                if (c == '۔' && lastChar == '\"')
                {

                }


                if (c == '\"')
                {
                    inDoubleQuote = !inDoubleQuote;
                }

                #endregion Double Quotes


                if (c != '۔' && lastChar == '۔' && c != ' ')
                {
                    sb.Append(' ');
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.NoSpaceAfterFullStop });
                }


                if (c != '؟' && lastChar == '؟' && c != ' ')
                {
                    sb.Append(' ');
                    suggessions.Add(new Suggesstion { Position = index, SuggesstionType = SuggesstionTypes.NoSpaceAfterQuestion });
                }

                sb.Append(c);
            }

            return new CleanupResult { CorrectedString = sb.ToString().Trim(), Suggesstions = suggessions };
            /*line = RegexFactory.MultipleSpace.Replace(line, " ");
            line = RegexFactory.SpaceAroundComma.Replace(line, "، ");
            
            line = line.Replace(" ،", "،");
            line = line.Replace("،", "، ");

            line = line.Replace(" !", "!");
            line = line.Replace("!", "! ");

            line = line.Replace(" ؟", "؟");
            line = line.Replace(" ۔", "۔");
            line = line.Replace("۔\"", "\"۔");

            line = line.Replace("”", "\"");
            line = line.Replace("“", "\"");

            //line = line.Replace("\" ", "\"");
            //line = line.Replace("\"", " \"");

            line = line.Replace("\' ", "\'");
            line = line.Replace("\'", " \'");

            return line.Trim();*/
        }
    }

    public class CleanupResult
    {
        public string CorrectedString { get; set; }

        public IEnumerable<Suggesstion> Suggesstions { get; set; }
    }

    public class Suggesstion
    {
        public  int Position { get; set; }

        public SuggesstionTypes SuggesstionType { get; set; }

        public string Original { get; set; }

        public string Suggestere { get; set; }
    }

    public enum SuggesstionTypes
    {
        ReplaceCorrectQuotes,
        DoubleSpace,
        CommaPreceededBySpace,
        DoubleComma,
        NoSpaceAfterComma,
        ExclaimationPreceededBySpace,
        NoSpaceAfterExclaimation,
        QuestionPreceededBySpace,
        NoSpaceAfterQuestion,
        FullStopPreceededBySpace,
        NoSpaceAfterFullStop,
        FullStopPreceededByQuote,
        NoSpaceBeforeQuote
    }
}