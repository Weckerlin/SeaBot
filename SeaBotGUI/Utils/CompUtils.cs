using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
                int plat = (int)Environment.OSVersion.Platform;
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
                    string cmdline = String.Format("gnome-open {0} || open {0} || " +
                                                   "firefox {0} || mozilla-firefox {0} || konqueror {0}", address);
                    Process proc = Process.Start(cmdline);

                    // Sleep some time to wait for the shell to return in case of error
                    System.Threading.Thread.Sleep(250);

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
