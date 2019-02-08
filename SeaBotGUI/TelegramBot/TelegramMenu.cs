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

    using System.Linq;
    using System.Text;

    using SeaBotCore;
    using SeaBotCore.Data.Materials;

    using SeaBotGUI.Localization;

    using Telegram.Bot.Types;

    #endregion

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
                public class AutoShipMaterial : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.autoshiptype == "coins"
                                                ? "✅" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_COINS
                                                : "❎" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_COINS,
                                            () => { Core.Config.autoshiptype = "coins"; })
                                            {
                                                redirect = (int)EMenu.AutoShipMaterial
                                            },
                                        new TelegramBot.Button(
                                            Core.Config.autoshiptype == "stone"
                                                ? "✅" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_STONE
                                                : "❎" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_STONE,
                                            () => { Core.Config.autoshiptype = "stone"; })
                                            {
                                                redirect = (int)EMenu.AutoShipMaterial
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.autoshiptype == "iron"
                                                ? "✅" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_IRON
                                                : "❎" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_IRON,
                                            () => { Core.Config.autoshiptype = "iron"; })
                                            {
                                                redirect = (int)EMenu.AutoShipMaterial
                                            },
                                        new TelegramBot.Button(
                                            Core.Config.autoshiptype == "wood"
                                                ? "✅" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_WOOD
                                                : "❎" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_WOOD,
                                            () => { Core.Config.autoshiptype = "wood"; })
                                            {
                                                redirect = (int)EMenu.AutoShipMaterial
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.autoshiptype == "fish"
                                                ? "✅" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_FISH
                                                : "❎" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_MATERIAL_FISH,
                                            () => { Core.Config.autoshiptype = "fish"; })
                                            {
                                                redirect = (int)EMenu.AutoShipMaterial
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_STRATEGY, () => { })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MAIN, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.AutoShipMaterial;

                    public void OnEnter()
                    {
                        // blablaofc
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            // bla//bla :D
                        }
                    }
                }

                public class AutoShipOptimal : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            !Core.Config.autoshipprofit
                                                ? "✅" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_OPTIMAL_SAVE_SAILORS
                                                : "❎" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_OPTIMAL_SAVE_SAILORS,
                                            () => { Core.Config.autoshipprofit = !Core.Config.autoshipprofit; })
                                            {
                                                redirect = (int)EMenu.AutoShipOptimal
                                            },
                                        new TelegramBot.Button(
                                            Core.Config.autoshipprofit
                                                ? "✅" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_OPTIMAL_MORE_LOOT
                                                : "❎" + PrivateLocal.TELEGRAM_SHIP_STRATEGY_OPTIMAL_MORE_LOOT,
                                            () => { Core.Config.autoshipprofit = !Core.Config.autoshipprofit; })
                                            {
                                                redirect = (int)EMenu.AutoShipOptimal
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_STRATEGY, () => { })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MAIN, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.AutoShipOptimal;

                    public void OnEnter()
                    {
                        // blablaofc
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            // bla//bla :D
                        }
                    }
                }

                public class BarrelInterval : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MENU, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_INTERVALS, () => { })
                                            {
                                                redirect = (int)EMenu.Intervals
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.IntervalBarrel;

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(
                            this.Message,
                            PrivateLocal.TELEGRAM_INTERVAL_BARREL + Core.Config.barrelinterval);
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                            {
                                Core.Config.barrelinterval = ints;
                            }
                            else
                            {
                                TelegramBotController.SendMessage(this.Message, PrivateLocal.TELEGRAM_ERR_CANT_PARSE);
                            }
                        }
                    }
                }

                public class HibernationInterval : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MENU, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_INTERVALS, () => { })
                                            {
                                                redirect = (int)EMenu.Intervals
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.IntervalHibernation;

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(
                            this.Message,
                            PrivateLocal.TELEGRAM_INTERVAL_HIBERNATION + Core.Config.hibernateinterval);
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                            {
                                Core.Config.hibernateinterval = ints;
                            }
                            else
                            {
                                TelegramBotController.SendMessage(this.Message, PrivateLocal.TELEGRAM_ERR_CANT_PARSE);
                            }
                        }
                    }
                }

                public class Intervals : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_BARRELS, () => { })
                                            {
                                                redirect = (int)EMenu.IntervalBarrel
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_HIBERNATION, () => { })
                                            {
                                                redirect = (int)EMenu.IntervalHibernation
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MENU, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.Intervals;

                    public void OnEnter()
                    {
                        // blablaofc
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            // bla//bla :D
                        }
                    }
                }

                public class LimitIron : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MENU, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_LIMITS, () => { })
                                            {
                                                redirect = (int)EMenu.Limits
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.LimitsIron;

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(
                            this.Message,
                            PrivateLocal.TELEGRAM_INTERVAL_IRON + Core.Config.ironlimit);
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                            {
                                Core.Config.stonelimit = ints;
                            }
                            else
                            {
                                TelegramBotController.SendMessage(this.Message, PrivateLocal.TELEGRAM_ERR_CANT_PARSE);
                            }
                        }
                    }
                }

                public class Limits : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_WOOD, () => { })
                                            {
                                                redirect = (int)EMenu.LimitsWood
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_IRON, () => { })
                                            {
                                                redirect = (int)EMenu.LimitsIron
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_STONE, () => { })
                                            {
                                                redirect = (int)EMenu.LimitsStone
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MENU, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.Limits;

                    public void OnEnter()
                    {
                        // blablaofc
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            // bla//bla :D
                        }
                    }
                }

                public class LimitStone : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MENU, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_LIMITS, () => { })
                                            {
                                                redirect = (int)EMenu.Limits
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.LimitsStone;

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(
                            this.Message,
                            PrivateLocal.TELEGRAM_INTERVAL_STONE + Core.Config.stonelimit);
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                            {
                                Core.Config.stonelimit = ints;
                            }
                            else
                            {
                                TelegramBotController.SendMessage(this.Message, PrivateLocal.TELEGRAM_ERR_CANT_PARSE);
                            }
                        }
                    }
                }

                public class LimitWood : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MENU, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            },
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_LIMITS, () => { })
                                            {
                                                redirect = (int)EMenu.Limits
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.LimitsWood;

                    public void OnEnter()
                    {
                        TelegramBotController.SendMessage(
                            this.Message,
                            PrivateLocal.TELEGRAM_INTERVAL_WOOD + Core.Config.woodlimit);
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            var ints = 0;
                            var parsed = int.TryParse(msg.Text, out ints);
                            if (parsed)
                            {
                                Core.Config.woodlimit = ints;
                            }
                            else
                            {
                                TelegramBotController.SendMessage(this.Message, PrivateLocal.TELEGRAM_ERR_CANT_PARSE);
                            }
                        }
                    }
                }

                public class MainMenu : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            PrivateLocal.TELEGRAM_BTN_START,
                                            () =>
                                                {
                                                    if (Core.IsBotRunning)
                                                    {
                                                        TelegramBotController.SendMessage(
                                                            this.Message,
                                                            PrivateLocal.TELEGRAM_ERR_BOT_RUNNING);
                                                    }
                                                    else
                                                    {
                                                        Core.StartBot();
                                                        TelegramBotController.SendMessage(
                                                            this.Message,
                                                            PrivateLocal.TELEGRAM_MSG_STARTING);
                                                    }
                                                }) {
                                                      redirect = -1 
                                                   },
                                        new TelegramBot.Button(
                                            PrivateLocal.TELEGRAM_BTN_STOP,
                                            () =>
                                                {
                                                    if (Core.IsBotRunning)
                                                    {
                                                        Core.StopBot();
                                                        TelegramBotController.SendMessage(
                                                            this.Message,
                                                            PrivateLocal.TELEGRAM_MSG_STOPPED);
                                                    }
                                                    else
                                                    {
                                                        TelegramBotController.SendMessage(
                                                            this.Message,
                                                            PrivateLocal.TELEGRAM_ERR_BOT_NOT_RUNNING);
                                                    }
                                                }) {
                                                      redirect = -1 
                                                   }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_STRATEGY, () => { })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            PrivateLocal.TELEGRAM_BTN_INVENTORY,
                                            () =>
                                                {
                                                    TelegramBotController.SendMessage(
                                                        this.Message,
                                                        this.GetInventory());
                                                }) {
                                                      redirect = -1 
                                                   }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_SETTINGS, () => { })
                                            {
                                                redirect = (int)EMenu.Settings
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            PrivateLocal.TELEGRAM_BTN_ABOUT,
                                            () =>
                                                {
                                                    TelegramBotController.SendMessage(
                                                        this.Message,
                                                        "SeaBot by Weespin\n2018-2019");
                                                }) {
                                                      redirect = -1 
                                                   }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.Main;

                    public void OnEnter()
                    {
                    }

                    public void Unknown(Message msg)
                    {
                    }

                    private string GetInventory()
                    {
                        var builder = new StringBuilder();
                        if (Core.GlobalData != null)
                        {
                            if (Core.GlobalData.Inventory != null)
                            {
                                foreach (var item in Core.GlobalData.Inventory.Where(n => n.Amount != 0))
                                {
                                    builder.AppendLine($"{MaterialDB.GetItem(item.Id).Name} - {item.Amount}");
                                }
                            }
                            else
                            {
                                builder.Append(PrivateLocal.TELEGRAM_EXCEPTION_NULL_INVENTORY);
                            }
                        }
                        else
                        {
                            builder.Append(PrivateLocal.TELEGRAM_EXCEPTION_NULL_INVENTORY);
                        }

                        return builder.ToString();
                    }
                }

                public class Settings : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MAIN, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_LIMITS, () => { })
                                            {
                                                redirect = (int)EMenu.Limits
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.upgradeonlyfactory
                                                ? "✅" + PrivateLocal.TELEGRAM_SETTINGS_UPGRADE_FACTORY_ONLY
                                                : "❎" + PrivateLocal.TELEGRAM_SETTINGS_UPGRADE_FACTORY_ONLY,
                                            () => { Core.Config.upgradeonlyfactory = !Core.Config.upgradeonlyfactory; })
                                            {
                                                redirect = (int)EMenu.Settings
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.Settings;

                    public void OnEnter()
                    {
                        // blablaofc
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            // bla//bla :D
                        }
                    }
                }

                public class Strategy : TelegramBot.IMenu
                {
                    public Message Message { get; set; }

                    TelegramBot.Button[][] TelegramBot.IMenu.buttons =>
                        new[]
                            {
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.collectfish
                                                ? "✅" + PrivateLocal.TELEGRAM_STRATEGY_COLLECT_FISH
                                                : "❎" + PrivateLocal.TELEGRAM_STRATEGY_COLLECT_FISH,
                                            () => { Core.Config.collectfish = !Core.Config.collectfish; })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            },
                                        new TelegramBot.Button(
                                            Core.Config.autoupgrade
                                                ? "✅" + PrivateLocal.TELEGRAM_STRATEGY_AUTO_UPGRADE
                                                : "❎" + PrivateLocal.TELEGRAM_STRATEGY_AUTO_UPGRADE,
                                            () => { Core.Config.autoupgrade = !Core.Config.autoupgrade; })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.prodfactory
                                                ? "✅" + PrivateLocal.TELEGRAM_STRATEGY_PRODUCE_FACTORY
                                                : "❎" + PrivateLocal.TELEGRAM_STRATEGY_PRODUCE_FACTORY,
                                            () => { Core.Config.prodfactory = !Core.Config.prodfactory; })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            },
                                        new TelegramBot.Button(
                                            Core.Config.autoship
                                                ? "✅" + PrivateLocal.TELEGRAM_STRATEGY_AUTO_SHIP
                                                : "❎" + PrivateLocal.TELEGRAM_STRATEGY_AUTO_SHIP,
                                            () => { Core.Config.autoship = !Core.Config.autoship; })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.barrelhack
                                                ? "✅" + PrivateLocal.TELEGRAM_STRATEGY_AUTO_BARREL
                                                : "❎" + PrivateLocal.TELEGRAM_STRATEGY_AUTO_BARREL,
                                            () => { Core.Config.barrelhack = !Core.Config.barrelhack; })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            },
                                        new TelegramBot.Button(
                                            Core.Config.finishupgrade
                                                ? "✅" + PrivateLocal.TELEGRAM_STRATEGY_FINISH_UPGRADE
                                                : "❎" + PrivateLocal.TELEGRAM_STRATEGY_FINISH_UPGRADE,
                                            () => { Core.Config.finishupgrade = !Core.Config.finishupgrade; })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(
                                            Core.Config.collectfactory
                                                ? "✅" + PrivateLocal.TELEGRAM_STRATEGY_COLLECT_FACTORY
                                                : "❎" + PrivateLocal.TELEGRAM_STRATEGY_COLLECT_FACTORY,
                                            () => { Core.Config.collectfactory = !Core.Config.collectfactory; })
                                            {
                                                redirect = (int)EMenu.Strategy
                                            }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_AUTOSHIP_OPTIMAL, () => { })
                                            {
                                                redirect = (int)EMenu.AutoShipOptimal
                                            },
                                        new TelegramBot.Button(
                                            PrivateLocal.TELEGRAM_BTN_STOP_AUTOSHIP_MATERIAL,
                                            () => { }) {
                                                          redirect = (int)EMenu.AutoShipMaterial 
                                                       }
                                    },
                                new[]
                                    {
                                        new TelegramBot.Button(PrivateLocal.TELEGRAM_BTN_MAIN, () => { })
                                            {
                                                redirect = (int)EMenu.Main
                                            }
                                    }
                            };

                    int TelegramBot.IMenu.ID => (int)EMenu.Strategy;

                    public void OnEnter()
                    {
                        // blablaofc
                    }

                    public void Unknown(Message msg)
                    {
                        if (msg.Text != null)
                        {
                            // bla//bla :D
                        }
                    }
                }
            }
        }
    }
}