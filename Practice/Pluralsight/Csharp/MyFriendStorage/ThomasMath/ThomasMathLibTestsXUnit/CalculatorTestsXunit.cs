using System;
using System.Diagnostics;
using ThomasMathLib;
using Xunit;
using Xunit.Abstractions;

namespace ThomasMathLibTestsXUnit
{
    public class CalculatorTestsXunit : TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CalculatorTestsXunit()
        {
            _testOutputHelper = new OutputHelper();
        }
        //[Theory]
        //[InlineData(5, 2, 3)]
        //[InlineData(10, 4, 6)]
        //[InlineData(2, 5, -3)]
        //public void AddShouldReturnSumOfValues(int expectedResult, int val1, int val2)
        //{
        //    // Arrange
        //    var calculator = new Calculator();

        //    // Act
        //    var result = calculator.Add(val1, val2);

        //    // Assert
        //    Assert.Equal(expectedResult, result);
        //}

        [Fact]
        public void TestA()
        {
            _testOutputHelper.WriteLine(RandomInt.ToString());
            _testOutputHelper.WriteLine(RandomInt.ToString());
        }

        [Fact]
        public void TestB()
        {
            _testOutputHelper.WriteLine(RandomInt.ToString());
        }
    }

    public class TestBase
    {
        private Random _random = new Random();
        private int? _randomInt = null;

        public int RandomInt
        {
            get
            {
                if (_randomInt == null)
                {
                    _randomInt = _random.Next(100, 10000);
                }

                return _randomInt.GetValueOrDefault();
            }
        }
    }

    public class OutputHelper : ITestOutputHelper
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine($"{format}");
        }
    }
}
