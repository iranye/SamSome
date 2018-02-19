using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace Test
{
    /*
     * XAML uses:
     * QueryText
     * QueryCollection
     * 
     * Window1.cs uses:
     * SuggestEntries
     */


    [Serializable]
    public class ViewModel : INotifyPropertyChanged
    {
        private const int MaxSuggestionEntries = 128;
        
        [NonSerialized]
        private List<WaitMsg> _waitMessage = new List<WaitMsg>() { new WaitMsg() };

        [XmlIgnore]
        public IEnumerable WaitMessage
        {
            get { return _waitMessage; }
        }

        [NonSerialized]
        private string _queryText = @"C:\";

        [XmlIgnore]
        public string QueryText
        {
            get
            {
                return _queryText;
            }

            set
            {
                if (String.IsNullOrEmpty(value) || value.Length < 3)
                {
                    _queryText = String.Empty;
                    return;
                }
                if (_queryText != value)
                {
                    _queryText = value;
                    OnPropertyChanged(typeof(ViewModel).GetProperty("QueryText").Name);
                    _queryCollection = null;
                    OnPropertyChanged(typeof(ViewModel).GetProperty("QueryCollection").Name);
                }
            }
        }
        
        [NonSerialized]
        private IEnumerable _queryCollection = null;

        [XmlIgnore]
        public IEnumerable QueryCollection
        {
            get
            {
                QueryList(QueryText);
                return (_queryCollection == null ? new List<string>() : _queryCollection);
            }
        }

        [NonSerialized]
        private SortedList<string, int> _suggestShare = new SortedList<string, int>();

        private List<string> _suggestEntries = new List<string>();
        public List<string> SuggestEntries
        {
            get
            {
                return _suggestEntries;
            }

            set
            {
                _suggestEntries = new List<string>();
                _suggestShare = new SortedList<string, int>();

                if (value != null)
                {
                    for (int i = 0; i < value.Count && i < ViewModel.MaxSuggestionEntries; i++)
                    {
                        if (value[i] != null)
                        {
                            _suggestEntries.Add(value[i].ToUpper());
                            _suggestShare.Add(value[i].ToUpper(), 1);
                        }
                    }
                }

                OnPropertyChanged(typeof(ViewModel).GetProperty("SuggestEntries").Name);
            }
        }

        private bool IsServerFolderShare(string path, out string serverFolderShare)
        {
            serverFolderShare = string.Empty;
            if (path == null || path.Length <= 8)
            {
                return false;
            }

            char pathSeparatorChar = Path.DirectorySeparatorChar;
            if (path[0] != pathSeparatorChar || path[1] != pathSeparatorChar) return false;

            int serverInd = -1;
            int shareIndex = -1;

            if ((serverInd = path.IndexOf(pathSeparatorChar, 2)) == -1) return false;

            if ((shareIndex = path.IndexOf(pathSeparatorChar, serverInd + 1)) == -1)
            {
                if (Directory.Exists(path))      // String is of the form: '\\Server\SharedFolder'
                {                                            // which is still OK
                    serverFolderShare = path.ToUpper() + pathSeparatorChar;
                    return true;
                }
                else
                    return false;
            }

            serverFolderShare = path.ToUpper().Substring(0, shareIndex + 1);
            return true;
        }
        
        protected void AddListSuggest(string sharePath)
        {
            if (sharePath != null)
            {
                if (sharePath.Trim().Length > 0)
                    if (_suggestShare.ContainsKey(sharePath) == false)
                    {
                        // Increment frequency if entry is already avaliable
                        // Otherwise, remove first or least frequency item and add new entry to keep list size at limit
                        if (_suggestShare.Count < ViewModel.MaxSuggestionEntries)
                        {
                            _suggestEntries.Add(sharePath.ToUpper());
                            _suggestShare.Add(sharePath.ToUpper(), 1);
                        }
                        else
                        {
                            int iMinIdx = 0;
                            for (int i = 0; i < _suggestShare.Count; i++)
                            {
                                if (_suggestShare.Values[i] < _suggestShare.Values[iMinIdx])
                                {
                                    iMinIdx = i;
                                }
                            }

                            _suggestEntries.RemoveAt(iMinIdx);
                            _suggestShare.RemoveAt(iMinIdx);

                            _suggestEntries.Add(sharePath.ToUpper());
                            _suggestShare.Add(sharePath.ToUpper(), 1);
                        }
                    }
                    else
                    {
                        _suggestShare[sharePath.ToUpper()] += 1;
                    }
            }
        }

        private void QueryList(string searchPath)
        {
            char pathSeparatorChar = Path.DirectorySeparatorChar;
            string serverFolderShare;

            List<string> listOfPaths = null;

            try
            {
                // Simply use this to test whether query is asynchrone to UI or not
                //System.Threading.Thread.Sleep(5000);

                // suggest list of drives if user has not typed anything, yet
                if ((searchPath == null ? string.Empty : searchPath).Length <= 1)
                {
                    listOfPaths = new List<string>(Directory.GetLogicalDrives());
                }
                else
                {
                    if ((searchPath == null ? string.Empty : searchPath).Length == 2)
                    {
                        if (searchPath[1] == ':')
                        {
                            listOfPaths = new List<string>(Directory.GetDirectories(searchPath + pathSeparatorChar));
                        }
                        else
                        {
                            // Suggest shares on a UNC path addressed computer
                            listOfPaths = SuggestEntries;
                        }
                    }
                    else
                    {
                        bool bQueryUNCShares = false;
                        string fileServerPath = string.Empty;

                        // Determine whether this is a UNC Path without any share (just the server portion of it)
                        if (searchPath.Length > 3)
                        {
                            if (searchPath[0] == pathSeparatorChar 
                                && searchPath[1] == pathSeparatorChar
                                && searchPath[searchPath.Length - 1] == pathSeparatorChar
                                && searchPath.Substring(2, searchPath.Length - 3).Contains("\\") == false)
                            {
                                bQueryUNCShares = true;
                                fileServerPath = searchPath.Substring(2, searchPath.Length - 3);
                            }
                        }

                        if (bQueryUNCShares == true) // query for shared folders on a file server via UNC
                        {
                            ShareCollection shareCollection = new ShareCollection(fileServerPath);

                            if (shareCollection.Count > 0)
                            {
                                listOfPaths = new List<string>();
                                foreach (Share s in shareCollection)
                                {
                                    if (s.IsFileSystem)
                                    {
                                        listOfPaths.Add(s.ToString());

                                        // Add server to list of suggestions for next time when accessing this UNC address
                                        AddListSuggest("\\\\" + s.Server + "\\");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Directory.Exists(searchPath))
                            {
                                listOfPaths = new List<string>(Directory.GetDirectories(searchPath));

                                if (listOfPaths.Count > 0)
                                    if (IsServerFolderShare(searchPath, out serverFolderShare))
                                    {
                                        // Add server + share to list of suggestions for next time when accessing this address
                                        AddListSuggest(serverFolderShare);
                                    }
                            }
                            else
                            {
                                int idx = searchPath.LastIndexOf(Path.DirectorySeparatorChar);

                                if (idx > 0)
                                {
                                    string sParentDir = searchPath.Substring(0, idx + 1);
                                    string sSearchPattern = searchPath.Substring(idx + 1) + "*";
                                    listOfPaths = new List<string>(Directory.GetDirectories(sParentDir, sSearchPattern));
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            // Determine whether collection changed to comparison to current data
            // and change it only if it is indeed different
            // (this saves messaging and event overheads that would othwise produce
            //  the same results multiple times causing overheads and oddities nobody needs...
            bool bChange = (listOfPaths == null && _queryCollection != null) ||
                            (listOfPaths != null && _queryCollection == null);

            if (listOfPaths != null && _queryCollection != null)
                bChange = (listOfPaths == _queryCollection);

            if (bChange == true)
                _queryCollection = listOfPaths;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        internal class WaitMsg
        {
            public override string ToString()
            {
                return "Please Wait...";
            }
        }
    }
}
