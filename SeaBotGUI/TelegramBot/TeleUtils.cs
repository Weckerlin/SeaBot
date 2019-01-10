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

using System.Net.NetworkInformation;
using System.Text;
using SeaBotCore.Logger;

namespace SeaBotGUI.TelegramBot
{
    public static class TeleUtils
    {
        private static string _macaddr;


        public static string MacAdressCode
        {
            get
            {
                if (_macaddr == null) _macaddr = GetDefMac();

                return _macaddr;
            }
        }

        public static string GetDefMac()
        {
            var computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            if (nics == null || nics.Length < 1)
            {
                Logger.Info("No network interfaces found.");
                return "DEFCODE";
            }

            foreach (var adapter in nics)
            {
                var address = adapter.GetPhysicalAddress();
                var bytes = address.GetAddressBytes();
                var addr = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                    // Display the physical address in hexadecimal.
                    addr.Append(bytes[i].ToString("X2"));
                // Insert a hyphen after each byte, unless we are at the end of the

                if (addr.ToString() != "") return addr.ToString();
            }

            return "DEFCODE";
        }
    }
}