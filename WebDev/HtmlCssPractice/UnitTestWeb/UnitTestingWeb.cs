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
        private IWebDriver _webDriver;
        private const string TEST_DEK_FILE = "Arty.dek";
        private const string TEST_DIRECTORY = @"C:\Temp\UnitTestingWeb";

        private FileInfo _xmlDeckFile;
        private Deck _testDeck;

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
            Assert.IsTrue(File.Exists(TestDeckFilePath), $"The file '{TestDeckFilePath}' does not exist.");

            _xmlDeckFile = Program.ConvertDeckToDeserializableXml(inputFileInfo);
            _testDeck = Deck.GetFromFile(_xmlDeckFile);

            if (_testDeck.CardStack.Count < 1)
            {
                throw new Exception(string.Format("Failed to read any files from dek: '{0}'.", TestDeckFilePath));
            }
            Debug.WriteLine("Found {0} Cards.", _testDeck.CardStack.Count);
            foreach (Cards card in _testDeck.CardStack)
            {
                Debug.WriteLine(card.ToString());
            }
            //_webDriver = new FirefoxDriver();
        }

        [TestMethod]
        public void TestCardSearch()
        {
            Assert.IsTrue(_testDeck.CardStack.Count > 0);
            Assert.IsTrue(File.Exists(TestDeckFilePath));
            int sleepTimeMs = 4000;
            try
            {
                string url = @"http://gatherer.wizards.com/Pages/Default.aspx";
                _webDriver.Navigate().GoToUrl(url);
                string pattern = @"multiverseid=(\d+)";
                
                foreach (var card in _testDeck.CardStack)
                {
                    Thread.Sleep(sleepTimeMs);
                    var searchBox = _webDriver.FindElement(By.Id("ctl00_ctl00_MainContent_Content_SearchControls_CardSearchBoxParent_CardSearchBox"));
                    Assert.IsTrue(searchBox != null, "searchBox is NULL");

                    searchBox.Clear();
                    searchBox.SendKeys(string.Format("{0}{1}", card.Name, Environment.NewLine));
                    MatchCollection matches = Regex.Matches(_webDriver.Url, pattern);
                    Assert.IsTrue(matches.Count > 0, $"Failed to match '{_webDriver.Url}' using '{pattern}'");
                    string multiversid = matches[0].Groups[1].Value;
                    Assert.IsTrue(Int32.TryParse(multiversid, out var mid), $"Failed to Parse multiversid '{multiversid}'");

                    card.MultiverseID = mid;

                    var imageOnlyUrl = $@"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={multiversid}&type=card";

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

                    _webDriver.Navigate().Back();
                }

                _testDeck.SaveToFile(_xmlDeckFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _webDriver.Close();
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
