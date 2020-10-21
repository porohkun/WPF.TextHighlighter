using System.Linq;

namespace WPF.TextHighlighter
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string value) => value.All(char.IsNumber);
    }
}
