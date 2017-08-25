using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MicroMvvm;

namespace TestPort
{
    class TestSuiteViewModel
    {
        private string rootJsonDir = @"C:\TMP\FileIOTesting";

        ObservableCollection<TestConfigViewModel> mTests = new ObservableCollection<TestConfigViewModel>();
        public ObservableCollection<TestConfigViewModel> Tests
        {
            get { return mTests; }
            set { mTests = value; }
        }

        public StatusViewModel StatusViewModel { get; set; }

        ObservableCollection<TestConfig> mFiledTestConfigs = new ObservableCollection<TestConfig>();
        public ObservableCollection<TestConfig> FiledTestConfigs
        {
            get { return mFiledTestConfigs; }
            set { mFiledTestConfigs = value; }
        }

        private TestConfig mSelectedTestConfig = new TestConfig();
        public TestConfig SelectedTestConfig
        {
            get { return mSelectedTestConfig; }
            set
            {
                if (mSelectedTestConfig != value)
                {
                    mSelectedTestConfig = value;
                    if (!String.IsNullOrEmpty(mSelectedTestConfig.FileName))
                    {
                        StatusViewModel.AddLogMessage($"Selected Filed Test '{mSelectedTestConfig.Name}'");
                    }
                }
            }
        }

        private TestConfigViewModel mSelectedItem;
        public TestConfigViewModel SelectedItem
        {
            get { return mSelectedItem; }
            set
            {
                if (mSelectedItem != value)
                {
                    mSelectedItem = value;
                    if (mSelectedItem != null)
                    {
                        StatusViewModel.AddLogMessage($"Opened the Test '{mSelectedItem.TestName}'");
                    }
                }
            }
        }

        public TestSuiteViewModel()
        {
            mTests.Add(new TestConfigViewModel
            {
                TestName = "CreateTextAttribute",
                TestOperation = TestOperation.AttributeCreate
            });
            mTests.Add(new TestConfigViewModel
            {
                TestName = "CreateNumericAttribute",
                TestOperation = TestOperation.AttributeCreate
            });
            StatusViewModel = new StatusViewModel();

            FiledTestConfigs.Add(new TestConfig { Name = "(Select a saved Test Config)", FileName = "" });
            if (!String.IsNullOrEmpty(rootJsonDir) && Directory.Exists(rootJsonDir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(rootJsonDir);
                foreach (var fileInfo in dirInfo.GetFiles("*.json"))
                {
                    var testConfigName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    FiledTestConfigs.Add(new TestConfig { Name = testConfigName, FileName = fileInfo.Name });
                }
            }
            if (!Directory.Exists(rootJsonDir))
            {
                Directory.CreateDirectory(rootJsonDir);
            }
        }

        #region UpdateTestNames
        void UpdateTestNamesExecute()
        {
            throw new NotImplementedException("TestDatabase is deleted.");
        }

        bool CanUpdateTestNamesExecute()
        {
            return true;
        }

        public ICommand UpdateTestNames
        {
            get { return new RelayCommand(UpdateTestNamesExecute, CanUpdateTestNamesExecute); }
        }
        #endregion

        void DumpJsonExecute()
        {
            if (SelectedItem == null)
            {
                return;
            }
            SelectedItem.UpdateJsonText();
            if (SelectedItem != null && SelectedItem.JsonDump.Length > 0)
            {
                Clipboard.SetText(SelectedItem.JsonDump);
            }
        }

        bool CanDumpJsonExecute()
        {
            bool dumpJsonEnabled = SelectedItem != null;
            if (!dumpJsonEnabled)
            {
                return false;
            }
            switch (SelectedItem.TestOperation)
            {
                    case TestOperation.AttributeCreate:
                    dumpJsonEnabled = dumpJsonEnabled && SelectedItem.DataLength > 0;
                    dumpJsonEnabled = dumpJsonEnabled && !string.IsNullOrEmpty(SelectedItem.HelpText);
                    break;
            }

            return dumpJsonEnabled;
        }

        public ICommand DumpJson
        {
            get { return new RelayCommand(DumpJsonExecute, CanDumpJsonExecute); }
        }

        void SaveJsonExecute()
        {
            if (SelectedItem == null)
            {
                return;
            }

            string fileName = "";
            string filePath = "";
            if (!String.IsNullOrEmpty(SelectedItem.FileName))
            {
                fileName = SelectedItem.FileName;
                filePath = Path.Combine(rootJsonDir, SelectedItem.FileName);
            }
            else
            {
                fileName = SelectedItem.TestName + ".json";
                filePath = Path.Combine(rootJsonDir, fileName);
                if (File.Exists(filePath))
                {
                    StatusViewModel.AddLogMessage($"File for Test Config '{filePath}' already exists.");
                    return;
                }
            }

            PersistingData.ReadWriteJson.WriteSingleInstanceToJson(SelectedItem, filePath);
            StatusViewModel.AddLogMessage($"Wrote Test '{SelectedItem.TestName}' to JSON file: '{filePath}'");
            FiledTestConfigs.Add(new TestConfig {Name = SelectedItem.TestName, FileName = fileName});
        }

        bool CanSaveJsonExecute()
        {
            return SelectedItem != null;
        }

        public ICommand SaveJson
        {
            get { return new RelayCommand(SaveJsonExecute, CanSaveJsonExecute); }
        }
        
        void ReadJsonExecute()
        {
            if (SelectedTestConfig == null || String.IsNullOrEmpty(SelectedTestConfig.FileName))
            {
                return;
            }

            string filePath = Path.Combine(rootJsonDir, SelectedTestConfig.FileName);

            try
            {
                TestConfigViewModel testConfigViewModel = PersistingData.ReadWriteJson.ReadSingleInstance<TestConfigViewModel>(filePath);
                testConfigViewModel.FileName = SelectedTestConfig.FileName;
                TestConfigViewModel toReplace = null;
                foreach (var el in Tests)
                {
                    if (el.FileName == testConfigViewModel.FileName)
                    {
                        toReplace = el;
                        break;
                    }
                }
                if (toReplace != null)
                {
                    Tests.Remove(toReplace);
                }
                Tests.Add(testConfigViewModel);
                SelectedItem = testConfigViewModel;
                StatusViewModel.AddLogMessage($"Read Test Config '{SelectedTestConfig.Name}' from JSON file: '{filePath}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                StatusViewModel.AddLogMessage($"Error reading Test Config from JSON file: '{filePath}'");
            }
        }

        bool CanReadJsonExecute()
        {
            return !String.IsNullOrEmpty(SelectedTestConfig.FileName);
        }

        public ICommand ReadJson
        {
            get { return new RelayCommand(ReadJsonExecute, CanReadJsonExecute); }
        }

        void ClearLogMessagesExecute()
        {
            StatusViewModel.ClearLog();
        }

        bool CanClearLogMessagesExecute()
        {
            return true;
        }

        public ICommand ClearLogMessages
        {
            get { return new RelayCommand(ClearLogMessagesExecute, CanClearLogMessagesExecute); }
        }
    }
}
