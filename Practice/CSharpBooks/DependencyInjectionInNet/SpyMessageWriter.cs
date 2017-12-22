namespace DependencyInjectionInNet
{
    public class SpyMessageWriter : IMessageWriter
    {
        public string WrittenMessage { get; private set; }

        public void Write(string message)
        {
            WrittenMessage += message;
        }
    }
}
