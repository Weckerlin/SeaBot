// BarrelTest
// Copyright (C) 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SeaBotCore;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Events;
using SeaBotCore.Logger;
using SeaBotCore.Utils;
using Task = SeaBotCore.Task;

namespace ConsoleApp1
{
    class Program
    {
        public static bool kicked;

        static void Main(string[] args)
        {
            Console.WriteLine("BarrelTest");
            var token = File.ReadAllText("token.txt");
            Core.ServerToken = token;

            Events.SyncFailedEvent.SyncFailed.OnSyncFailedEvent += Kicked_OnWrongSession;
            for (var i = 7; i < 200; i++)
            {
                kicked = false;
                var attempts = 0;
                Console.WriteLine("Testing inverval = " + i);
                Networking.Login();
                while (true)
                {
                    Thread.Sleep(i * 1000);
                    if (!kicked)
                    {
                        var bar = BarrelController.GetNextBarrel(Defenitions.BarrelDef.Items.Item
                            .Where(n => n.DefId == 21).First());
                        if (bar.Definition.Id != 0)
                        {
                            Console.WriteLine(
                                $"Barrel! Collecting {bar.Amount} {MaterialDB.GetItem(bar.Definition.Id).Name}");
                        }

                        Networking.AddTask(new Task.ConfirmBarrelTask("21", bar.get_type(), bar.Amount.ToString(),
                            bar.Definition.Id.ToString(), Core.GlobalData.Level.ToString()));
                        attempts++;
                    }
                    else
                    {
                        File.WriteAllText(i.ToString(), attempts.ToString());
                        break;
                    }
                }
            }
        }

        private static void Kicked_OnWrongSession(Enums.EErrorCode e)
        {
            kicked = true;
            Console.WriteLine("Kicked us with error " + e);
        }

        private static void Kicked_OnWrongSession()
        {
            kicked = true;
        }
    }
}