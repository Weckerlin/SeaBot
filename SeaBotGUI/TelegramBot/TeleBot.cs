// // SeabotGUI
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

namespace SeaBotGUI.TelegramBot
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using SeaBotCore.Logger;

    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    #endregion

    public class User
    {
        public int MenuID;

        public int userid;
    }

    public static class TelegramBotController
    {
        public static TelegramBot bot;

        public static void SendMessage(Message msg, string message)
        {
            bot.botClient.SendTextMessageAsync(msg.From.Id, message);
        }

        public static void StartBot(string apikey)
        {
            if (apikey == string.Empty)
            {
                Logger.Fatal("Telegram API key is empty!");
            }
            else
            {
                bot = new TelegramBot(apikey);
            }
        }

        public static void StopBot()
        {
            bot?.botClient.StopReceiving();
        }
    }

    public class TelegramBot
    {
        public TelegramBotClient botClient;

        public TelegramBot(string apikey)
        {
            this.botClient = new TelegramBotClient(apikey);
            this.botClient.OnMessage += this.Bot_OnMessage;
            this.botClient.StartReceiving();
        }

        public interface IMenu
        {
            Button[][] buttons { get; }

            int ID { get; }

            Message Message { set; }

            void OnEnter();

            void Unknown(Message msg);
        }

        public async void Parse(Message msg)
        {
            var ser = Form1._teleconfig.users.Where(n => n.userid == msg.From.Id).FirstOrDefault();
            if (ser == null)
            {
                if (msg.Text != null)
                {
                    var mac = TeleUtils.MacAdressCode;
                    var smac = mac.Substring(0, mac.Length / 2);
                    if (string.Compare(msg.Text, smac, true) == 0)
                    {
                        var men = GetMenuItems();

                        foreach (var mn in men)
                        {
                            try
                            {
                                var instance = Activator.CreateInstance(mn) as IMenu;
                                if (instance.ID == 0)
                                {
                                    await this.botClient.SendTextMessageAsync(
                                        msg.From.Id,
                                        "Redirecting..",
                                        replyMarkup: this.ParseReplyKeyboardMarkup(instance));
                                }
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }

                        Form1._teleconfig.users.Add(new User { MenuID = 0, userid = msg.From.Id });
                        TeleConfigSer.Save();
                    }
                    else
                    {
                        TelegramBotController.SendMessage(msg, "Wrong Code!\nPlease enter Startup Code from settings.");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            ser = Form1._teleconfig.users.Where(n => n.userid == msg.From.Id).FirstOrDefault();
            var menus = GetMenuItems();

            var menu = ser.MenuID;
            IMenu first = null;
            foreach (var mn in menus)
            {
                try
                {
                    var instance = Activator.CreateInstance(mn) as IMenu;
                    if (instance.ID == menu)
                    {
                        first = instance;
                    }
                }
                catch (Exception)
                {
                    // ingored
                }
            }

            if (first == null)
            {
                await this.botClient.SendTextMessageAsync(msg.Chat, "Exeption, can't find menu item");
            }
            else
            {
                first.Message = msg;
                if (msg.Text != null)
                {
                    Button a = null;
                    foreach (var rowButton in first.buttons)
                    foreach (var n in rowButton)
                    {
                        if (string.Compare(n.name, msg.Text, true) == 0)
                        {
                            a = n;
                            break;
                        }
                    }

                    if (a == null)
                    {
                        await Task.Run(() => first.Unknown(msg));
                    }
                    else
                    {
                        if (a.redirect != -1)
                        {
                            await Task.Run(a.act);
                            Form1._teleconfig.users.Where(n => n.userid == msg.From.Id).First().MenuID = a.redirect;
                            TeleConfigSer.Save();
                            IMenu newinst = null;
                            foreach (var mn in menus)
                            {
                                try
                                {
                                    var instance = Activator.CreateInstance(mn) as IMenu;
                                    if (instance.ID == a.redirect)
                                    {
                                        newinst = instance;
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }

                            await this.botClient.SendTextMessageAsync(
                                msg.From.Id,
                                "Redirecting..",
                                replyMarkup: this.ParseReplyKeyboardMarkup(newinst));
                            newinst.Message = msg;
                            newinst.OnEnter();
                            return;
                        }

                        await Task.Run(a.act);
                    }
                }
                else
                {
                    await Task.Run(() => first.Unknown(msg));
                }
            }
        }

        private static List<Type> GetMenuItems()
        {
            var type = typeof(IMenu);
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)).ToList();
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message.Text ?? string.Empty;
            Logger.Debug($"Received a text message in chat {e.Message.Chat.Id}. Text: {message}");

            // THIS IS A NEW USER
            if (message.StartsWith("/start"))
            {
                var reg = new Regex(@"(\/start\s)(\d+)").Match(message);
                if (reg.Success)
                {
                    TelegramBotController.SendMessage(e.Message, "Hello! Please enter Startup Code from settings.");
                }
                else
                {
                    this.Parse(e.Message);
                }
            }
            else
            {
                this.Parse(e.Message);
            }
        }

        private ReplyKeyboardMarkup ParseReplyKeyboardMarkup(IMenu arr)
        {
            if (arr == null)
            {
                Logger.Fatal("No menu!");
                return null;
            }

            var global = new List<List<KeyboardButton>>();

            foreach (var button in arr.buttons)
            {
                var a = new List<KeyboardButton>();
                foreach (var minib in button)
                {
                    a.Add(new KeyboardButton(minib.name));
                }

                global.Add(a);
            }

            return new ReplyKeyboardMarkup(global);
        }

        public class Button
        {
            public int redirect = 0;

            public Button(string Name, Action Act)
            {
                this.name = Name;
                this.act = Act;
            }

            public Action act { get; set; }

            public string name { get; set; }
        }
    }
}