﻿// SeaBotCore
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

using SeaBotCore.Data;
using SeaBotCore.Utils;
using System.Net.Http;
using SeaBotCore.Config;

namespace SeaBotCore
{
    public static class Core
    {
        private static readonly HttpClient Client = new HttpClient();
        public static string Ssid = "";
        public static GlobalData GlobalData = new GlobalData();
        public static bool Debug;
        public static int hibernation = 0;
        public static string ServerToken = "";
        public static Config.Config Config = new Config.Config();

         static Core()
        {
            Config.PropertyChanged += Config_PropertyChanged;
        }

        private static void Config_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           Configurator.Save();
        }

        public static void StopBot()
        {
            ThreadKill.KillTheThread(Networking._syncThread);
        }
    }
}