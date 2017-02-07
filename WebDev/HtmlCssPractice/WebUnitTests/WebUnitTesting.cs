using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using System.Threading;

namespace WebUnitTests
{
    [TestClass]
    public class WebUnitTesting
    {
        IWebDriver driver;
        //string url = @"http://learnseleniumtesting.com/demo";

        [TestInitialize]
        public void TestSetup()
        {
            driver = new ChromeDriver();
            //driver.Navigate().GoToUrl(url);
            Thread.Sleep(2255);
        }

        [TestMethod]
        public void VerifyItems()
        {
            driver.FindElement(By.PartialLinkText("Nexus")).Click();
            var summary = driver.FindElement(By.CssSelector("#product-18 &amp;gt; div.row &amp;gt; div.col-md-7 &amp;gt; div &amp;gt; div.product_meta &amp;gt; span &amp;gt; span")).Text;

        }

        [TestMethod]
        public void TestCardSearch()
        {
            string url = @"http://gatherer.wizards.com/Pages/Default.aspx";            
            driver.Navigate().GoToUrl(url);
            string nextCard = GetNextCardName();
            Thread.Sleep(2255);
            driver.FindElement(By.ClassName("textboxinput")).SendKeys(string.Format("{0}{1}", GetNextCardName(), Environment.NewLine));
            //driver.FindElement(By.ClassName("searchbutton")).Click();
            Thread.Sleep(2255);
            string cardOnResultsPage = "";
            try
            {
                //cardOnResultsPage = driver.FindElement(By.Id("#ctl00_ctl00_ctl00_MainContent_SubContent_SubContentHeader_subtitleDisplay")).Text;
                cardOnResultsPage = driver.FindElement(By.ClassName("contentTitle")).FindElement(By.TagName("span")).Text;
            }
            catch (OpenQA.Selenium.InvalidSelectorException ex)
            {
                Console.WriteLine("Failed to find element.");
            }
            Assert.AreEqual(nextCard, cardOnResultsPage);

            var elementToRightClick = driver.FindElement(By.Id("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_cardImage"));

            // step 2 - create and step up an Actions object with your driver
            var action = new OpenQA.Selenium.Interactions.Actions(driver);
            action.ContextClick(elementToRightClick);
            action.Perform();
            // step 3 - execute the action
            action.KeyDown(Keys.ArrowDown);
            action.Perform();
            action.KeyDown(Keys.ArrowDown);
            action.Perform();
            action.KeyDown(Keys.Enter);
            action.Perform();
            action.KeyDown(Keys.NumberPad2);
            action.Perform();
            action.KeyDown(Keys.Enter);
            action.Perform();
            Thread.Sleep(2255);
        }

        private string GetNextCardName()
        {
            return @"Abattoir Ghoul";
        }

        [TestCleanup]
        public void Cleanup()
        {
            Thread.Sleep(8255);
            driver.Quit();
        }
    }
}

