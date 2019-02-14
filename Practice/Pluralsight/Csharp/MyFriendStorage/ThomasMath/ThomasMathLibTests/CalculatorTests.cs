using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThomasMathLib;

namespace ThomasMathLibTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod()]
        public void AddShouldReturnSumOfValues()
        {
            var calculator = new Calculator();
            var result = calculator.Add(2, 3);
            Assert.AreEqual(5, result);
        }
    }
}
