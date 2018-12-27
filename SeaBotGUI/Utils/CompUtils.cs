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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBotGUI.Utils
{
    static class CompUtils
    {
        //https://www.mono-project.com/archived/howto_openbrowser/
        //for older mono comp.
        public static void OpenLink(string address)
        {
            try
            {
                var plat = (int) Environment.OSVersion.Platform;
                if ((plat != 4) && (plat != 128))
                {
                    // Use Microsoft's way of opening sites
                    Process.Start(address);
                }
                else
                {
                    // We're on Unix, try gnome-open (used by GNOME), then open
                    // (used my MacOS), then Firefox or Konqueror browsers (our last
                    // hope).
                    var cmdline = String.Format("gnome-open {0} || open {0} || " +
                                                "firefox {0} || mozilla-firefox {0} || konqueror {0}", address);
                    var proc = Process.Start(cmdline);

                    // Sleep some time to wait for the shell to return in case of error
                    Thread.Sleep(250);

                    // If the exit code is zero or the process is still running then
                    // appearently we have been successful.
                }
            }
            catch (Exception e)
            {
                // We don't want any surprises
            }
        }
    }
}