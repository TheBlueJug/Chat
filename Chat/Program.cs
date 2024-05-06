using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
namespace Chat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Network.Client client = new("127.0.0.1", 8080);
        }
        

    }
}
