using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class TestCamelCase
    {
        [TestMethod]
        public void Test_GetLongestChain_SmallLists()
        {
            List<string> listOfStrings = new List<string>
            {
                "blat blit boo",
                "aces bar ventura",
                "bar foo",
            };
            listOfStrings = listOfStrings.OrderBy(s => s).ToList();

            string expectedLongestStringChain = "aces bar foo ventura";
            var actualLongestStringChain = CamelCase.Program.GetLongestStringChain(listOfStrings);
            Assert.AreEqual(expectedLongestStringChain, actualLongestStringChain);
        }

        [TestMethod]
        public void Test_GetLongestChain_LongLists()
        {
            List<string> listOfStrings = new List<string>
            {
                "aces bar ventura",
                "blat blit boo bing bong",
                "bar foo",
            };
            listOfStrings = listOfStrings.OrderBy(s => s).ToList();

            string expectedLongestStringChain = "blat blit boo bing bong";
            var actualLongestStringChain = CamelCase.Program.GetLongestStringChain(listOfStrings);
            Assert.AreEqual(expectedLongestStringChain, actualLongestStringChain);
        }
    }
}
