using System;
using DependencyInjectionInNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericTesting
{
    [TestClass]
    public class DependencyInjectionTesting
    {
        [TestMethod]
        public void ExclaimWillWriteCorrectMessageToMessageWriter()
        {
            var writer = new SpyMessageWriter();
            var sut = new Salutation(writer);
            sut.Exclaim();
            string expected = "Hello DI!";

            Assert.AreEqual(expected, writer.WrittenMessage);
        }
    }
}
