using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ReadCardsExport;

namespace UnitTestWeb
{
    [TestClass]
    public class UnitTestingWeb
    {
        private IWebDriver driver;
        private string dekFileName = "Arty.dek";
        private Deck mTestDeck;

        [TestMethod]

        [TestInitialize]
        public void TestSetup()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            Thread.Sleep(2255);

            FileInfo inputFileInfo = new FileInfo(dekFileName);
            if (!File.Exists(dekFileName))
            {
                throw new Exception(string.Format("The file: '{0}' does not exist.", dekFileName));
            }
            
            FileInfo xmlFile = Program.ConvertDeckToDeserializableXml(inputFileInfo);
            mTestDeck = Deck.GetFromFile(xmlFile);

            Debug.WriteLine("Found {0} Cards.", mTestDeck.CardStack.Count);
            foreach (Cards card in mTestDeck.CardStack)
            {
                Debug.WriteLine(card.ToString());
            }
        }

        [TestMethod]
        public void TestCardSearch()
        {
            Assert.IsTrue(mTestDeck.CardStack.Count > 0);
            string errorMessage;
            int sleepTimeMs = 4000;
            try
            {
                string url = @"http://gatherer.wizards.com/Pages/Default.aspx";
                driver.Navigate().GoToUrl(url);

                foreach (var nextCard in mTestDeck.CardStack)
                {
                    Thread.Sleep(sleepTimeMs);
                    var searchBox = driver.FindElement(By.ClassName("textboxinput"));
                    searchBox.Clear();
                    searchBox.SendKeys(string.Format("{0}{1}", nextCard.Name, Environment.NewLine));

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

                        var imageOnlyUrl = string.Format(
                            @"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", multiversid);
                        //driver.Navigate()
                        //    .GoToUrl(imageOnlyUrl);

                        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(FilesPath, multiversid + ".jpg"), FileMode.Create)))
                        {
                            using (var client = new WebClient())
                            {
                                //client.Credentials =
                                writer.Write(client.DownloadData(imageOnlyUrl));
                            }
                            writer.Write(true);
                        }

                        Thread.Sleep(sleepTimeMs);
                    }
                    driver.Navigate().Back();
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
            return @"Primeval Bounty";
        }

        public string FilesPath
        {
            get { return @"C:\Temp\UnitTestingWeb"; }
        }
        
        private bool LogToFile(DateTime currentTime, string message, out string errorMessage)
        {
            bool success = true;
            errorMessage = String.Empty;

            string logMessage = String.Format("{0}\t{1}", currentTime, message);
            // TODO: Open (and close) the log file on loading of MO and close when Tester is closed or on LogOut.
            string logFileName = String.Format(@"{0}.log", currentTime.ToString("yyyy-MM-dd"));
            string logFilePath = Path.Combine(FilesPath, logFileName);
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
