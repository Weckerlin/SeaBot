// SeaBotCore
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
namespace SeaBotCore
{
    #region

    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net.Http;
    using System.Threading;

    using Newtonsoft.Json;

    using SeaBotCore.BotMethods;
    using SeaBotCore.Config;
    using SeaBotCore.Data;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Utils;

    using Contractor = SeaBotCore.BotMethods.Contractor;

    #endregion

    public static class Core
    {
        public static Config.Config Config = new Config.Config();

        public static GlobalData GlobalData = new GlobalData();

        public static int hibernation = 0;

        public static string ServerToken = string.Empty;

        public static string Ssid = string.Empty;

        private static readonly HttpClient Client = new HttpClient();

        public static Thread BotThread = new Thread(BotVoid) { IsBackground = true };

        public static DateTime lastsleep = DateTime.Now.AddMinutes(-1);

        private static DateTime _lastbarrel = DateTime.Now;

        private static DateTime _lastdefinv = DateTime.Now.AddSeconds(-100); // ( ͡° ͜ʖ ͡°) travelin in time

        private static bool aftercrash = false;
        static Core()
        {
            Configurator.Load();
            Config.PropertyChanged += Config_PropertyChanged;
            Events.Events.SyncFailedEvent.SyncFailed.OnSyncFailedEvent += SyncFailed_OnSyncFailedEvent;
            LocalizationController.SetLanguage(Config.language);
            Events.Events.LoginedEvent.Logined.OnLoginedEvent += Logined_OnLoginedEvent;
        }

        public static bool Debug => Config.debug;

        public static bool IsBotRunning
        {
            get
            {
                if (BotThread != null)
                {
                    return BotThread.IsAlive;
                }

                return false;
            }
        }

        public static void StartBot()
        {
            if (Config.server_token == string.Empty)
            {
                Logger.Logger.Fatal(Localization.CORE_NO_SERV_TOKEN);
                return;
            }

            new System.Threading.Tasks.Task(() => { Networking.Login(); }).Start();
        }

        public static void StopBot()
        {
            new System.Threading.Tasks.Task(
                () =>
                    {
                        ThreadKill.KillTheThread(Networking._syncThread); // todo fix
                        ThreadKill.KillTheThread(BotThread);
                        Logger.Logger.Info(Localization.CORE_STOPPED);
                        Events.Events.BotStoppedEvent.BotStopped.Invoke();
                    }).Start();
        }

        private static void BotVoid()
        {
            LocalizationController.SetLanguage(Config.language);
            while (true)
            {
                Thread.Sleep(100);
                if (Config.sleepenabled)
                {
                    Sleeping.Sleep();
                }

                if ((DateTime.Now - _lastdefinv).TotalSeconds >= 10)
                {
                    if (Config.autoupgrade)
                    {
                        Buildings.AutoUpgrade(Config.upgradeonlyfactory);
                    }

                    if (Config.autoship)
                    {
                        Outposts.ConfirmOutpost();
                        Upgradable.UpgradeUpgradable();
                        Contractor.UpgradeContractor();
                        Ships.AutoShip();
                    }

                    if (Config.collectfish)
                    {
                        FishPier.CollectFish();
                    }

                    if (Config.collectmuseum)
                    {
                        Museum.CollectMuseum();
                    }

                    if (Config.collectfactory)
                    {
                        Buildings.CollectMaterials();
                    }

                    if (Config.prodfactory)
                    {
                        Factories.ProduceFactories();
                    }

                    if (Config.finishupgrade)
                    {
                        Buildings.FinishUpgrade();
                    }

                    _lastdefinv = DateTime.Now;
                }

                if (Config.barrelhack && (DateTime.Now - _lastbarrel).TotalSeconds >= Config.barrelinterval)
                {
                    Barrels.CollectBarrel();
                    _lastbarrel = DateTime.Now;
                }
            }
        }

        private static void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Configurator.Save();
        }

        private static void Logined_OnLoginedEvent()
        {
            Networking.StartThread();
            if (!IsBotRunning)
            {
                BotThread = new Thread(BotVoid) { IsBackground = true };
                BotThread.Start();
            }

            if (aftercrash)
            {
                if (Core.GlobalData != null)
                {
                    if (!Directory.Exists("crashlog"))
                    {
                        Directory.CreateDirectory("crashlog");
                    }
                    aftercrash = false;
                    File.WriteAllText(
                        "afcrashdmp" + DateTime.Now.ToString(@"yyyy-MM-dd HH-mm-ss"),
                        JsonConvert.SerializeObject(Core.GlobalData));
                }
            }
            Events.Events.BotStartedEvent.BotStarted.Invoke();
        }

        private static void SyncFailed_OnSyncFailedEvent(Enums.EErrorCode e)
        {
            new System.Threading.Tasks.Task(
                () =>
                    {
                        if ((int)e == 4010 || e == 0 || e == Enums.EErrorCode.INVALID_SESSION
                            || e == Enums.EErrorCode.COLLECTION_IN_FUTURE || e == Enums.EErrorCode.COLLECTION_IN_PAST
                            || (int)e == 1011 || (int)e == 2006)
                        {
                            Logger.Logger.Info(Localization.CORE_RESTARTING);
                            if (Core.GlobalData != null)
                            {
                                if (!Directory.Exists("crashlog"))
                                {
                                    Directory.CreateDirectory("crashlog");
                                }
                                aftercrash = true;
                                File.WriteAllText(
                                    "bfcrashdmp" + DateTime.Now.ToString(@"yyyy-MM-dd HH-mm-ss"),
                                    JsonConvert.SerializeObject(Core.GlobalData));
                            }

                            StopBot();
                            StartBot();
                        }

                        if (e == Enums.EErrorCode.PLAYER_BANNED)
                        {
                            Logger.Logger.Fatal(Localization.CORE_USER_BANNED);
                            StopBot();
                        }

                        if (e == Enums.EErrorCode.MAINTENANCE || e == Enums.EErrorCode.PLAYER_MAINTENANCE)
                        {
                            Logger.Logger.Info(Localization.CORE_MAINTENANCE_30MIN);
                            Thread.Sleep(30 * 60 * 1000);
                            StopBot();
                            StartBot();
                        }
                    }).Start();
        }
    }
}