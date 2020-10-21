using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WPF.TextHighlighter
{
    public class TextBlockInliner
    {
        #region Inlines

        public static readonly DependencyProperty InlinesProperty =
            DependencyProperty.RegisterAttached(
                "Inlines",
                typeof(IEnumerable<Inline>),
                typeof(TextBlockInliner),
                new FrameworkPropertyMetadata(OnInlinesPropertyChanged));

        public static IEnumerable<Inline> GetInlines(DependencyObject d)
        {
            return (IEnumerable<Inline>)d.GetValue(InlinesProperty);
        }

        public static void SetInlines(DependencyObject d, IEnumerable<Inline> value)
        {
            d.SetValue(InlinesProperty, value);
        }

        #endregion //Inlines

        private static void OnInlinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBlock textBlock))
                return;

            var inlinesCollection = textBlock.Inlines;
            inlinesCollection.Clear();
            inlinesCollection.AddRange((IEnumerable<Inline>)e.NewValue);
        }
    }
}
