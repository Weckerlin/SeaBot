using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SeaBotCore;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Logger;
using SeaBotCore.Utils;
using Task = SeaBotCore.Task;

namespace ConsoleApp1
{
    class Program
    {
      public  static bool kicked = false;
        static void Main(string[] args)
        {
            Console.WriteLine("BarrelTest");
            var token = File.ReadAllText("token.txt");
            SeaBotCore.Core.ServerToken = token;
           
            SeaBotCore.Events.Events.SyncFailedEvent.SyncFailedChat.OnSyncFailedEvent += Kicked_OnWrongSession;
            for (int i = 7; i < 200; i++)
            {
                kicked = false;
                var attempts = 0;
                Console.WriteLine("Testing inverval = "+  i);
                Networking.Login();
                while (true)
                {
                    Thread.Sleep(i*1000);
                    if (!kicked)
                    {
                        var bar = BarrelController.GetNextBarrel(Defenitions.BarrelDef.Items.Item
                            .Where(n => n.DefId == 21).First());
                     Console.WriteLine(
                            $"Barrel! Collecting {bar.Amount} {((Enums.EMaterial) bar.Definition.Id).ToString()}");

                        Networking.AddTask(new Task.ConfirmBarrelTask("21", bar.get_type(), bar.Amount.ToString(),
                            bar.Definition.Id.ToString(), Core.GolobalData.Level.ToString()));
                        attempts++;
                    }
                    else
                    {
                        File.WriteAllText(i.ToString(),attempts.ToString());
                        break;
                        
                    }
                }
            }
        }

        private static void Kicked_OnWrongSession(Enums.EErrorCode e)
        {
            Program.kicked = true;
            Console.WriteLine("Kicked us with error "+e.ToString());
        }

        private static void Kicked_OnWrongSession()
        {
            Program.kicked = true;
        }
    }
}
