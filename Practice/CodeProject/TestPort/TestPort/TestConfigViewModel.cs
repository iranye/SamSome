using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MicroMvvm;
using Newtonsoft.Json;

namespace TestPort
{
    public class TestConfigViewModel : ObservableObject
    {
        #region Properties
        private TestConfig mTestConfig;

        [JsonIgnore]
        public TestConfig TestConfig
        {
            get { return mTestConfig; }
            set { mTestConfig = value; }
        }

        public string TestName
        {
            get { return mTestConfig.Name; }
            set
            {
                if (mTestConfig.Name != value)
                {
                    mTestConfig.Name = value;
                    FileName = value + ".json";
                    NotifyPropertyChanged("TestName");
                }
            }
        }
        
        [JsonIgnore]
        public string FileName
        {
            get { return mTestConfig.FileName; }
            set
            {
                if (mTestConfig.FileName != value)
                {
                    mTestConfig.FileName = value;
                    NotifyPropertyChanged("FileName");
                }
            }
        }
        #endregion

        public override string ToString()
        {
            return TestName;
        }

        #region SPEED API Support Properties
        private SpeedApiObject mSpeedApiObject;
        public SpeedApiObject SpeedApiObject
        {
            get
            {
                if (mSpeedApiObject == null)
                {
                    mSpeedApiObject = new SpeedApiObject();
                    mSpeedApiObject.TestType = TestType.Text;
                    mSpeedApiObject.TestStatus = TestStatus.Normal;
                    mSpeedApiObject.IsActive = true;
                }
                return mSpeedApiObject;
            }
            set
            {
                if (mSpeedApiObject != value)
                {
                    mSpeedApiObject = value;
                    NotifyPropertyChanged("SpeedApiObject");
                }
            }
        }

        private TestOperation mTestOperation;
        public TestOperation TestOperation
        {
            get { return mTestOperation; }
            set
            {
                if (mTestOperation != value)
                {
                    mTestOperation = value;
                    NotifyPropertyChanged("TestOperation");
                }
            }
        }

        [JsonIgnore]
        public int AttributeId
        {
            get
            {
                if (mSpeedApiObject != null)
                {
                    return SpeedApiObject.AttributeId;
                }
                return 0;
            }
            set
            {
                if (mSpeedApiObject != null && mSpeedApiObject.AttributeId != value)
                {
                    mSpeedApiObject.AttributeId = value;
                    NotifyPropertyChanged("AttributeId");
                }
            }
        }

        [JsonIgnore]
        public string AttributeName
        {
            get
            {
                if (mSpeedApiObject != null)
                {
                    return SpeedApiObject.AttributeDsc;
                }
                return String.Empty;
            }
            set
            {
                if (mSpeedApiObject != null && mSpeedApiObject.AttributeDsc != value)
                {
                    mSpeedApiObject.AttributeDsc = value;
                }
            }
        }
        
        [JsonIgnore]
        public List<ClassAttribute> ClassAttributeList
        {
            get
            {
                if(SpeedApiObject.ClassAttributeList == null)
                {
                    SpeedApiObject.ClassAttributeList = new List<ClassAttribute>();
                }
                return SpeedApiObject.ClassAttributeList;
            }
            set
            {
                if (SpeedApiObject.ClassAttributeList != value)
                {
                    SpeedApiObject.ClassAttributeList = value;
                }
            }
        }

        [JsonIgnore]
        public int DataLength
        {
            get
            {
                if (mSpeedApiObject != null)
                {
                    return SpeedApiObject.DataLength;
                }
                return 0;
            }
            set
            {
                if (mSpeedApiObject != null && mSpeedApiObject.DataLength != value)
                {
                    mSpeedApiObject.DataLength = value;
                    NotifyPropertyChanged("DataLength");
                }
            }
        }

        [JsonIgnore]
        public bool IsActive
        {
            get
            {
                if (mSpeedApiObject != null)
                {
                    return SpeedApiObject.IsActive;
                }
                return true;
            }
            set
            {
                if (mSpeedApiObject != null && mSpeedApiObject.IsActive != value)
                {
                    mSpeedApiObject.IsActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
        }

        [JsonIgnore]
        public TestStatus TestStatus
        {
            get
            {
                if (mSpeedApiObject != null)
                {
                    return SpeedApiObject.TestStatus;
                }
                return TestStatus.Normal;
            }
            set
            {
                if (mSpeedApiObject != null && mSpeedApiObject.TestStatus != value)
                {
                    mSpeedApiObject.TestStatus = value;
                    NotifyPropertyChanged("TestStatus");
                }
            }
        }

        [JsonIgnore]
        public TestType TestType
        {
            get
            {
                if (SpeedApiObject != null)
                {
                    return SpeedApiObject.TestType;
                }
                return TestType.Text;
            }
            set
            {
                if (mSpeedApiObject != null && mSpeedApiObject.TestType != value)
                {
                    mSpeedApiObject.TestType = value;
                    NotifyPropertyChanged("TestType");
                }
            }
        }

        [JsonIgnore]
        public string HelpText
        {
            get
            {
                if (mSpeedApiObject != null)
                {
                    return SpeedApiObject.HelpTxt;
                }
                return String.Empty;
            }
            set
            {
                if (mSpeedApiObject != null && mSpeedApiObject.HelpTxt != value)
                {
                    mSpeedApiObject.HelpTxt = value;
                }
            }
        }

        private string mJsonDump;
        [JsonIgnore]
        public string JsonDump
        {
            get { return mJsonDump; }
            set
            {
                mJsonDump = value;
                NotifyPropertyChanged("JsonDump");
            }
        }
        #endregion

        public TestConfigViewModel()
        {
            mJsonDump = String.Empty;
            mTestConfig = new TestConfig();
        }
        
        public void UpdateJsonText()
        {
            if (SpeedApiObject == null)
            {
                JsonDump = "Null SPEED API object";
                return;
            }
            JsonDump = $"[{Environment.NewLine}{JsonConvert.SerializeObject(SpeedApiObject, Formatting.Indented)}{Environment.NewLine}]";
        }
    }
}
