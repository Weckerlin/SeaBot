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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBotCore.Logger;

namespace SeaBotGUI.TelegramBot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.ReplyMarkups;

    namespace WTGLib
    {
        public class User
        {
            public int userid;
            public int MenuID;
        }

        public class WTGLib
        {
            public WTGLib(string apikey)
            {
                botClient = new TelegramBotClient(apikey);
                botClient.OnMessage += Bot_OnMessage;
                botClient.StartReceiving();
            }

            private async void Bot_OnMessage(object sender, MessageEventArgs e)
            {
                var message = e.Message.Text ?? "";
                Logger.Debug($"Received a text message in chat {e.Message.Chat.Id}. Text: {message}");

                //THIS IS A NEW USER
                if (message.StartsWith("/start"))
                {
                    var reg = new Regex(@"(\/start\s)(\d+)").Match(message);
                    if (reg.Success)
                    {
                        if (reg.Groups.Count > 1)
                        {
                            //new user
                        }
                    }
                    else
                    {
                        Parse(e.Message);
                    }
                }
            }

            static List<Type> GetMenuItems()
            {
                var type = typeof(IMenu);
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p)).ToList();
            }

            public async void Parse(Message msg)
            {
                var menus = GetMenuItems();
                var ser = Form1._teleconfig.users.Where(n => n.userid == msg.From.Id).First();
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
                        // ignored
                    }
                }

                if (first == null)
                {
                    await botClient.SendTextMessageAsync(msg.Chat, "Exeption, can't find menu item");
                }
                else
                {
                    first.Message = msg;
                    if (msg.Text != null)
                    {
                        Button a = null;
                        foreach (var rowButton in first.buttons)
                        {
                            foreach (var n in rowButton)
                            {
                                if (n.name.ToLower() == msg.Text.ToLower())
                                {
                                    a = n;
                                    break;
                                }
                            }
                        }

                        if (a == null)
                        {
                            await Task.Run(() => first.Unknown(msg));
                        }
                        else
                        {
                            if (a.redirect != 0)
                            {
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

                                await botClient.SendTextMessageAsync(msg.From.Id, "Переходим..", replyMarkup:
                                    ParseReplyKeyboardMarkup(newinst));
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

            public ReplyKeyboardMarkup ParseReplyKeyboardMarkup(IMenu arr)
            {
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
                public Button(string Name, Action Act)
                {
                    name = Name;
                    act = Act;
                }

                public string name { get; set; }
                public Action act { get; set; }
                public int redirect = 0;
                public bool adminonly = false;
            }

            public interface IMenu
            {
                Message Message { set; }
                int ID { get; }
                Button[][] buttons { get; }
                void Unknown(Message msg);
                void OnEnter();
            }

            public TelegramBotClient botClient;
        }
    }
}