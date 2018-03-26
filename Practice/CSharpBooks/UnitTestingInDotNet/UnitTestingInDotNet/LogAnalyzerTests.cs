using LogAn;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;

namespace UnitTestingInDotNet
{
    // Name your tests clearly using the following model: [MethodUnderTest]_[Scenario]_[ExpectedBehavior]

    // Arrange
    // Act
    // Assert

    [TestFixture]
    public class LogAnalyzerTests
    {
        LogAnalyzer _analyzer;
        [SetUp]
        public void Setup()
        {
            _analyzer = new LogAnalyzer();
        }

        [TearDown]
        public void TearDown()
        {
            _analyzer = null;
        }

        [Test]
        public void IsValidFile_validFileLowerCased_ReturnsTrue()
        {
            // Arrange
            LogAnalyzer analyzer = new LogAnalyzer();

            // Act
            bool result = analyzer.IsValidLogFileName("whatever.slf");

            // Assert
            Assert.IsTrue(result, "file should be valid!");
        }

        [Test]
        public void IsValidFile_validFileUpperCased_ReturnsTrue()
        {
            // Arrange
            LogAnalyzer analyzer = new LogAnalyzer();

            // Act
            bool result = analyzer.IsValidLogFileName("whatever.SLF");

            // Assert
            Assert.IsTrue(result, "file should be valid!");
        }

        [Test]
        public void IsValidFile_validFileName_RemembersTrue()
        {
            // Arrange
            LogAnalyzer analyzer = new LogAnalyzer();

            // Act
            analyzer.IsValidLogFileName("somefile.slf");

            // Assert
            Assert.IsTrue(analyzer.WasLastFileNameValid, "Analyzer should remember valid=true!");
        }

        [Test]
        public void IsValidFile_inValidFileName_RemembersFalse()
        {
            // Arrange
            LogAnalyzer analyzer = new LogAnalyzer();

            // Act
            analyzer.IsValidLogFileName("somefile.wrong");

            // Assert
            Assert.IsFalse(analyzer.WasLastFileNameValid, "Analyzer should remember valid=false!");
        }

        [Test]
        public void IsValidFile_InvalidFilenameEmptyString_ThrowsException()
        {
            // Arrange
            LogAnalyzer analyzer = new LogAnalyzer();

            //Act
            ActualValueDelegate<object> testDelegate = () => analyzer.IsValidLogFileName(String.Empty);

            //Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsValidFile_InvalidFilenameWhitespaceString_ThrowsException()
        {
            // Arrange
            LogAnalyzer analyzer = new LogAnalyzer();

            //Act
            ActualValueDelegate<object> testDelegate = 
                () => analyzer.IsValidLogFileName(string.Format(Environment.NewLine));

            //Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }
    }
}
