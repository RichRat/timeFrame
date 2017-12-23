using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timeframeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Serv(9042).listen();
        }
    }
}
