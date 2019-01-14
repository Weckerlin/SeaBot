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

using System.Linq;
using System.Text;
using SeaBotCore;
using SeaBotCore.Data.Materials;
using Telegram.Bot.Types;

namespace SeaBotGUI.TelegramBot
{
    internal class TelegramMenu
    {
        public class NewMenu
        {
            public enum EMenu
            {
                NONE = -1,
                Main,
                Strategy,
                Settings,
                BasicShipUpg,
                LimitsWood,
                LimitsIron,
                LimitsStone,
                Intervals,
                AutoShipOptimal,
                IntervalHibernation,
                IntervalBarrel,
                Limits,
                AutoShipMaterial
            }

            public static class MenuItems
            {
                public class MainMenu : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.Main;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Start", () =>
                            {
                                if (Core.IsBotRunning)
                                {
                                    TelegramBotController.SendMessage(Message, "But the bot is already running");
                                }
                                else
                                {
                                    Core.StartBot();
                                    TelegramBotController.SendMessage(Message, "Starting...");
                                }
                            }) {redirect = -1},
                            new TelegramBot.Button("Stop", () =>
                            {
                                if (Core.IsBotRunning)
                                {
                                    Core.StopBot();
                                    TelegramBotController.SendMessage(Message, "Stopped!");
                                }
                                else
                                {
                                    TelegramBotController.SendMessage(Message, "But the bot is not running");
                                }
                            }) {redirect = -1}
                        },
                        new[] {new TelegramBot.Button("Strategy", () => { }) {redirect = (int) EMenu.Strategy}},
                        new[]
                        {
                            new TelegramBot.Button("Inventory",
                                () => { TelegramBotController.SendMessage(Message, GetInventory()); }) {redirect = -1}
                        },
                        new[] {new TelegramBot.Button("Settings", () => { }) {redirect = (int) EMenu.Settings}},
                        new[]
                        {
                            new TelegramBot.Button("About",
                                    () =>
                                    {
                                        TelegramBotController.SendMessage(Message, "SeaBot by Weespin\n2018-2019");
                                    })
                                {redirect = -1}
                        }
                    };

                    public void Unknown(Message msg)
                    {
                    }

                    public void OnEnter()
                    {
                    }

