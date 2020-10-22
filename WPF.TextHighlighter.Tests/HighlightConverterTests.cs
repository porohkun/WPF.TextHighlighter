using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WPF.TextHighlighter.Tests
{
    [TestClass]
    public class HighlightConverterTests
    {
        [TestMethod]
        public void MergeHighlightedParts_01()
        {
            var parts = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 5, Length = 5 }
            };

            var parts2 = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 2, Length = 5 }
            };

            HighlightConverter.MergeHighlightedParts(parts, parts2);

            Assert.AreEqual(parts.Count, 2);
            AssertHighlightedPartEqual(parts[0], new HighlightedPart() { Index = 2, Length = 5 });
            AssertHighlightedPartEqual(parts[1], new HighlightedPart() { Index = 7, Length = 3 });
        }

        [TestMethod]
        public void MergeHighlightedParts_02()
        {
            var parts = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 0, Length = 5 },
                new HighlightedPart() { Index = 5, Length = 5 }
            };

            var parts2 = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 2, Length = 5 }
            };

            HighlightConverter.MergeHighlightedParts(parts, parts2);

            Assert.AreEqual(parts.Count, 3);
            AssertHighlightedPartEqual(parts[0], new HighlightedPart() { Index = 0, Length = 2 });
            AssertHighlightedPartEqual(parts[1], new HighlightedPart() { Index = 2, Length = 5 });
            AssertHighlightedPartEqual(parts[2], new HighlightedPart() { Index = 7, Length = 3 });
        }

        #region Override

        [TestMethod]
        public void Override_01()
        {
            var parts = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 0, Length = 5 },
                new HighlightedPart() { Index = 3, Length = 5 }
            };

            HighlightConverter.Override(parts, 0);

            Assert.AreEqual(parts.Count, 2);
            AssertHighlightedPartEqual(parts[0], new HighlightedPart() { Index = 0, Length = 5 });
            AssertHighlightedPartEqual(parts[1], new HighlightedPart() { Index = 5, Length = 3 });
        }

        [TestMethod]
        public void Override_02()
        {
            var parts = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 0, Length = 5 },
                new HighlightedPart() { Index = 5, Length = 5 },
                new HighlightedPart() { Index = 8, Length = 5 }
            };

            HighlightConverter.Override(parts, 1);

            Assert.AreEqual(parts.Count, 3);
            AssertHighlightedPartEqual(parts[0], new HighlightedPart() { Index = 0, Length = 5 });
            AssertHighlightedPartEqual(parts[1], new HighlightedPart() { Index = 5, Length = 5 });
            AssertHighlightedPartEqual(parts[2], new HighlightedPart() { Index = 10, Length = 3 });
        }

        [TestMethod]
        public void Override_03()
        {
            var parts = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 0, Length = 8 },
                new HighlightedPart() { Index = 2, Length = 2 },
                new HighlightedPart() { Index = 5, Length = 10 }
            };

            HighlightConverter.Override(parts, 0);

            Assert.AreEqual(parts.Count, 2);
            AssertHighlightedPartEqual(parts[0], new HighlightedPart() { Index = 0, Length = 8 });
            AssertHighlightedPartEqual(parts[1], new HighlightedPart() { Index = 8, Length = 7 });
        }

        [TestMethod]
        public void Override_04()
        {
            var parts = new List<HighlightedPart>()
            {
                new HighlightedPart() { Index = 0, Length = 5 },
                new HighlightedPart() { Index = 5, Length = 5 }
            };

            HighlightConverter.Override(parts, 1);

            Assert.AreEqual(parts.Count, 2);
            AssertHighlightedPartEqual(parts[0], new HighlightedPart() { Index = 0, Length = 5 });
            AssertHighlightedPartEqual(parts[1], new HighlightedPart() { Index = 5, Length = 5 });
        }

        #endregion

        private void AssertHighlightedPartEqual(HighlightedPart one, HighlightedPart another)
        {
            Assert.AreEqual(one.Index, another.Index);
            Assert.AreEqual(one.Length, another.Length);
            Assert.AreEqual(one.TextStyle, another.TextStyle);
        }
    }
}
