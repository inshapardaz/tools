using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Inshapardaz.Language.Tools
{
    public static class RegexFactory
    {
        private static RegexOptions options = RegexOptions.Multiline & RegexOptions.RightToLeft;

        public static Regex MultipleSpace => new Regex(@"\s\s+", options);
        public static Regex SpaceAroundComma => new Regex(@"[,،](?=[^\s])", options);
    }
}
