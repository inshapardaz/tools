using System;
using Shouldly;
using Xunit;

namespace Inshapardaz.Language.Tools.Tests
{
    public class CleanupTests
    {
        private void Test(string input, string output)
        {
            var result = new Cleanup().Clean(input);

            result.CorrectedString.ShouldBe(output);
        }


        [Fact]
        public void ShouldRemoveSpacesFromStartOfLine()
        {
            string input = " اب یہاں یہ سوال کیا جاسکتا ہے";
            string output = "اب یہاں یہ سوال کیا جاسکتا ہے";

            Test(input, output);
        }

        [Fact]
        public void ShouldRemoveSpacesFromEndOfLine()
        {
            string input = "اب یہاں یہ سوال کیا جاسکتا ہے ";
            string output = "اب یہاں یہ سوال کیا جاسکتا ہے";

            Test(input, output);
        }

        [Fact]
        public void ShouldRemoveMultipleSpacesFromSentence()
        {
            string input = "دے تو          پو  نہیں پھٹتی لہذا";
            string output = "دے تو پو نہیں پھٹتی لہذا";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceAfterComma()
        {
            string input = "کام،کام";
            string output = "کام، کام";

            Test(input, output);
        }

        [Fact]
        public void ShouldRemoveSpaceBeforeComma()
        {
            string input = "کام ،کام";
            string output = "کام، کام";

            Test(input, output);
        }

        [Fact]
        public void ShouldRemoveMultipleCommas()
        {
            string input = "دے تو،،، پو  نہیں پھٹتی لہذا";
            string output = "دے تو، پو نہیں پھٹتی لہذا";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceAfterInvertedComma()
        {
            string input = "رو نہو،روروم مور۔";
            string output = "رو نہو، روروم مور۔";

            Test(input, output);
        }

        [Fact]
        public void ShouldRemoveSpaceBeforeInvertedComma()
        {
            string input = "رو نہو ،روروم مور۔";
            string output = "رو نہو، روروم مور۔";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceAfterExclaimation()
        {
            string input = "ٹھہرو!بات سنو";
            string output = "ٹھہرو! بات سنو";

            Test(input, output);
        }

        [Fact]
        public void ShouldRemoveSpaceBeforeExclaimation()
        {
            string input = "ٹھہرو !بات سنو";
            string output = "ٹھہرو! بات سنو";

            Test(input, output);
        }

        [Fact]
        public void ShouldNotAddSpceBetweenMultipleExclaimationMarks()
        {
            string input = "ٹھہرو ! ! !";
            string output = "ٹھہرو!!!";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceBeforeQuestionMark()
        {
            string input = "کیا بات ہے ؟";
            string output = "کیا بات ہے؟";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceAfterQuestionMark()
        {
            string input = "ایک؟دو؟تین؟";
            string output = "ایک؟ دو؟ تین؟";

            Test(input, output);
        }

        [Fact]
        public void ShouldNotAddSpceBetweenMultipleQuestionMarks()
        {
            string input = "کیا ؟ ؟ ؟ ";
            string output = "کیا؟؟؟";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceBeforeFullStop()
        {
            string input = "کیا بات ہے ۔";
            string output = "کیا بات ہے۔";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceAfterFullStop()
        {
            string input = "ایک۔دو۔تین۔";
            string output = "ایک۔ دو۔ تین۔";

            Test(input, output);
        }

        [Fact]
        public void ShouldNOTAddSpaceAfterLastFullStopInParagraph()
        {
            string input = "ایک۔دو۔\r\nتین۔";
            string output = "ایک۔ دو۔\rnتین۔";

            Test(input, output);
        }

        [Fact]
        public void ShouldNotAddSpceBetweenMultipleFullStops()
        {
            string input = "ٹھہرو ۔ ۔ ۔";
            string output = "ٹھہرو۔۔۔";

            Test(input, output);
        }
        
        [Fact]
        public void ShouldReplaceStartingQuoteWithNormalquote()
        {
            string input = "”کیا بات ہے۔";
            string output = "\"کیا بات ہے۔";

            Test(input, output);
        }

        [Fact]
        public void ShouldReplaceEndingQuoteWithNormalquote()
        {
            string input = "\"کیا بات ہے“";
            string output = "\"کیا بات ہے\"";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceToStartQuote()
        {
            string input = "کہا\"کیا بات ہے\"";
            string output = "کہا \"کیا بات ہے\"";

            Test(input, output);
        }

        [Fact]
        public void ShouldNotAddSpaceOnStartOfLine()
        {
            string input = "کہا\n\r\"کیا بات ہے\"";
            string output = "کہا\r\n\"کیا بات ہے\"";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddFullStopAfterQuotes()
        {
            string input = "\"کیا بات ہے۔\"";
            string output = "\"کیا بات ہے\"۔";

            Test(input, output);
        }
        
        [Fact]
        public void ShouldAddSpaceBeforeStartDoubleQuote()
        {
            string input = "اس نے کہا\"سنو\"";
            string output = "اس نے کہا \"سنو\"";

            Test(input, output);
        }

        [Fact(Skip ="Not implemented yet")]
        public void ShouldAddSpaceBeforeStartSingleQuote()
        {
            string input = "اس نے کہا\'سنو\'";
            string output = "اس نے کہا \'سنو\'";

            Test(input, output);
        }

        [Fact(Skip = "Not sure if needed")]
        public void ShouldAddSpaceAfterQuestionAndQuote()
        {
            string input = "\"تم یہاں کہاں؟\"ابّا مسرّت سے مغلوب ہو کر بولے۔ ";
            string output = "\"تم یہاں کہاں؟\" ابّا مسرّت سے مغلوب ہو کر بولے۔ ";

            Test(input, output);
        }

        [Fact]
        public void ShouldAddSpaceBeforeBrace()
        {
            string input = "کام(جو کہ)کرنا";
            string output = "کام (جو کہ) کرنا";

            Test(input, output);
        }
    }
}
