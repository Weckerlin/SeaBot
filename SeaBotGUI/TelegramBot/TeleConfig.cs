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
namespace SeaBotGUI.TelegramBot
{
    #region

    using System.Collections.Generic;
    using System.IO;
    using System.Web.Script.Serialization;

    using Exceptionless.Json;

    #endregion

    public class TeleConfigData
    {
        public List<User> users = new List<User>();
    }

    internal class TeleConfigSer
    {
        public static void Load()
        {
            if (File.Exists("telegramconfig.json"))
            {
                Form1._teleconfig =
                    JsonConvert.DeserializeObject<TeleConfigData>(File.ReadAllText("telegramconfig.json"));
            }
        }

        public static void Save()
        {
            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(Form1._teleconfig);
            File.WriteAllText("telegramconfig.json", json);
        }
    }
}