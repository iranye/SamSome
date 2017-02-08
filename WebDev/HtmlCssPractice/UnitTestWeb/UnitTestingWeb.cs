using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.Events;

namespace UnitTestWeb
{
    [TestClass]
    public class UnitTestingWeb
    {
        private IWebDriver driver;
        [TestMethod]

        [TestInitialize]
        public void TestSetup()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver.Navigate().GoToUrl(url);
            Thread.Sleep(2255);
        }


        [TestMethod]
        public void TestCardSearch()
        {
            string errorMessage;
            try
            {
                string url = @"http://gatherer.wizards.com/Pages/Default.aspx";
                driver.Navigate().GoToUrl(url);
                string nextCard = GetNextCardName();
                Thread.Sleep(2255);
                var searchBox = driver.FindElement(By.ClassName("textboxinput"));
                searchBox.Clear();
                searchBox.SendKeys(string.Format("{0}{1}", nextCard, Environment.NewLine));

                string pattern = @"multiverseid=(\d+)";
                MatchCollection matches = Regex.Matches(driver.Url, pattern);
                if (matches.Count == 0)
                {
                    if (
                        !LogToFile(DateTime.Now, string.Format("Failed to match '{0}' using '{1}'", driver.Url, pattern),
                            out errorMessage))
                    {
                        Debug.WriteLine("Failed to Log to file: " + errorMessage);
                        // MessageBox.Show(@"Failed to Log to file." + Environment.NewLine + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string multiversid = matches[0].Groups[1].Value;
                    if (!LogToFile(DateTime.Now, string.Format("{0} multiverseid={1}", nextCard, multiversid), out errorMessage))
                    {
                        Debug.WriteLine("Failed to Log to file: " + errorMessage);
                    }
                    
                    Debug.WriteLine(driver.Url + ", " + multiversid);

                    driver.Navigate()
                        .GoToUrl(string.Format(
                            @"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", multiversid));
                    Thread.Sleep(2255);

                    var el = driver.FindElement(By.TagName("img"));
                    // el.Click();
                    el.SendKeys(Keys.LeftControl + "s");
                    // var action = new OpenQA.Selenium.Interactions.Actions(driver);
                    // action.SendKeys(Keys.Control + "s");
                    // action.KeyDown(Keys.Control).KeyDown("s").Perform();

                    //            action.ContextClick(elementToRightClick);
                    //            action.Perform();
                    //            // step 3 - execute the action
                    //            action.KeyDown(Keys.ArrowDown);
                    //            action.Perform();
                    //            action.KeyDown(Keys.ArrowDown);
                    //            action.Perform();
                    //            action.KeyDown(Keys.Enter);
                    //            action.Perform();
                    //            action.KeyDown(Keys.NumberPad2);
                    //            action.Perform();
                    //            action.KeyDown(Keys.Enter);
                    //            action.Perform();
                    Thread.Sleep(2255);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                driver.Close();
            }
        }

        private string GetNextCardName()
        {
            return @"Brass Man";
        }

        public string LogPath
        {
            get { return @"C:\Temp\Log"; }
        }
        
        private bool LogToFile(DateTime currentTime, string message, out string errorMessage)
        {
            bool success = true;
            errorMessage = String.Empty;

            string logMessage = String.Format("{0}\t{1}", currentTime, message);
            // TODO: Open (and close) the log file on loading of MO and close when Tester is closed or on LogOut.
            string logFileName = String.Format(@"{0}.log", currentTime.ToString("yyyy-MM-dd"));
            string logFilePath = Path.Combine(LogPath, logFileName);
            try
            {
                using (StreamWriter file = new StreamWriter(logFilePath, true))
                {
                    file.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("Failed to log to the file: " + logFilePath + " [" + ex.Message + "]");
                success = false;
            }
            return success;
        }
    }
}
