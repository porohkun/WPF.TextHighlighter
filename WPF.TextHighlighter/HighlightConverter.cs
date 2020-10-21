using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace WPF.TextHighlighter
{
    [System.Windows.Markup.ContentProperty(nameof(Data))]
    public class TextHighlighter : IValueConverter
    {
        public Brush Foreground { get; set; }
        public IList Data { get; set; } = new List<HighlightData>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string input) || string.IsNullOrWhiteSpace(input))
                return null;

            var parts = new List<HighlightedPart>() { new HighlightedPart() { Foreground = Foreground, Index = 0, Length = input.Length } };

            foreach (HighlightData dataItem in Data)
            {
                var regex = new Regex(dataItem.Pattern);
                var matches = regex.Matches(input);

                foreach (Match match in matches)
                {
                    for (int g = 0; g < match.Groups.Count; g++)
                    {
                        var group = match.Groups[g];
                        if (!group.Success || string.IsNullOrWhiteSpace(group.Name) || group.Name.IsNumeric())
                            continue;
                        var part = new HighlightedPart() { Foreground = (Brush)dataItem.Brushes[group.Name], Index = group.Index, Length = group.Length };

                        if (parts.Count == 0)
                            parts.Add(part);
                        else
                        {
                            for (int i = 0; i < parts.Count; i++)
                            {
                                var current = parts[i];
                                if (part.Index <= current.Index)
                                {
                                    parts.Insert(i, part);
                                    break;
                                }
                                else if (part.Index < current.Index + current.Length)
                                {
                                    var next = new HighlightedPart() { Foreground = current.Foreground, Index = part.Index, Length = current.Length - (part.Index - current.Index) };
                                    current.Length = part.Index - current.Index;
                                    parts.Insert(i + 1, part);
                                    parts.Insert(i + 2, next);
                                    break;
                                }
                                else if (parts.Count == i + 1)
                                {
                                    parts.Add(part);
                                    break;
                                }
                            }

                            var clearedParts = new List<HighlightedPart>();
                            {
                                HighlightedPart current = null;
                                foreach (var next in parts)
                                {
                                    if (current != null && next.Index < current.Index + current.Length)
                                    {
                                        next.Length -= current.Length - (next.Index - current.Index);
                                        if (next.Length <= 0)
                                            continue;
                                        next.Index = current.Index + current.Length;
                                    }
                                    current = next;
                                    clearedParts.Add(current);
                                }
                            }

                            parts = clearedParts;
                        }
                    }
                }
            }
            return parts.Select(p => new Run(input.Substring(p.Index, p.Length)) { Foreground = p.Foreground }).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        class HighlightedPart
        {
            public int Index;
            public int Length;
            public Brush Foreground;
        }
    }

    [System.Windows.Markup.ContentProperty(nameof(Brushes))]
    public class HighlightData
    {
        public string Pattern { get; set; }
        public IDictionary Brushes { get; set; } = new Dictionary<string, Brush>();
    }
}
