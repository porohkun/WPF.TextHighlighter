using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPF.TextHighlighter
{
    public class HighlightedPart
    {
        public int Index;
        public int Length;
        public int End => Index + Length;
        public Style TextStyle;
    }
}
