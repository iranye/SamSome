using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager
{
    public class SongDatabase
    {
        Random mRandom = new Random();
        string[] mArtists = { "Metallica", "Elvis Presley", "Madonna", "The Beatles", "The Rolling Stones", "Abba" };
        string[] mSongTitles = { "Islands in the Stream", "Imagine", "Living on a Prayer", "Enter Sandman", "A Little Less Conversation", "Wonderful World" };

        public string GetRandomArtistName
        {
            get { return mArtists[mRandom.Next(mArtists.Length)]; }
        }

        public string GetRandomSongTitle
        {
            get { return mSongTitles[mRandom.Next(mSongTitles.Length)]; }
        }
    }
}
