using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF.TextHighlighter
{
    [System.Windows.Markup.ContentProperty(nameof(Highlighters))]
    public class HighlightConverter : FrameworkElement, IValueConverter
    {
        public IList Highlighters { get; set; } = new List<IHighlighter>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string input) || string.IsNullOrWhiteSpace(input))
                return null;

            try
            {
                var parts = new List<HighlightedPart>();

                foreach (IHighlighter highlighter in Highlighters)
                {
                    MergeHighlightedParts(parts, highlighter.GetParts(input, 0));
                }

                return parts.Select(p => new Run(input.Substring(p.Index, p.Length)) { Style = p.TextStyle }).ToArray();
            }
            catch (Exception e)
            {
                return new[] { new Run(e.ToString()) { Foreground = Brushes.Magenta } };
            }
        }

        internal static void MergeHighlightedParts(IList<HighlightedPart> parts, IEnumerable<HighlightedPart> newParts)
        {
            int last = 0;
            foreach (var part in newParts)
            {
                if (parts.Count == 0)
                {
                    parts.Add(part);
                    continue;
                }

                for (int i = last; i < parts.Count; i++)
                {
                    last = i;
                    var current = parts[i];
                    if (part.Index <= current.Index)
                    {
                        parts.Insert(i, part);
                        Override(parts, i);
                        break;
                    }
                    else if (part.Index < current.End)
                    {
                        var next = new HighlightedPart()
                        {
                            TextStyle = current.TextStyle,
                            Index = part.Index,
                            Length = current.End - part.Index
                        };
                        current.Length = part.Index - current.Index;
                        parts.Insert(i + 1, part);
                        parts.Insert(i + 2, next);
                        Override(parts, i + 1);
                        break;
                    }
                    else if (parts.Count == i + 1)
                    {
                        parts.Add(part);
                        break;
                    }
                }
            }
        }

        internal static void Override(IList<HighlightedPart> parts, int partIndex)
        {
            var part = parts[partIndex];
            int i = partIndex + 1;
            if (parts.Count > i)
            {
                HighlightedPart current;
                do
                {
                    current = parts[i];
                    var newIndex = part.End;
                    var offset = newIndex - current.Index;
                    if (offset >= current.Length)
                        parts.RemoveAt(i);
                    else
                    {
                        current.Index = newIndex;
                        current.Length -= offset;
                    }
                } while (current.Index < part.End && parts.Count > i);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
