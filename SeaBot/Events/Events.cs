using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBotCore.Events
{
 public   static class Events
    {
        public static class SyncFailedEvent
        {
            public delegate void SyncFailedHandler(Enums.EErrorCode e);

            public static class SyncFailedChat
            {
                public static event SyncFailedHandler OnSyncFailedEvent;

                public static void Invoke(Enums.EErrorCode e)
                {
                    OnSyncFailed(e);
                }

                private static void OnSyncFailed(Enums.EErrorCode e)
                {
                    OnSyncFailedEvent?.Invoke(e);
                }
            }
        }

    }
}
