using ACM.BL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACM.BLTest
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void FullNameTestValid()
        {
            // Arrange
            Customer customer = new Customer
            {
                FirstName = "Bilbo",
                LastName = "Baggins"
            };
            string expectedFullName = "Baggins, Bilbo";

            // Act
            var actual = customer.FullName;

            // Assert
            Assert.AreEqual(expectedFullName, actual);
        }

        [TestMethod]
        public void FulllNameFirstNameEmpty()
        {
            // Arrange
            Customer customer = new Customer
            {
                LastName = "Baggins"
            };
            string expectedFullName = "Baggins";

            // Act
            var actual = customer.FullName;

            // Assert
            Assert.AreEqual(expectedFullName, actual);
        }

        [TestMethod]
        public void FulllNameLastNameEmpty()
        {
            // Arrange
            Customer customer = new Customer
            {
                FirstName = "Bilbo"
            };
            string expectedFullName = "Bilbo";

            // Act
            var actual = customer.FullName;

            // Assert
            Assert.AreEqual(expectedFullName, actual);
        }

        [TestMethod]
        public void ObjectTypeTest()
        {
            // Arrange
            Customer c1 = new Customer
            {
                FirstName = "Bilbo"
            };
            string expectedFullName = "Bilbo";

            // Act
            var c2 = c1;
            c2.FirstName = "Frodo";

            // Assert
            Assert.AreEqual("Frodo", c1.FullName);
        }

        [TestMethod]
        public void StaticTest()
        {
            // Arrange
            Customer c1 = new Customer
            {
                FirstName = "Bilbo"
            };
            Customer.InstanceCount++;

            Customer c2 = new Customer
            {
                FirstName = "Frodo"
            };
            Customer.InstanceCount++;

            Customer c3 = new Customer
            {
                FirstName = "Rosie"
            };
            Customer.InstanceCount++;

            // Act

            // Assert
            Assert.AreEqual(3, Customer.InstanceCount);
        }

        [TestMethod]
        public void ValidateValid()
        {
            // Arrange
            Customer c1 = new Customer
            {
                LastName = "Baggins",
                EmailAddress = "fbaggins@hobboton.me"
            };
            bool expected = true;

            // Act
            bool actual = c1.Validate();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidateMissingLastName()
        {
            // Arrange
            Customer c1 = new Customer
            {
                LastName = "   ",
                EmailAddress = "fbaggins@hobboton.me"
            };
            bool expected = false;

            // Act
            bool actual = c1.Validate();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
