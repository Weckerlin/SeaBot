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
                public class MainMenu : WTGLib.IMenu
                {
                    public Message Message { get; set; }
                    int WTGLib.IMenu.ID => (int) EMenu.Main;

                    WTGLib.Button[][] WTGLib.IMenu.buttons => new WTGLib.Button[][]
                    {
                    new[]
                    {
                        new WTGLib.Button("Стратегия", new Action(() =>
                        {

                        })){ redirect=(int)EMenu.Strategy},

                    },
                    new[] {new WTGLib.Button("Данные", new Action(() =>
                    {

                    })){ redirect=(int)EMenu.Data}},
                    new[] {new WTGLib.Button("О боте", new Action(() => { })) { redirect = (int)EMenu.About } },
                    new[] {new WTGLib.Button("Настройки", new Action(() => { })) { redirect = (int)EMenu.Settings } }
                    };

               
                   
                    public void Unknown(Message msg)
                    {

                    }

                    public void OnEnter()
                    {

                    }
                };
               
                public class RandMenu : WTGLib.IMenu
                {
                    public Message Message { get; set; }
                    int WTGLib.IMenu.ID => (int)EMenu.Settings;

                    public WTGLib.Button[][] buttons => new[]
                        {new[] { new WTGLib.Button("Меню", new Action(() => { })) {redirect = (int)EMenu.Main}}};
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
         
          



        }
    }
}