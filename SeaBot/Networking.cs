// SeaBotCore
// Copyright (C) 2019 Weespin
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using SeaBotCore.Data;
using SeaBotCore.Utils;

namespace SeaBotCore
{
    public static class Networking
    {
        public class DelayedTask
        {
            public Task.IGameTask Task { get; set; }
            public DateTime InvokeTime { get; set; }

            DelayedTask(Task.IGameTask task, DateTime InvokeAt)
            {
                Task = task;
                InvokeTime = InvokeAt;
            }
        }

        static Networking()
        {
            Events.Events.SyncFailedEvent.SyncFailed.OnSyncFailedEvent += SyncFailedChat_OnSyncFailedEvent;
            _syncThread.IsBackground = true;

            _syncThread.Start();
        }

        private static void SyncFailedChat_OnSyncFailedEvent(Enums.EErrorCode e)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                if (e == Enums.EErrorCode.WRONG_SESSION)
                {
                    Logger.Logger.Info(
                        $"Someone is playing this game right now, waiting for {Core.hibernation} minutes");
                    _syncThread.Abort();
                    Logger.Logger.Muted = true;
                    Thread.Sleep(Core.hibernation * 1000 * 60);
                    Logger.Logger.Muted = false;
                    Logger.Logger.Info("Mwaaah, waking up after hibernation");

                    StartThread();
                    Login();
                }
            });
        }

        private static List<DelayedTask> _delayedtaskList = new List<DelayedTask>();
        public static Thread _syncThread = new Thread(SyncVoid);
        private static DateTime _lastRaised = DateTime.Now;

        public static void StartThread()
        {
            if (!_syncThread.IsAlive)
            {
                _syncThread = new Thread(SyncVoid);
                _gametasks.Clear();
                _taskId = 1;
                _syncThread.Start();
            }
        }

        private static void SyncVoid()
        {
            while (true)
            {
                Thread.Sleep(6 * 1000);
                if ( /*(DateTime.Now - _lastRaised).TotalSeconds > 6&&*/ _gametasks.Count != 0 &&
                                                                         Core.GlobalData.Level != 0)
                {
                    Logger.Logger.Debug("Syncing...");
                    Sync();
                }

                if ((DateTime.Now - _lastRaised).TotalSeconds > 300)
                {
                    Logger.Logger.Debug("Sending Heartbeat...");
                    _gametasks.Add(new Task.HeartBeat());

                    Sync();
                }

                foreach (var delayedtask in _delayedtaskList)
                {
                    if ((delayedtask.InvokeTime - DateTime.Now).Seconds > 0)
                    {
                        //invoke
                        Logger.Logger.Debug("Added delayedtask");
                        _gametasks.Add(delayedtask.Task);
                    }
                }
            }
        }

        private static int _taskId = 1;
        private static string _lastsend = "";
        private static List<Task.IGameTask> _gametasks = new List<Task.IGameTask>();
        private static readonly MD5 Md5 = new MD5CryptoServiceProvider();

        public static void AddTask(Task.IGameTask task)
        {
            _gametasks.Add(task);
            _lastRaised = DateTime.Now;
        }

        public static void AddDelayedTask(DelayedTask task)
        {
            _delayedtaskList.Add(task);
        }

        private static readonly HttpClient Client = new HttpClient();

        public static string SendRequest(Dictionary<string, string> data, string action)
        {
            try
            {
                var content = new FormUrlEncodedContent(data);

                var response = Client.PostAsync("https://portal.pixelfederation.com/sy/?a=" + action, content);

                return response.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Logger.Logger.Fatal(ex.ToString());
            }

            return "";
        }

        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length * 2);

            for (var i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        public static void Login()
        {
            Logger.Logger.Info("Logining ");
            //Get big token
            var tempuid = String.Empty;
            var baseAddress = new Uri("https://portal.pixelfederation.com/");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            using (var client = new HttpClient(handler) {BaseAddress = baseAddress})
            {
                cookieContainer.Add(baseAddress, new Cookie("_pf_login_server_token", Core.ServerToken));
                Logger.Logger.Info("[1/3] Getting another cookies");
                var result = client.GetAsync("en/seaport/").Result;
                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.80 Safari/537.36");
                result = client.GetAsync("en/seaport/").Result;
                Logger.Logger.Info("[2/3] Getting protal");
                var stringtext = result.Content.ReadAsStringAsync().Result;
                var regex = new Regex(
                    "portal.pixelfederation.com\\/(_sp\\/\\?direct_login=portal&portal_request=(.*))\" allowfullscreen>");
                var match = regex.Match(stringtext);
                if (match.Success)
                {
                    stringtext = client.GetAsync(match.Groups[1].Value).Result.Content.ReadAsStringAsync().Result;
                    Logger.Logger.Info("[3/3] Getting sessionid");
                    regex = new Regex(@"session_id': '(.*)', 'test");

                    Core.Ssid = regex.Match(stringtext).Groups[1].Value;
                    regex = new Regex(@"pid': '(.*)', 'platform");
                    tempuid = regex.Match(stringtext).Groups[1].Value;
                    Logger.Logger.Info("Successfully logged in! Session ID = " + Core.Ssid);
                }
                else
                {
                    Logger.Logger.Fatal("CANT LOGIN!");
                    return;
                }
            }

            var values = new Dictionary<string, string>
            {
                {"pid", tempuid},
                {"session_id", Core.Ssid}
            };
            var s = SendRequest(values, "client.login");
            Core.GlobalData = Parser.ParseXmlToGlobalData(s);
        }

        public static void Sync()
        {
            var taskstr = new StringBuilder("<xml>\n");
            foreach (var task in _gametasks)
            {
                taskstr.Append("<task>\n");
                taskstr.Append("<action>" + task.Action + "</action>\n");
                foreach (var customobj in task.CustomObjects)
                    taskstr.Append($"<{customobj.Key}>{customobj.Value}</{customobj.Key}>\n");
                taskstr.Append("<time>" + task.Time + "</time>\n");
                taskstr.Append("</task>\n")
                    ;
            }

            taskstr.Append($"<task_id>{_taskId}</task_id>\n");
            taskstr.Append($"<time>{TimeUtils.GetEpochTime()}</time>\n");
            taskstr.Append("</xml>");
            _lastsend = taskstr.ToString();
            var lenght = 0;
            lenght = _lastsend.Length > 224 ? 224 : _lastsend.Length;
            var sig = ToHex(
                Md5.ComputeHash(Encoding.ASCII.GetBytes(_lastsend.Substring(0, lenght) + Core.Ssid + "KNn2R4sK")),
                false); //_loc2_.substr(0,224) + _sessionData.sessionId + "KNn2R4sK"
            var values = new Dictionary<string, string>
            {
                {"pid", Core.GlobalData.UserId.ToString()},
                {"session_id", Core.Ssid},
                {"data", taskstr.ToString()},
                {"sig", sig}
            };
            _taskId++;
            Logger.Logger.Debug(new XMLMinifier(XMLMinifierSettings.Aggressive).Minify(taskstr.ToString()));
            var response = SendRequest(values, "client.synchronize");
            Logger.Logger.Debug(response);
            var doc = new XmlDocument();
            doc.LoadXml(response);
            if (doc.DocumentElement != null)
            {
                var s = doc.DocumentElement.SelectNodes("task");
                var passed = 0;
                foreach (XmlNode node in s)
                {
                    if (node.SelectSingleNode("result")?.InnerText == "OK")
                    {
                        Logger.Logger.Debug(node.SelectSingleNode("action")?.InnerText + " has been passed");
                        passed++;
                    }
                    else
                    {
                        Logger.Logger.Debug(node.SelectSingleNode("action")?.InnerText + " failed!");
                    }
                }

                if (passed != 0)
                {
                    Logger.Logger.Debug("[GOOD] Server accepted our " + passed + " requests");
                }
                else
                {
                    Logger.Logger.Warning("[BAD] Server accepted our " + passed + " requests!");
                    Logger.Logger.Info("Checking Fatal error...");
                    if (doc.SelectSingleNode("xml/task/result")?.InnerText == "ERROR")
                    {
                        var errcode =
                            (Enums.EErrorCode) Convert.ToInt32(doc.SelectSingleNode("xml/task/error_code")?.InnerText);
                        Logger.Logger.Fatal($"Server disconnected us with error {errcode.ToString()}");
                        Events.Events.SyncFailedEvent.SyncFailed.Invoke(errcode);
                    }
                    else if (doc.SelectSingleNode("xml/result")?.InnerText == "ERROR")
                    {
                        var errcode =
                            (Enums.EErrorCode) Convert.ToInt32(doc.SelectSingleNode("xml/error_code")?.InnerText);
                        Logger.Logger.Fatal($"Server disconnected us with error {errcode.ToString()}");
                        Events.Events.SyncFailedEvent.SyncFailed.Invoke(errcode);
                    }
                }

                var pushnode = doc.DocumentElement.SelectSingleNode("push");
                if (pushnode != null)
                    foreach (XmlNode node in pushnode.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "level_up":

                                Core.GlobalData.Level = Convert.ToInt32(node.ChildNodes[0].InnerText);

                                break;
                            case "sailors":
                                Core.GlobalData.Sailors = Convert.ToInt32(node.ChildNodes[0].InnerText);
                                break;
                            case "sync_interval":
                                Core.GlobalData.SyncInterval = Convert.ToByte(node.ChildNodes[0].InnerText);
                                break;
                            case "xp":
                                Core.GlobalData.Xp = Convert.ToInt32(node.ChildNodes[0].InnerText);
                                break;
                            case "material":
                            {
                                foreach (XmlNode materials in node.ChildNodes)
                                {
                                    var defid = Convert.ToInt32(materials.SelectSingleNode("def_id")?.InnerText);
                                    var amount = Convert.ToInt32(materials.SelectSingleNode("value")?.InnerText);
                                    if (Core.GlobalData.Inventory.Count(n => n.Id == defid) != 0)
                                    {
                                        for (var i = 0; i < Core.GlobalData.Inventory.Count; i++)
                                        {
                                            if (Core.GlobalData.Inventory[i].Id == defid)
                                            {
                                                Core.GlobalData.Inventory[i].Amount = amount;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Core.GlobalData.Inventory.Add(new Item {Id = defid, Amount = amount});
                                    }
                                }

                                break;
                            }
                        }
                    }
            }
            else
            {
                Logger.Logger.Fatal("Sync failed, no response");
            }

            _lastRaised = DateTime.Now;
            _gametasks.Clear();
        }
    }
}