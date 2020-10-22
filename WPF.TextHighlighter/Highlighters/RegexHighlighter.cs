using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WPF.TextHighlighter
{
    [System.Windows.Markup.ContentProperty(nameof(Highlighters))]
    public class RegexHighlighter : IHighlighter
    {
        public string Pattern { get; set; }
        public HighlighterDictionary Highlighters { get; set; } = new HighlighterDictionary();

        public IEnumerable<HighlightedPart> GetParts(string input, int externalOffset)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
                yield break;
            var regex = new Regex(Pattern);
            var matches = regex.Matches(input);
            foreach (Match match in matches)
            {
                for (int g = 0; g < match.Groups.Count; g++)
                {
                    var group = match.Groups[g];
                    if (!group.Success || string.IsNullOrWhiteSpace(group.Name) || !Highlighters.ContainsKey(group.Name))
                        continue;

                    var highlighter = Highlighters[group.Name];
                    foreach (var part in highlighter.GetParts(group.Value, externalOffset + group.Index))
                        yield return part;
                }
            }
        }
    }
}
