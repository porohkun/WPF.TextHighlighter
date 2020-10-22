using System.Collections.Generic;

namespace WPF.TextHighlighter
{
    public interface IHighlighter
    {
        IEnumerable<HighlightedPart> GetParts(string input, int externalOffset);
    }
}
