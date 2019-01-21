// SeaBotHeadless
// Copyright (C) 2018 - 2019 Weespin
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
                    Core.StartBot();
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