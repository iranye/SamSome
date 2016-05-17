
using System.ComponentModel;

namespace MusicManagerWPF_Init
{
    /// <summary>
    /// This class is a view model of a song.
    /// </summary>
    public class SongViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructs the default instance of a SongViewModel
        /// </summary>
        public SongViewModel()
        {
            mSong = new Song { ArtistName = "Unknown", SongTitle = "Unknown" };
        }
        
        Song mSong; 

        public Song Song
        {
            get
            {
                return mSong;
            }
            set
            {
                mSong = value;
            }
        }

        public string ArtistName
        {
            get { return Song.ArtistName; }
            set
            {
                if (Song.ArtistName != value)
                {
                    Song.ArtistName = value;
                    RaisePropertyChanged("ArtistName");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
