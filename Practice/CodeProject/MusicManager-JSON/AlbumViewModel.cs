using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;

namespace MusicManager
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        private SongDatabase mDatabase = new SongDatabase();

        public ObservableCollection<SongViewModel> SongViewModels { get; set; }

        private string mStatusLog;
        public string StatusLog
        {
            get { return mStatusLog; }
            set
            {
                if (mStatusLog != value)
                {
                    mStatusLog = value;
                    OnPropertyChanged("StatusLog");
                }
            }
        }

        private string mDirectoryPath;
        public string DirectoryPath
        {
            get { return mDirectoryPath; }
            set
            {
                if (mDirectoryPath != value)
                {
                    mDirectoryPath = value;
                    FullPath = Path.Combine(mDirectoryPath, mFileName);
                    OnPropertyChanged("DirectoryPath");
                }
            }
        }

        private string mFileName;
        public string FileName
        {
            get { return mFileName; }
            set
            {
                if (mFileName != value)
                {
                    mFileName = value;
                    FullPath = Path.Combine(mDirectoryPath, mFileName);
                    OnPropertyChanged("FileName");
                }
            }
        }

        private string mFullPath;
        public string FullPath
        {
            get { return mFullPath; }
            set
            {
                if (mFullPath != value)
                {
                    mFullPath = value;
                    OnPropertyChanged("FullPath");
                }
            }
        }

        public AlbumViewModel()
        {
            SongViewModels = new ObservableCollection<SongViewModel>();
            for (int i = 0; i < 3; i++)
            {
                var song = new Song
                {
                    ArtistName = mDatabase.GetRandomArtistName,
                    SongTitle = mDatabase.GetRandomSongTitle
                };
                SongViewModels.Add(new SongViewModel{Song = song});
            }
            string mStatusLog = "";
            mDirectoryPath = @"C:\tmp\Configs";
            mFileName = "file.json";
            FullPath = Path.Combine(mDirectoryPath, mFileName);
        }

        #region Add Artist & Song
        void AddAlbumArtistsExecute()
        {
            if (SongViewModels == null)
            {
                return;
            }

            var song = new Song
            {
                ArtistName = mDatabase.GetRandomArtistName,
                SongTitle = mDatabase.GetRandomSongTitle
            };
            SongViewModels.Add(new SongViewModel { Song = song });
        }

        bool CanAddAlbumArtistsExecute()
        {
            return true;
        }

        public ICommand AddAlbumArtist { get { return new RelayCommand(AddAlbumArtistsExecute, CanAddAlbumArtistsExecute); } } 
        #endregion

        #region Update Artists
        void UpdateAlbumArtistsExecute()
        {
            if (SongViewModels == null)
            {
                return;
            }

            foreach (var song in SongViewModels)
            {
                song.ArtistName = mDatabase.GetRandomArtistName;
            }
        }

        bool CanUpdateAlbumArtistsExecute()
        {
            return true;
        }

        public ICommand UpdateAlbumArtists { get { return new RelayCommand(UpdateAlbumArtistsExecute, CanUpdateAlbumArtistsExecute); } } 
        #endregion

        #region Update Song Titles
        void UpdateSongTitlesExecute()
        {
            if (SongViewModels == null)
            {
                return;
            }
            foreach (var songViewModel in SongViewModels)
            {
                songViewModel.SongTitle = mDatabase.GetRandomSongTitle;
            }
        }

        bool CanUpdateSongTitlesExecute()
        {
            return true;
        }

        private void LogMessage(string message)
        {
            StatusLog += message + Environment.NewLine;
        }

        public ICommand UpdateSongTitles { get { return new RelayCommand(UpdateSongTitlesExecute, CanUpdateSongTitlesExecute); } }
        #endregion

        #region Save Data
        void SaveDataExecute()
        {
            if (SongViewModels == null)
            {
                return;
            }

            string fullPath = Path.Combine(DirectoryPath, FileName);
            LogMessage($"Checking path: '{fullPath}'");
            FileInfo fileInfo = new FileInfo(fullPath);
            try
            {
                if (!Directory.Exists(mDirectoryPath))
                {
                    throw new Exception($"Directory not found: '{DirectoryPath}'");
                }
                if (fileInfo.Exists)
                {
                    if (fileInfo.IsReadOnly)
                    {
                        throw new Exception($"File at '{fullPath}' is read-only");
                    }
                }

                DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
                if (dirInfo.Exists)
                {
                    throw new Exception($"'{fullPath}' is a directory");
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
                return;
            }

            List<Song> songs = new List<Song>();
            foreach (var songViewModel in SongViewModels)
            {
                songs.Add(songViewModel.Song.Copy());
            }
            LogMessage($"Saving {songs.Count} Songs to file or DB");

            File.WriteAllText(fileInfo.FullName, JsonConvert.SerializeObject(songs.ToArray()));
            LogMessage($"Wrote Data to JSON file: '{fileInfo.FullName}'");
        }

        bool CanSaveDataExecute()
        {
            return true;
        }

        public ICommand SaveData { get { return new RelayCommand(SaveDataExecute, CanSaveDataExecute); } }
        #endregion

        #region Read Data
        void ReadDataExecute()
        {
            if (SongViewModels == null)
            {
                return;
            }

            string fullPath = Path.Combine(DirectoryPath, FileName);
            LogMessage($"Checking path: '{fullPath}'");
            FileInfo fileInfo = new FileInfo(fullPath);
            try
            {
                if (!Directory.Exists(mDirectoryPath))
                {
                    throw new Exception($"Directory not found: '{DirectoryPath}'");
                }
                if (!fileInfo.Exists)
                {
                    if (fileInfo.IsReadOnly)
                    {
                        throw new Exception($"File not found '{fullPath}'");
                    }
                }

                DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
                if (dirInfo.Exists)
                {
                    throw new Exception($"'{fullPath}' is a directory");
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
                return;
            }

            List<Song> songs = ReadCollectionData<Song>(fileInfo.FullName);
            if (songs == null)
            {
                return;
            }
            if (songs.Count == 0)
            {
                LogMessage("Failed to read any Songs from file or DB");
                return;
            }

            SongViewModels.Clear();
            foreach (var song in songs)
            {
                SongViewModels.Add(new SongViewModel {Song = song});
            }
            LogMessage($"{songs.Count} Songs read from file or DB");
        }

        private static List<T> ReadCollectionData<T>(string fileName) where T : new()
        {
            List<T> objCol = new List<T>();
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                objCol = JsonConvert.DeserializeObject<List<T>>(json);
            }
            return objCol;
        }

        bool CanReadDataExecute()
        {
            return true;
        }

        public ICommand ReadData { get { return new RelayCommand(ReadDataExecute, CanReadDataExecute); } }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
