using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneralPractice;

namespace TestGeneric
{
    [TestClass]
    public class TestGeneralPractice
    {
        [TestMethod]
        public void Test_FuzzyEqualSimple_Success()
        {
            string original = "abdac";
            string wantToExclude = "abcd";

            const float threshold = .80F;
            bool fuzzyEqual = Program.AreFuzzyEqual(original, wantToExclude, threshold);
            Assert.IsTrue(fuzzyEqual);
        }

        [TestMethod]
        public void Test_FuzzyEqualLongerString_Failed()
        {
            string original = "abd0a89erj08gjac";
            string wantToExclude = "aja09ejabcd";

            const float threshold = .80F;
            bool fuzzyEqual = Program.AreFuzzyEqual(original, wantToExclude, threshold);
            Assert.IsFalse(fuzzyEqual);
        }

        [TestMethod]
        public void Test_FuzzyCompareFileNames_Success()
        {
            string original = "21-01_The Rolling Stones_Miss You.mp3";
            string wantToExclude = "39_03_The Rolling Stones_Miss You.mp3";

            const float threshold = .80F;
            bool fuzzyEqual = Program.AreFuzzyEqual(original, wantToExclude, threshold);
            Assert.IsTrue(fuzzyEqual);
        }


    }
}