                    private string GetInventory()
                    {
                        var builder = new StringBuilder();
                        if (Core.GlobalData != null)
                        {
                            if (Core.GlobalData.Inventory != null)
                                foreach (var item in Core.GlobalData.Inventory.Where(n => n.Amount != 0))
                                    builder.AppendLine($"{MaterialDB.GetItem(item.Id).Name} - {item.Amount}");
                            else
                                builder.Append("Please start the bot before getting inventory");
                        }
                        else
                        {
                            builder.Append("Please start the bot before getting inventory");
                        }

                        return builder.ToString();
                    }
                }

                public class Settings : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.Settings;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[] {new TelegramBot.Button("Main", () => { }) {redirect = (int) EMenu.Main}},
                        new[] {new TelegramBot.Button("Limits", () => { }) {redirect = (int) EMenu.Limits}},
                        new[]
                        {
                            new TelegramBot.Button(
                                    Core.Config.upgradeonlyfactory
                                        ? "✅ Upgrade Factory only"
                                        : "❎ Upgrade Factory only",
                                    () => { Core.Config.upgradeonlyfactory = !Core.Config.upgradeonlyfactory; })
                                {redirect = (int) EMenu.Settings}
                        }
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

                public class Strategy : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.Strategy;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button(Core.Config.collectfish
                                        ? "✅ Collect Fish"
                                        : "❎ Collect Fish",
                                    () => { Core.Config.collectfish = !Core.Config.collectfish; })
                                {redirect = (int) EMenu.Strategy},
                            new TelegramBot.Button(Core.Config.autoupgrade
                                        ? "✅ Auto Upgrade"
                                        : "❎ Auto Upgrade",
                                    () => { Core.Config.autoupgrade = !Core.Config.autoupgrade; })
                                {redirect = (int) EMenu.Strategy}
                        },
                        new[]
                        {
                            new TelegramBot.Button(Core.Config.prodfactory
                                        ? "✅ Produce Factory"
                                        : "❎ Produce Factory",
                                    () => { Core.Config.prodfactory = !Core.Config.prodfactory; })
                                {redirect = (int) EMenu.Strategy},
                            new TelegramBot.Button(Core.Config.autoship
                                        ? "✅ Auto Ship"
                                        : "❎ Auto Ship",
                                    () => { Core.Config.autoship = !Core.Config.autoship; })
                                {redirect = (int) EMenu.Strategy}
                        },
                        new[]
                        {
                            new TelegramBot.Button(
                                    Core.Config.barrelhack
                                        ? "✅ Auto-Barrel"
                                        : "❎ Auto-Barrel",
                                    () => { Core.Config.barrelhack = !Core.Config.barrelhack; })
                                {redirect = (int) EMenu.Strategy},

                            new TelegramBot.Button(Core.Config.finishupgrade
                                        ? "✅ Finish Upgrade"
                                        : "❎ Finish Upgrade",
                                    () => { Core.Config.finishupgrade = !Core.Config.finishupgrade; })
                                {redirect = (int) EMenu.Strategy}
                        },
                        new[]
                        {
                            new TelegramBot.Button(
                                    Core.Config.collectfactory
                                        ? "✅ Collect Factory"
                                        : "❎ Collect Factory",
                                    () => { Core.Config.collectfactory = !Core.Config.collectfactory; })
                                {redirect = (int) EMenu.Strategy}
                        },
                        new[]
                        {
                            new TelegramBot.Button("AutoShip strategy", () => { })
                                {redirect = (int) EMenu.AutoShipOptimal},
                            new TelegramBot.Button("AutoShip material", () => { })
                                {redirect = (int) EMenu.AutoShipMaterial}
                        },


                        new[]
                        {
                            new TelegramBot.Button("Main", () => { }) {redirect = (int) EMenu.Main}
                        }
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

                public class AutoShipOptimal : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.AutoShipOptimal;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button(!Core.Config.autoshipprofit
                                        ? "✅ Save Sailors"
                                        : "❎ Save Sailors",
                                    () => { Core.Config.autoshipprofit = !Core.Config.autoshipprofit; })
                                {redirect = (int) EMenu.AutoShipOptimal},
                            new TelegramBot.Button(Core.Config.autoshipprofit
                                        ? "✅ More Loot"
                                        : "❎ More Loot",
                                    () => { Core.Config.autoshipprofit = !Core.Config.autoshipprofit; })
                                {redirect = (int) EMenu.AutoShipOptimal}
                        },


                        new[]
                        {
                            new TelegramBot.Button("Strategy", () => { }) {redirect = (int) EMenu.Strategy},
                            new TelegramBot.Button("Main", () => { }) {redirect = (int) EMenu.Main}
                        }
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

                public class AutoShipMaterial : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.AutoShipMaterial;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button(Core.Config.autoshiptype == "coins"
                                        ? "✅ Coins"
                                        : "❎ Coins",
                                    () => { Core.Config.autoshiptype = "coins"; })
                                {redirect = (int) EMenu.AutoShipMaterial},
                            new TelegramBot.Button(Core.Config.autoshiptype == "stone"
                                    ? "✅ Stone"
                                    : "❎ Stone",
                                () => { Core.Config.autoshiptype = "stone"; }) {redirect = (int) EMenu.AutoShipMaterial}
                        },
                        new[]
                        {
                            new TelegramBot.Button(Core.Config.autoshiptype == "iron"
                                    ? "✅ Iron"
                                    : "❎ Iron",
                                () => { Core.Config.autoshiptype = "iron"; }) {redirect = (int) EMenu.AutoShipMaterial},
                            new TelegramBot.Button(Core.Config.autoshiptype == "wood"
                                    ? "✅ Wood"
                                    : "❎ Wood",
                                () => { Core.Config.autoshiptype = "wood"; }) {redirect = (int) EMenu.AutoShipMaterial}
                        },
                        new[]
                        {
                            new TelegramBot.Button(Core.Config.autoshiptype == "fish"
                                    ? "✅ Fish"
                                    : "❎ Fish",
                                () => { Core.Config.autoshiptype = "fish"; }) {redirect = (int) EMenu.AutoShipMaterial}
                        },

                        new[]
                        {
                            new TelegramBot.Button("Strategy", () => { }) {redirect = (int) EMenu.Strategy},
                            new TelegramBot.Button("Main", () => { }) {redirect = (int) EMenu.Main}
                        }
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

                public class Limits : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.Limits;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Wood", () => { }) {redirect = (int) EMenu.LimitsWood},
                            new TelegramBot.Button("Iron", () => { }) {redirect = (int) EMenu.LimitsIron},
                            new TelegramBot.Button("Stone", () => { }) {redirect = (int) EMenu.LimitsStone}
                        },

                        new[]
                        {
                            new TelegramBot.Button("Menu", () => { }) {redirect = (int) EMenu.Main}
                        }
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

                public class Intervals : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.Intervals;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Barrels", () => { }) {redirect = (int) EMenu.IntervalBarrel},
                            new TelegramBot.Button("Hibernation", () => { })
                                {redirect = (int) EMenu.IntervalHibernation}
                        },

                        new[]
                        {
                            new TelegramBot.Button("Menu", () => { }) {redirect = (int) EMenu.Main}
                        }
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

                public class HibernationInterval : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.IntervalHibernation;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Menu", () => { }) {redirect = (int) EMenu.Main},
                            new TelegramBot.Button("Intervals", () => { }) {redirect = (int) EMenu.Intervals}
                        }
                    };

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                                Core.Config.hibernateinterval = ints;
                            else
                                TelegramBotController.SendMessage(Message, "Can't parse string!");
                        }
                    }

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(Message,
                            "Please enter here your hibernation interval.\nYour current hibernation interval: " +
                            Core.Config.hibernateinterval);
                    }
                }

                public class BarrelInterval : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.IntervalBarrel;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Menu", () => { }) {redirect = (int) EMenu.Main},
                            new TelegramBot.Button("Intervals", () => { }) {redirect = (int) EMenu.Intervals}
                        }
                    };

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                                Core.Config.barrelinterval = ints;
                            else
                                TelegramBotController.SendMessage(Message, "Can't parse string!");
                        }
                    }

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(Message,
                            "Please enter here your barrel interval.\nYour current barrel interval: " +
                            Core.Config.barrelinterval);
                    }
                }

                public class LimitWood : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.LimitsWood;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Menu", () => { }) {redirect = (int) EMenu.Main},
                            new TelegramBot.Button("Limits", () => { }) {redirect = (int) EMenu.Limits}
                        }
                    };

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                                Core.Config.woodlimit = ints;
                            else
                                TelegramBotController.SendMessage(Message, "Can't parse string!");
                        }
                    }

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(Message,
                            "Please enter here your Wood limit.\nYour current limit on Wood: " + Core.Config.woodlimit);
                    }
                }

                public class LimitStone : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.LimitsStone;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Menu", () => { }) {redirect = (int) EMenu.Main},
                            new TelegramBot.Button("Limits", () => { }) {redirect = (int) EMenu.Limits}
                        }
                    };

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                                Core.Config.stonelimit = ints;
                            else
                                TelegramBotController.SendMessage(Message, "Can't parse string!");
                        }
                    }

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(Message,
                            "Please enter here your Stone limit.\nYour current limit on Stone: " +
                            Core.Config.stonelimit);
                    }
                }

                public class LimitIron : TelegramBot.IMenu
                {
                    public Message Message { get; set; }
                    int TelegramBot.IMenu.ID => (int) EMenu.LimitsIron;

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons => new[]
                    {
                        new[]
                        {
                            new TelegramBot.Button("Menu", () => { }) {redirect = (int) EMenu.Main},
                            new TelegramBot.Button("Limits", () => { }) {redirect = (int) EMenu.Limits}
                        }
                    };

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                                Core.Config.stonelimit = ints;
                            else
                                TelegramBotController.SendMessage(Message, "Can't parse string!");
                        }
                    }

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(Message,
                            "Please enter here your Iron limit.\nYour current limit on Iron: " + Core.Config.ironlimit);
                    }
                }
            }
        }
    }
}