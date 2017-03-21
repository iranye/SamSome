 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReadCardsExport
{
    class Program
    {
        const string filename = @"Arty.dek";
        public static Deck MyDeck
        {
            get
            {
                return Deck.GetFromFile(filename);
            }
        }

        static void Main(string[] args)
        {
            MassageXmlStuff();
            DeckStuff();
        }

        private static void DeckStuff()
        {
            var xmlDeckFile = "Arty.xml";
            Deck deck;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Deck));
            using (FileStream fs = new FileStream(xmlDeckFile, FileMode.Open))
            {
                deck = (Deck) xmlSerializer.Deserialize(fs);
            }
        }

        private static void MassageXmlStuff()
        {
            var dekFile = "Arty.dek";
            var fileOutput = "Arty.xml";
            using (FileStream inFileStream = new FileStream(dekFile, FileMode.Open))
            {
                TextReader textReader = new StreamReader(inFileStream);
                using (StreamWriter fileOutWriter = new StreamWriter(fileOutput))
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
            XmlSerializer xmls = new XmlSerializer(typeof(Deck));
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                Deck newDeck = (Deck)xmls.Deserialize(fs);
                return newDeck;
            }
        }
    }

    [Serializable]
    public class Cards
    {
        [XmlAttribute(AttributeName = "CatID")]
        public string CatID { get; set; }

        [XmlAttribute(AttributeName = "Quantity")]
        public int Quantity { get; set; }

        [XmlAttribute(AttributeName = "Sideboard")]
        public bool Sideboard { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    } 
    #endregion
}
