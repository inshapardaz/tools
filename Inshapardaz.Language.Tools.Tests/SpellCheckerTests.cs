using System.Linq;
using Shouldly;
using Xunit;

namespace Inshapardaz.Language.Tools.Tests
{
    public class SpellCheckerTests
    {

        [Fact]
        public void SpellCheckIdentifyTheCorrectWords()
        {
            var result = new SpellChecker().CheckSpellings("کام کی بات");

            result.ShouldBeEmpty();
        }

        [Fact]
        public void SpellCheckIdentifyTheIncorrectWords()
        {
            var result = new SpellChecker().CheckSpellings("کام ثکی بات");

            result.ShouldNotBeEmpty();
        }

        [Fact]
        public void SpellProvideSuggessions()
        {
            var result = new SpellChecker().CheckSpellings("کام ثکی بات");

            result.ShouldNotBeEmpty();

            result.First().Suggestion.ShouldNotBeEmpty();
        }
    }
}
