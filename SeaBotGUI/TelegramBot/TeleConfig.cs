// SeabotGUI
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
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace SeaBotGUI.TelegramBot
{
    public class TeleConfigData
    {
        public List<User> users = new List<User>();
    }

    class TeleConfigSer
    {
        public static void Save()
        {
            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(Form1._teleconfig);
            File.WriteAllText("telegramconfig.json", json);
        }

        public static void Load()
        {
            if (File.Exists("telegramconfig.json"))
            {
                var ser = new JavaScriptSerializer();
                Form1._teleconfig = ser.Deserialize<TeleConfigData>(File.ReadAllText("telegramconfig.json"));
            }
        }
    }
}