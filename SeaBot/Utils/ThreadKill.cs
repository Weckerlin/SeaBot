using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBotCore.Utils
{
  public static class ThreadKill
    {
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public static void KillTheThread(Thread th)
        {
            th.Abort();
        }
    }
}
