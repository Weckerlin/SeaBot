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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = Telegram.Bot.Types.File;
using Message = Telegram.Bot.Types.Message;

namespace SeaBotGUI.TelegramBot
{
    class TelegramMenu
    {
        public class NewMenu
        {
            public enum EMenu
            {
                NONE = -1,
                Main,
                Token,
                Control,
                Strategy,
                Settings,
                Data,
                About
            }
            public static class MenuItems
            {
                public class MainMenu : IMenu
                {
                    public Message Message { get; set; }
                    public EMenu ID => EMenu.Main;

                    public Button[][] buttons => new Button[][]
                    {
                    new[]
                    {
                        new Button("Стратегия", new Action(() =>
                        {

                        })){ redirect=EMenu.Strategy},

                    },
                    new[] {new Button("Данные", new Action(() =>
                    {

                    })){ redirect=EMenu.Data}},
                    new[] {new Button("О боте", new Action(() => { })) { redirect = EMenu.About } },
                    new[] {new Button("Настройки", new Action(() => { })) { redirect = EMenu.Settings } }
                    };
                    public void Unknown(Message msg)
                    {

                    }

                    public void OnEnter()
                    {

                    }
                };
               
                public class RandMenu : IMenu
                {
                    public Message Message { get; set; }
                    public EMenu ID => EMenu.Settings;

                    public Button[][] buttons => new[]
                        {new[] {new Button("Меню", new Action(() => { })) {redirect = EMenu.Main}}};
                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                          //bla//bla :D
                        }
                    }

                    public void OnEnter()
                    {
                        //blablaofc
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
            public static async void Parse(Telegram.Bot.Types.Message msg)
            {
                var menus = GetMenuItems();
                var menu = (EMenu) Form1._teleconfig.users.Where(n => n.userid == msg.From.Id).First().MenuID;
                IMenu first = null;
                foreach (var mn in menus)
                {
                    try
                    {
                        var instance = Activator.CreateInstance((Type)mn) as IMenu;
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
                    await Form1.bot.botClient.SendTextMessageAsync(msg.Chat, "Exeption, can't find menu item");
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

                            if (a.redirect != NewMenu.EMenu.NONE)
                            {
                             
                               Form1._teleconfig.users.Where(n=>n.userid==msg.From.Id).First().MenuID =(int) a.redirect;
                               TeleConfigSer.Save();
                               IMenu newinst = null;
                                foreach (var mn in menus)
                                {
                                    try
                                    {
                                        var instance = Activator.CreateInstance((Type)mn) as IMenu;
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

                                await Form1.bot.botClient.SendTextMessageAsync(msg.From.Id, "Переходим..", replyMarkup:
                                    NewMenu.ParseReplyKeyboardMarkup(newinst));
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
            public static ReplyKeyboardMarkup ParseReplyKeyboardMarkup(IMenu arr, bool admin = false)
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
                public EMenu redirect = EMenu.NONE;
                
            }

            public interface IMenu
            {
                Message Message { set; }
                EMenu ID { get; }
                Button[][] buttons { get; }
                void Unknown(Telegram.Bot.Types.Message msg);
                void OnEnter();
            }



        }
    }
}