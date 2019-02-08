// // SeaBotCore
// // Copyright (C) 2018 - 2019 Weespin
// // 
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU General Public License for more details.
// // 
// // You should have received a copy of the GNU General Public License
// // along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace SeaBotCore.Events
{
    public static class Events
    {
        public static class BotStartedEvent
        {
            public delegate void BotStartedHandler();

            public static class BotStarted
            {
                public static event BotStartedHandler OnBotStartedEvent;

                internal static void Invoke()
                {
                    OnBotStarted();
                }

                internal static void OnBotStarted()
                {
                    OnBotStartedEvent?.Invoke();
                }
            }
        }

        public static class BotStoppedEvent
        {
            public delegate void BotStoppedHandler();

            public static class BotStopped
            {
                public static event BotStoppedHandler OnBotStoppedEvent;

                internal static void Invoke()
                {
                    OnBotStopped();
                }

                internal static void OnBotStopped()
                {
                    OnBotStoppedEvent?.Invoke();
                }
            }
        }

        public static class LoginedEvent
        {
            public delegate void LoginedHandler();

            public static class Logined
            {
                public static event LoginedHandler OnLoginedEvent;

                internal static void Invoke()
                {
                    OnLogined();
                }

                internal static void OnLogined()
                {
                    OnLoginedEvent?.Invoke();
                }
            }
        }

        public static class SyncFailedEvent
        {
            public delegate void SyncFailedHandler(Enums.EErrorCode e);

            public static class SyncFailed
            {
                public static event SyncFailedHandler OnSyncFailedEvent;

                internal static void Invoke(Enums.EErrorCode e)
                {
                    OnSyncFailed(e);
                }

                internal static void OnSyncFailed(Enums.EErrorCode e)
                {
                    OnSyncFailedEvent?.Invoke(e);
                }
            }
        }
    }
}