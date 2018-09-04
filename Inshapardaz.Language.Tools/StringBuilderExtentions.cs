using System;
using System.Collections.Generic;
using System.Text;

namespace Inshapardaz.Language.Tools
{
    public static class StringBuilderExtentions
    {
        public static void ReplaceLastCharacter(this StringBuilder sb, char newChar)
        {
            sb[sb.Length - 1] = newChar;
        }

        public static char LastCharacter(this StringBuilder sb)
        {
            if (sb.Length > 1)
                return sb[sb.Length - 1];
            return char.MinValue;
        }
        public static char SecondLastCharacter(this StringBuilder sb)
        {
            if (sb.Length > 2)
                return sb[sb.Length - 2];
            return char.MinValue;
        }
    }
}
