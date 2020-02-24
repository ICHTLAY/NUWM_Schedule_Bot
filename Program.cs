using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUWM_Schedule_Bot
{
    class Program
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.Default;
            Console.InputEncoding = Encoding.Default;

            Bot.Start();
        }
    }
}