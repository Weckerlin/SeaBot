using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SeaBotCore;
using SeaBotCore.Logger;

namespace SeaBotHeadLess
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Event.LogMessageChat.OnLogMessage += message => Console.WriteLine(message.message);
            if (File.Exists("config.json"))
            {
                if (Core.Config.server_token != String.Empty)
                {
                    SeaBotCore.Core.StartBot();
                }
            }
            else
            {
                Logger.Fatal("config.json is missing..");
            }
            Thread.Sleep(-1);
        }
        
    }
}
