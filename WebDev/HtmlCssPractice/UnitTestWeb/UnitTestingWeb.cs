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
        private const string TEST_DEK_FILE = "Arty.dek";
        private const string TEST_DIRECTORY = @"C:\Temp\UnitTestingWeb";

        private FileInfo mXmlDeckFile;
        private Deck mTestDeck;

        public string TestDirectoryPath
        {
            get {
                if (!Directory.Exists(TEST_DIRECTORY))
                {
                    Directory.CreateDirectory(TEST_DIRECTORY);
                }
                return TEST_DIRECTORY;
            }
        }
        
        public string TestDeckFilePath
        {
            get
            {
                string filePath = Path.Combine(TestDirectoryPath, TEST_DEK_FILE);
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, GetDekFileContents);
                }
                return filePath;
            }
        }
        
        private string GetDekFileContents
        {
            //TODO: return a random set of cards to search for.
            get
            {
                return @"<?xml version=""1.0"" encoding=""utf-8""?>
<Deck>
  <NetDeckID>0</NetDeckID>
  <PreconstructedDeckID>0</PreconstructedDeckID>
  <Cards CatID=""39449"" Quantity=""4"" Sideboard=""false"" Name=""Brass Squire"" />
  <Cards CatID=""49233"" Quantity=""1"" Sideboard=""false"" Name=""Accorder's Shield"" />
  <Cards CatID=""54158"" Quantity=""1"" Sideboard=""false"" Name=""Briber's Purse"" />
  <Cards CatID=""54456"" Quantity=""1"" Sideboard=""false"" Name=""Cranial Archive"" />
  <Cards CatID=""49139"" Quantity=""1"" Sideboard=""false"" Name=""Door of Destinies"" />
</Deck>";
            }
        }

        private void ClearTestDirectory()
        {
            if (!Directory.Exists(TestDirectoryPath))
            {
                return;
            }
            DirectoryInfo dirInfo = new DirectoryInfo(TestDirectoryPath);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                File.Delete(file.FullName);
            }
        }

        [TestInitialize]
        public void TestSetup()
        {
            // Delete the test files prior to creating.
            ClearTestDirectory();

            FileInfo inputFileInfo = new FileInfo(TestDeckFilePath);
            if (!File.Exists(TestDeckFilePath))
            {
                throw new Exception(string.Format("The file: '{0}' does not exist.", TestDeckFilePath));
            }

            mXmlDeckFile = Program.ConvertDeckToDeserializableXml(inputFileInfo);
            mTestDeck = Deck.GetFromFile(mXmlDeckFile);

            if (mTestDeck.CardStack.Count < 1)
            {
                throw new Exception(string.Format("Failed to read any files from dek: '{0}'.", TestDeckFilePath));
            }
            Debug.WriteLine("Found {0} Cards.", mTestDeck.CardStack.Count);
            foreach (Cards card in mTestDeck.CardStack)
            {
                Debug.WriteLine(card.ToString());
            }
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            Thread.Sleep(2255);
        }

        [TestMethod]
        public void TestCardSearch()
        {
            Assert.IsTrue(mTestDeck.CardStack.Count > 0);
            Assert.IsTrue(File.Exists(TestDeckFilePath));
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
                        int mid = 0;
                        if (!Int32.TryParse(multiversid, out mid))
                        {
                            Debug.WriteLine("Failed to Parse multiversid: " + multiversid);
                        }

                        nextCard.MultiverseID = mid;
                        if (!LogToFile(DateTime.Now, string.Format("{0} multiverseid={1}", nextCard, multiversid), out errorMessage))
                        {
                            Debug.WriteLine("Failed to Log to file: " + errorMessage);
                        }

                        Debug.WriteLine(driver.Url + ", " + multiversid);

                        var imageOnlyUrl = string.Format(
                            @"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", multiversid);

                        string pathToImageFile = Path.Combine(TestDirectoryPath, multiversid + ".jpg");
                        if (!File.Exists(pathToImageFile))
                        {
                            using (BinaryWriter writer = new BinaryWriter(File.Open(pathToImageFile, FileMode.Create)))
                            {
                                using (var client = new WebClient())
                                {
                                    writer.Write(client.DownloadData(imageOnlyUrl));
                                }
                                writer.Write(true);
                            }
                        }
                    }
                    driver.Navigate().Back();
                }
                mTestDeck.SaveToFile(mXmlDeckFile);
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
        
        private bool LogToFile(DateTime currentTime, string message, out string errorMessage)
        {
            bool success = true;
            errorMessage = String.Empty;

            string logMessage = String.Format("{0}\t{1}", currentTime, message);
            // TODO: Open (and close) the log file on loading of MO and close when Tester is closed or on LogOut.
            string logFileName = String.Format(@"{0}.log", currentTime.ToString("yyyy-MM-dd"));
            string logFilePath = Path.Combine(TestDirectoryPath, logFileName);
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
