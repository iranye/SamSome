using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionInNet
{
    class Program
    {
        static void Main(string[] args)
        {
            NormalMessage();
            AuthenticatedMessage();
        }

        private static void AuthenticatedMessage()
        {
            IMessageWriter writer = new SecureMessageWriter(new ConsoleMessageWriter(), WindowsIdentity.GetCurrent());
            writer.Write("Secure message!");
        }

        private static void NormalMessage()
        {
            IMessageWriter writer = new ConsoleMessageWriter();
            var salutation = new Salutation(writer);
            salutation.Exclaim();
        }
    }
}
