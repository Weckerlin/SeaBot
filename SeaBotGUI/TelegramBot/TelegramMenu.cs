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
                BasicShipUpg,
                CPLWood,
                CPLIron,
                CPLStone,
                Intervals,
                ShipOptimal,
                Data,

                About
            }

            public static class MenuItems
            {
                public class MainMenu : WTGLib.IMenu
                {
                    public Message Message { get; set; }
                    int WTGLib.IMenu.ID => (int) EMenu.Main;

                    WTGLib.Button[][] WTGLib.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new WTGLib.Button("Start", () => { }) {redirect = -1},
                            new WTGLib.Button("Stop", () => { }) {redirect = -1}
                        },
                        new[] {new WTGLib.Button("Strategy", () => { }) {redirect = (int) EMenu.Strategy}},
                        new[] {new WTGLib.Button("Settings", () => { }) {redirect = (int) EMenu.Settings}},
                        new[] {new WTGLib.Button("About", () => { }) {redirect = (int) EMenu.About}}
                    };


                    public void Unknown(Message msg)
                    {
                    }

                    public void OnEnter()
                    {
                    }
                }

                public class Settings : WTGLib.IMenu
                {
                    public Message Message { get; set; }
                    int WTGLib.IMenu.ID => (int) EMenu.Settings;

                    WTGLib.Button[][] WTGLib.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new WTGLib.Button("Start", () => { }) {redirect = -1},
                            new WTGLib.Button("Stop", () => { }) {redirect = -1}
                        },
                        new[] {new WTGLib.Button("Strategy", () => { }) {redirect = (int) EMenu.Strategy}},
                        new[] {new WTGLib.Button("Settings", () => { }) {redirect = (int) EMenu.Settings}},
                        new[] {new WTGLib.Button("About", () => { }) {redirect = (int) EMenu.About}}
                    };

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