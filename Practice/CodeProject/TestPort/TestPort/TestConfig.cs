namespace TestPort
{
    public class TestConfig
    {
        public string Name { get; set; }
        public string FileName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
