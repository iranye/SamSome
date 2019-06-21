using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtg.Classes
{
    public enum CardType
    {
        Unknown = 2,
        Land = 4,
        Artifact = 8,
        Creature = 16,
        Enchantment = 64,
        Instant = 128,
        Sorcery = 256
    }

    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CardType CardType { get; set; }
        public string CatID { get; set; }
        public string ArenaID { get; set; }
        public Expansion Expansion { get; set; }
        public int ExpansionId { get; set; }

        public bool IsBanned { get; set; }
        public override string ToString()
        {
            string expansion = Expansion == null ? ExpansionId.ToString() : Expansion.ToString();
            return $"[{Id}] {Name} ({CardType}) in Expansion: {expansion}";
        }
    }

    public class Expansion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExpansionCode { get; set; }
        public int Formats { get; set; }
        public DateTime ReleaseDate { get; set; }

        public override string ToString()
        {
            return $"Id: [{Id}], Name: '{Name}', ExpansionCode: '{ExpansionCode}'";
        }
    }
}
