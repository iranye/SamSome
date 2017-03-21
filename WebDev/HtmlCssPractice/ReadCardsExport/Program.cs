 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReadCardsExport
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: {0} <input dek file>", "ReadCardsExport");
            }
            string dekFileName = args[0];
            if (!File.Exists(dekFileName))
            {
                Console.Error.WriteLine(string.Format("The file: '{0}' does not exist.", dekFileName));
                return;
            }
            FileInfo inputFileInfo = new FileInfo(dekFileName);
            //var xmlFile = GetXmlFileName(inputFileInfo);
            FileInfo xmlFile = ConvertDeckToDeserializableXml(inputFileInfo);
            Deck deck = Deck.GetFromFile(xmlFile);

            Console.WriteLine("Found {0} Cards.", deck.CardStack.Count);
            foreach (Cards card in deck.CardStack)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public static FileInfo ConvertDeckToDeserializableXml(FileInfo dekFileInfo)
        {
            var fileInfoOutput = GetXmlFileName(dekFileInfo);
            if (fileInfoOutput.Exists)
            {
                return fileInfoOutput;
            }
            using (FileStream inFileStream = new FileStream(dekFileInfo.FullName, FileMode.Open))
            {
                TextReader textReader = new StreamReader(inFileStream);
                using (StreamWriter fileOutWriter = new StreamWriter(fileInfoOutput.FullName))
                {
                    string line;
                    bool cardWasRead = false;
                    bool enclosingDeckTagReached = false;
                    while ((line = textReader.ReadLine()) != null)
                    {
                        if (enclosingDeckTagReached)
                        {
                            continue;
                        }
                        if (!cardWasRead && line.Contains("Cards"))
                        {
                            cardWasRead = true;
                            fileOutWriter.WriteLine("  <CardStack>");
                        }
                        if (cardWasRead && line.Contains("Cards"))
                        {
                            fileOutWriter.Write("  ");
                        }
                        if (cardWasRead && line.Contains("Deck"))
                        {
                            fileOutWriter.WriteLine("  </CardStack>");
                            enclosingDeckTagReached = true;
                        }
                        fileOutWriter.WriteLine(line);
                    }
                }
            }
            return fileInfoOutput;
        }

        public static Deck GetDeck(FileInfo xmlDeckFile)
        {
            Deck deck;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Deck));
            using (FileStream fs = new FileStream(xmlDeckFile.FullName, FileMode.Open))
            {
                deck = (Deck)xmlSerializer.Deserialize(fs);
            }
            return deck;
        }

        public static FileInfo GetXmlFileName(FileInfo fileInfo)
        {
            var fileNameParts = fileInfo.Name.Split('.');
            if (fileNameParts.Length < 2)
            {
                var newFile = new FileInfo(Path.Combine(fileInfo.Directory.FullName, string.Format("{0}.xml", fileInfo)));
                return newFile;
            }
            string xmlFile = "";
            for (int i = 0; i < fileNameParts.Length - 1; i++)
            {
                xmlFile += fileNameParts[i] + ".";
            }
            xmlFile += "xml";
            return new FileInfo(Path.Combine(fileInfo.DirectoryName, xmlFile));
        }
    }
    
    #region Deck
    [Serializable]
    public class Deck
    {
        public string NetDeckID { get; set; }
        public string PreconstructedDeckID { get; set; }

        public List<Cards> CardStack { get; set; }

        public static Deck GetFromFile(string filename)
        {
            return GetFromFile(new FileInfo(filename));
        }

        public static Deck GetFromFile(FileInfo fileInfo)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(Deck));
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                Deck newDeck = (Deck)xmls.Deserialize(fs);
                return newDeck;
            }
        }

        public void SaveToFile(FileInfo fileInfo)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(Deck));
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.OpenOrCreate))
            {
                xmls.Serialize(fs, this);
            }
        }
    }

    [Serializable]
    public class Cards
    {
        [XmlAttribute(AttributeName = "CatID")]
        public int CatID { get; set; }

        [XmlAttribute(AttributeName = "Quantity")]
        public int Quantity { get; set; }

        [XmlAttribute(AttributeName = "Sideboard")]
        public bool Sideboard { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "MultiverseID")]
        public int MultiverseID { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-CID:{1}-MID:{2}-{3}", Quantity.ToString("D3"), CatID.ToString("D6"), MultiverseID.ToString("D6"), Name);
        }
    } 
    #endregion
}
