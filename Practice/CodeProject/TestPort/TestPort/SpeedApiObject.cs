using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TestPort
{
    public enum TestOperation
    {
        Unknown=0,
        AttributeCreate,
        AttributesGet,
        AttributeUpdate,
        AttributeUpdateNext
    }

    public enum TestType
    {
        Unknown=0,
        Text=1,
        Numeric
    }

    public enum TestStatus
    {
        Unknown=0,
        Normal=1
    }

    public class SpeedApiObject
    {
        [JsonIgnore]
        public bool IsActive { get; set; }
        public string ActiveCd
        {
            get { return IsActive ? "true" : "false"; }
        }

        public string AttributeDsc { get; set; }

        public int AttributeId { get; set; }

        public bool ShouldSerializeAttributeId()
        {
            return AttributeId != 0;
        }

        public int DataLength { get; set; }

        public string HelpTxt { get; set; }

        [JsonIgnore]
        public TestStatus TestStatus { get; set; }
        public string StatusNm
        {
            get
            {
                switch (TestStatus)
                {
                    case TestStatus.Normal:
                        return "NORMAL";
                }
                return "NORMAL";
            }
            set
            {
                switch (value)
                {
                    case "NORMAL":
                        TestStatus = TestStatus.Normal;
                        break;
                    default:
                        TestStatus = TestStatus.Normal;
                        break;
                }
            }
        }

        [JsonIgnore]
        public TestType TestType { get; set; }
        public string DataTypeCd
        {
            get
            {
                switch (TestType)
                {
                    case TestType.Numeric:
                        return "Numeric";
                    case TestType.Text:
                        return "Text";
                }
                return "Text";
            }
            set
            {
                switch (value)
                {
                    case "Text":
                        TestType = TestType.Text;
                        break;
                    case "Numeric":
                        TestType = TestType.Numeric;
                        break;
                    default:
                        TestType = TestType.Text;
                        break;
                }
            }
        }

        private List<ClassAttribute> mClassAttributeList;
        public List<ClassAttribute> ClassAttributeList
        {
            get
            {
                if (mClassAttributeList == null)
                {
                    mClassAttributeList = new List<ClassAttribute>();
                }
                return mClassAttributeList;
            }
            set
            {
                mClassAttributeList = value;
            }
        }
    }
}

// TODO: Turn on or off "dump JSON button" until enough textbox controls are filled in