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
            var fileSnippet = "Snippet.xml";
            XmlSerializer xmls = new XmlSerializer(typeof(Cards));
            using (FileStream fs = new FileStream(fileSnippet, FileMode.Open))
            {
                Cards myCards = (Cards)xmls.Deserialize(fs);
            }
            Deck deck = MyDeck;
        }
    }

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
}
