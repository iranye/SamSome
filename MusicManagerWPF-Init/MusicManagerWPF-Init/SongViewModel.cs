using System.ComponentModel;
using System.Windows.Input;
using MicroMvvm;

namespace MusicManagerWPF_Init
{
    /// <summary>
    /// This class is a view model of a song.
    /// </summary>
    public class SongViewModel : ObservableObject
    {
        /// <summary>
        /// Constructs the default instance of a SongViewModel
        /// </summary>
        public SongViewModel()
        {
            mSong = new Song { ArtistName = "Unknown", SongTitle = "Unknown" };
        }
        
        Song mSong;
        private int mCount = 0;

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

        public string SongTitle
        {
            get { return Song.SongTitle; }
            set
            {
                if (Song.SongTitle != value)
                {
                    Song.SongTitle = value;
                    RaisePropertyChanged("SongTitle");
                }
            }
        }

        void UpdateArtistNameExecute()
        {
            ++mCount;
            ArtistName = string.Format("Elvis ({0})", mCount);
        }

        bool CanUpdateArtistNameExecute()
        {
            return true;
        }

        public ICommand UpdateArtistName { get { return new RelayCommand(UpdateArtistNameExecute, CanUpdateArtistNameExecute);} }
    }
}
