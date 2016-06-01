using System.Collections.ObjectModel;
using System.Windows.Input;
using MicroMvvm;

namespace MusicManagerWPF
{
    class AlbumViewModel
    {
        private int mCount = 0;
        private SongDatabase mSongDatabase = new SongDatabase();
        ObservableCollection<SongViewModel> mSongs = new ObservableCollection<SongViewModel>();

        public ObservableCollection<SongViewModel> Songs
        {
            get { return mSongs; }
            set { mSongs = value; }
        }

        public AlbumViewModel()
        {
            for (int i = 0; i < 3; i++)
            {
                mSongs.Add(new SongViewModel
                {
                    Song =
                        new Song
                        {
                            ArtistName = mSongDatabase.GetRandomArtistName,
                            SongTitle = mSongDatabase.GetRandomSongTitle
                        }
                });
            }
        }
        
        #region UpdateAlbumArtists
        void UpdateAlbumArtistsExecute()
        {
            if (mSongs == null)
                return;
            ++mCount;
            foreach (var song in mSongs)
            {
                song.ArtistName = mSongDatabase.GetRandomArtistName;
            }
        }

        bool CanUpdateAlbumArtistsExecute()
        {
            return true;
        }
        public ICommand UpdateAlbumArtists { get { return new RelayCommand(UpdateAlbumArtistsExecute, CanUpdateAlbumArtistsExecute); } }
        #endregion

        #region AddAlbumArtist
        void AddAlbumArtistExecute()
        {
            if (mSongs == null)
                return;
            mSongs.Add(new SongViewModel { Song = new Song { ArtistName = mSongDatabase.GetRandomArtistName, SongTitle = mSongDatabase.GetRandomSongTitle } });
        }

        bool CanAddAlbumArtistExecute()
        {
            return true;
        }

        public ICommand AddAlbumArtist { get { return new RelayCommand(AddAlbumArtistExecute, CanAddAlbumArtistExecute); } }
        #endregion

        #region UpdateSongTitles
        void UpdateSongTitlesExecute()
        {
            if (mSongs == null)
                return;
            ++mCount;
            foreach (var song in mSongs)
            {
                song.SongTitle = mSongDatabase.GetRandomSongTitle;
            }
        }

        bool CanUpdateSongTitlesExecute()
        {
            return true;
        }

        public ICommand UpdateSongTitles { get { return new RelayCommand(UpdateSongTitlesExecute, CanUpdateSongTitlesExecute); } } 
        #endregion
    }
}
