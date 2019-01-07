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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    public static class Ships
    {
        public static void AutoShip(string type, bool lootbased)
        {
            /* PLAN
             * 0. Load SHIPS xDDD
             * 1. Unload SHIPS
             * 2. Send Ships!
             * 3. God bless me, i need to do this until real world voyage :3
             */
            for (var index = 0; index < Core.GlobalData.Ships.Count; index++)
            {
                var ship = Core.GlobalData.Ships[index];
                if (ship.Type == "upgradeable" && ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 0)
                {
                    var lvl = Defenitions.UpgrDef.Items.Item.First(n => n.DefId == ship.TargetId).Levels.Level
                        .First(n => n.Id == ship.TargetLevel);
                    if ((DateTime.UtcNow - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds >
                        lvl.TravelTime + 1)
                    {
                        Logger.Logger.Info(
                            "Loading " + Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name);
                        Core.GlobalData.Upgradeables.First(n => n.DefId == ship.TargetId).Progress +=
                            (int) lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship);


                        Networking.AddTask(new Task.LoadShipUpgradeableTask(ship.InstId.ToString()));
                        Core.GlobalData.Ships[index].Loaded = 1;
                    }
                }
            }

            var _deship = new List<Ship>();

            for (var index = 0; index < Core.GlobalData.Ships.Count; index++)
            {
                var ship = Core.GlobalData.Ships[index];
                if (ship.Type == "upgradeable" && ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 1)
                {
                    var lvl = Defenitions.UpgrDef.Items.Item.First(n => n.DefId == ship.TargetId).Levels.Level
                        .First(n => n.Id == ship.TargetLevel);
                    if ((DateTime.UtcNow - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds >
                        lvl.TravelTime + 12)
                    {
                        Logger.Logger.Info(
                            "Unloading " + Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name);
                        Core.GlobalData.Upgradeables.First(n => n.DefId == ship.TargetId).Progress +=
                            (int) lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship);


                        _deship.Add(ship);
                        Networking.AddTask(new Task.UnloadShipTask(ship.InstId.ToString(),
                            Core.GlobalData.Level.ToString(), Enums.EObject.upgradeable,
                            AutoShipUtils.GetCapacity(ship).ToString(),
                            ((int) lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship)).ToString(),
                            AutoShipUtils.GetSailors(ship).ToString(), lvl.Sailors.ToString(),
                            ship.TargetLevel.ToString(),
                            Core.GlobalData.Upgradeables.First(n => n.DefId == ship.TargetId).Done.ToString(),
                            null, _deship.Count(n => n.DefId == ship.DefId).ToString()));
                        Core.GlobalData.Ships[index].Cargo = 0;
                        Core.GlobalData.Ships[index].Crew = 0;
                        Core.GlobalData.Ships[index].Loaded = 0;
                        Core.GlobalData.Ships[index].MaterialId = 0;
                        Core.GlobalData.Ships[index].Sent = 0;
                        Core.GlobalData.Ships[index].Type = String.Empty;
                        Core.GlobalData.Ships[index].TargetId = 0;
                        Core.GlobalData.Ships[index].TargetLevel = 0;
                    }
                }
            }

            _deship.Clear();
            //now send

            foreach (var ship in Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0))
            {
                var bestplace = AutoShipUtils.GetBestUpgPlace(type, AutoShipUtils.GetSailors(ship), lootbased);
                if (bestplace == null)
                {
                    continue;
                }

                var lvls = Defenitions.UpgrDef.Items.Item.Where(n => n.DefId == bestplace.DefId).First().Levels
                    .Level
                    .Where(n => n.Id == bestplace.Level).First();
                var wecan = lvls.MaterialKoef * AutoShipUtils.GetCapacity(ship);
                var remain = bestplace.Amount - bestplace.Progress;
                if (remain < wecan)
                {
                    wecan = remain;
                }

                bestplace.CargoOnTheWay += (int) wecan;
                Core.GlobalData.Ships.First(n => n.InstId == ship.InstId).Sent =
                    TimeUtils.GetEpochTime();
                Core.GlobalData.Ships.First(n => n.InstId == ship.InstId).Loaded =
                    0;
                Core.GlobalData.Ships.First(n => n.InstId == ship.InstId).Type = "upgradeable";
                Core.GlobalData.Ships.First(n => n.InstId == ship.InstId).TargetId =
                    bestplace.DefId;
                Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().TargetLevel = bestplace.Level;
                Logger.Logger.Info("Sending " + Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name);
                Networking.AddTask(new Task.SendShipUpgradeableTask(ship.InstId.ToString(),
                    bestplace.DefId.ToString(), lvls.Amount.ToString(), lvls.MaterialKoef.ToString(),
                    lvls.Sailors.ToString(), wecan.ToString(),
                    Core.GlobalData.Level.ToString()));
            }
        }
    }

    static class AutoShipUtils
    {
        public static ShipDefenitions.Item GetShipDefId(Ship ship)
        {
            return Defenitions.ShipDef.Items.Item.Where(n => n.DefId == ship.DefId).FirstOrDefault();
        }

        public static ShipDefenitions.LevelsLevel GetLevels(Ship ship, int level)
        {
            return GetShipDefId(ship).Levels.Level.Where(n => n.Id == level).FirstOrDefault();
        }

        public static int GetCapacity(Ship ship)
        {
            if (Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels == null)
            {
                return (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).CapacityLevels
                    .Level.First(n => n.Id == ship.CapacityLevel).Capacity.Value;
            }

            return (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels.Level
                .First(n => n.Id == ship.Level).Capacity;
        }

        public static int GetSailors(Ship ship)
        {
            if (Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).SailorsLevels == null)
            {
                return (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels
                    .Level.First(n => n.Id == ship.Level).Sailors;
            }

            return (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId)
                .SailorsLevels.Level
                .First(n => n.Id == ship.SailorsLevel).Sailors.Value;
        }

        public static Upgradeable GetBestUpgPlace(string itemname, int sailors, bool profitbased)
        {
            var matid =
                Defenitions.UpgrDef.Items.Item.Where(n => n.MaterialId == MaterialDB.GetItem(itemname).DefId);
            var p = matid
                .Where(shtItem =>
                    Core.GlobalData.Upgradeables.FirstOrDefault(n =>
                        n.DefId == shtItem.DefId && n.Amount != 0 && n.Progress < n.Amount) !=
                    null)
                .ToDictionary(shtItem => shtItem,
                    shtItem => Core.GlobalData.Upgradeables.First(n => n.DefId == shtItem.DefId));
            var best =
                new Dictionary<Upgradeable, decimal>();
            foreach (var up in p)
            {
                if (up.Key.Levels.Level.First(n => n.Id == up.Value.Level).Sailors > sailors)
                {
                    continue;
                }

                if (profitbased)
                {
                    var itemFirst = up.Key.Levels.Level.First(n => n.Id == up.Value.Level);
                    var time = (decimal) itemFirst
                        .TravelTime;
                    var koef = itemFirst.MaterialKoef;
                    var timepercoin = koef / time;
                    best.Add(up.Value, timepercoin);
                }
                else
                {
                    var koef = (decimal) up.Key.Levels.Level.First(n => n.Id == up.Value.Level).MaterialKoef;
                    var timepercoin = koef / sailors;
                    best.Add(up.Value, timepercoin);
                }
            }

            var bestplace = best.OrderBy(n => n.Value).LastOrDefault();
            return bestplace.Key;
        }
    }
}