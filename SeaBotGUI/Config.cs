// SeabotGUI
// Copyright (C) 2018 Weespin
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

using System.IO;
using System.Web.Script.Serialization;

namespace SeaBotGUI
{
    public class Config
    {
        public string server_token = ""; //done
        public bool debug = false; //done
        public int woodlimit = 0;
        public int ironlimit = 0;
        public int stonelimit = 0;
        public bool collectfish = false; //done
        public bool prodfactory = false; //done
        public bool collectfactory = false; //done
    }

    class ConfigSer
    {
        public static void Save()
        {
            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(Form1._config);
            File.WriteAllText("config.json", json);
        }

        public static void Load()
        {
            if (File.Exists("config.json"))
            {
                var ser = new JavaScriptSerializer();
                Form1._config = ser.Deserialize<Config>(File.ReadAllText("config.json"));
            }
        }
    }
}