using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManagerWPF
{
    public class Song
    {
        private string mArtistName;
        public string ArtistName
        {
            get { return mArtistName; }
            set
            {
                if (mArtistName != value)
                {
                    mArtistName = value;
                }
            }
        }

        private string mSongTitle;
        public string SongTitle
        {
            get { return mSongTitle; }
            set
            {
                if (mSongTitle != value)
                {
                    mSongTitle = value;
                }
            }
        }
        
    }
}
