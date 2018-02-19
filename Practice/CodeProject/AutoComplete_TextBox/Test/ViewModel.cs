using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Test
{
    [Serializable]
    public class ViewModel : INotifyPropertyChanged
    {
        private const int MaxSuggestionEntries = 128;
        
        [NonSerialized]
        private List<WaitMsg> mWaitMessage = new List<WaitMsg>() { new WaitMsg() };

        [NonSerialized]
        private string mQueryText = @"C:\";

        [NonSerialized]
        private IEnumerable mQueryCollection = null;

        [NonSerialized]
        private SortedList<string, int> mSuggestShare = new SortedList<string, int>();

        private List<string> mSuggestEntries = new List<string>();
        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> SuggestEntries
        {
            get
            {
                return mSuggestEntries;
            }

            set
            {
                mSuggestEntries = new List<string>();
                mSuggestShare = new SortedList<string, int>();

                if (value != null)
                {
                    for (int i = 0; i < value.Count && i < ViewModel.MaxSuggestionEntries; i++)
                    {
                        if (value[i] != null)
                        {
                            mSuggestEntries.Add(value[i].ToUpper());
                            mSuggestShare.Add(value[i].ToUpper(), 1);
                        }
                    }
                }

                OnPropertyChanged(typeof(ViewModel).GetProperty("SuggestEntries").Name);
            }
        }
        
        [System.Xml.Serialization.XmlIgnore]
        public IEnumerable WaitMessage
        {
            get { return mWaitMessage; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string QueryText
        {
            get
            {
                return mQueryText;
            }

            set
            {
                if (String.IsNullOrEmpty(value) || value.Length < 3)
                {
                    mQueryText = String.Empty;
                    return;
                }
                if (mQueryText != value)
                {
                    mQueryText = value;
                    OnPropertyChanged(typeof(ViewModel).GetProperty("QueryText").Name);
                    mQueryCollection = null;
                    OnPropertyChanged(typeof(ViewModel).GetProperty("QueryCollection").Name);
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnore]
        public IEnumerable QueryCollection
        {
            get
            {
                QueryList(QueryText);
                return (mQueryCollection == null ? new List<string>() : mQueryCollection);
            }
        }

        public bool GetServerFolderShare(string sInPath, out string sServerFolderShare)
        {
            sServerFolderShare = string.Empty;
            char cPathDel = '\\';

            if (sInPath == null) return false;

            if (sInPath.Length <= 8) return false;

            if (sInPath[0] != cPathDel || sInPath[1] != cPathDel) return false;

            int iServer = -1, iShare = -1;

            if ((iServer = sInPath.IndexOf(cPathDel, 2)) == -1) return false;

            if ((iShare = sInPath.IndexOf(cPathDel, iServer + 1)) == -1)
            {
                if (System.IO.Directory.Exists(sInPath))      // String is of the form: '\\Server\SharedFolder'
                {                                            // which is still OK
                    sServerFolderShare = sInPath.ToUpper() + cPathDel;
                    return true;
                }
                else
                    return false;
            }

            sServerFolderShare = sInPath.ToUpper().Substring(0, iShare + 1);
            return true;
        }
        
        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
        protected void AddListSuggest(string s)
        {
            if (s != null)
            {
                if (s.Trim().Length > 0)
                    if (mSuggestShare.ContainsKey(s) == false)
                    {
                        // Increment frequency if entry is already avaliable
                        // Otherwise, remove first or least frequency item and add new entry to keep list size at limit
                        if (mSuggestShare.Count < ViewModel.MaxSuggestionEntries)
                        {
                            mSuggestEntries.Add(s.ToUpper());
                            mSuggestShare.Add(s.ToUpper(), 1);
                        }
                        else
                        {
                            int iMinIdx = 0;
                            for (int i = 0; i < mSuggestShare.Count; i++)
                            {
                                if (mSuggestShare.Values[i] < mSuggestShare.Values[iMinIdx])
                                {
                                    iMinIdx = i;
                                }
                            }

                            mSuggestEntries.RemoveAt(iMinIdx);
                            mSuggestShare.RemoveAt(iMinIdx);

                            mSuggestEntries.Add(s.ToUpper());
                            mSuggestShare.Add(s.ToUpper(), 1);
                        }
                    }
                    else
                    {
                        mSuggestShare[s.ToUpper()] += 1;
                    }
            }
        }

        private void QueryList(string searchPath)
        {
            char sPathDel = System.IO.Path.DirectorySeparatorChar;
            string sServerFolderShare;

            List<string> sRet = null;

            try
            {
                // Simply use this to test whether query is asynchrone to UI or not
                ////System.Threading.Thread.Sleep(5000);

                // suggest list of drives if user has not typed anything, yet
                if ((searchPath == null ? string.Empty : searchPath).Length <= 1)
                {
                    sRet = new List<string>(System.IO.Directory.GetLogicalDrives());
                }
                else
                {
                    if ((searchPath == null ? string.Empty : searchPath).Length == 2)
                    {
                        if (searchPath[1] == ':')
                        {
                            sRet = new List<string>(System.IO.Directory.GetDirectories(searchPath + sPathDel));
                        }
                        else
                        {
                            // Suggest shares on a UNC path addressed computer
                            sRet = SuggestEntries;
                        }
                    }
                    else
                    {
                        bool bQueryUNCShares = false;
                        string sFileServer = string.Empty;

                        // Determine whether this is a UNC Path without any share (just the server portion of it)
                        if (searchPath.Length > 3)
                        {
                            if (searchPath[0] == '\\' && searchPath[1] == '\\' && searchPath[searchPath.Length - 1] == '\\' &&
                                 searchPath.Substring(2, searchPath.Length - 3).Contains("\\") == false)
                            {
                                bQueryUNCShares = true;
                                sFileServer = searchPath.Substring(2, searchPath.Length - 3);
                            }
                        }

                        if (bQueryUNCShares == true) // query for shared folders on a file server via UNC
                        {
                            ShareCollection sc = new ShareCollection(sFileServer);

                            if (sc.Count > 0)
                            {
                                sRet = new List<string>();
                                foreach (Share s in sc)
                                {
                                    if (s.IsFileSystem)
                                    {
                                        sRet.Add(s.ToString());

                                        // Add server to list of suggestions for next time when accessing this UNC address
                                        AddListSuggest("\\\\" + s.Server + "\\");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (System.IO.Directory.Exists(searchPath))
                            {
                                sRet = new List<string>(System.IO.Directory.GetDirectories(searchPath));

                                if (sRet.Count > 0)
                                    if (GetServerFolderShare(searchPath, out sServerFolderShare))
                                    {
                                        // Add server + share to list of suggestions for next time when accessing this address
                                        AddListSuggest(sServerFolderShare);
                                    }
                            }
                            else
                            {
                                int idx = searchPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);

                                if (idx > 0)
                                {
                                    string sParentDir = searchPath.Substring(0, idx + 1);
                                    string sSearchPattern = searchPath.Substring(idx + 1) + "*";
                                    sRet = new List<string>(System.IO.Directory.GetDirectories(sParentDir, sSearchPattern));
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
            bool bChange = (sRet == null && mQueryCollection != null) ||
                            (sRet != null && mQueryCollection == null);

            if (sRet != null && mQueryCollection != null)
                bChange = (sRet == mQueryCollection);

            if (bChange == true)
                mQueryCollection = sRet;
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
