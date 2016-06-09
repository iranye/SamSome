using System;
using BadCodeToGoodCode;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BadCodeToGoodCodeTest
{
    [TestClass]
    public class DiscountMethodTests
    {
        [TestMethod]
        public void Test_CustomerNotRegistered()
        {
            DiscountManager discountManager = new DiscountManager();
            int accountYears = 0;
            AccountStatus accountStatus = AccountStatus.NotRegistered;
            decimal originalPrice = 19.99m;
            decimal expectedPriceAfterDiscount = originalPrice;
            decimal actualPriceAfterDiscount = discountManager.ApplyDiscount(originalPrice, accountStatus, accountYears);
            Assert.AreEqual(expectedPriceAfterDiscount, actualPriceAfterDiscount);
        }
    }
}
