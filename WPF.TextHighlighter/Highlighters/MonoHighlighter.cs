using System.Collections.Generic;
using System.Windows;

namespace WPF.TextHighlighter
{
    public class MonoHighlighter : IHighlighter
    {
        public Style Style { get; set; }

        public IEnumerable<HighlightedPart> GetParts(string input, int externalOffset)
        {
            yield return new HighlightedPart() { Index = externalOffset, Length = input.Length, TextStyle = Style };
        }
    }
}
