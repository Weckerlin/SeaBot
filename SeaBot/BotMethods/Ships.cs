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
using System.Diagnostics;
using System.Linq;
using SeaBotCore.Cache;
using SeaBotCore.Data;
using SeaBotCore.Data.Definitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Localizaion;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    public static class Ships
    {
        public static void AutoShip(string type, bool lootbased)
        {

            //unload
            for (var index = 0; index < Core.GlobalData.Ships.Count; index++)
            {
                var ship = Core.GlobalData.Ships[index];
                if (ship.Type == "upgradeable" && ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 0)
                {
                    var lvl = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                        ?.Levels.Level
                        .First(n => n.Id == ship.TargetLevel);
                    if (lvl != null && AutoShipUtils.isVoyageCompleted(ship))
                    {
                        Logger.Logger.Info(
                            Localization.SHIPS_LOADING +
                            LocalizationCache.GetNameFromLoc(
                                Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                                Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
                        Core.GlobalData.Upgradeables.First(n => n.DefId == ship.TargetId).Progress +=
                            lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship);


                        Networking.AddTask(new Task.LoadShipUpgradeableTask(ship.InstId));
                        Core.GlobalData.Ships[index].Loaded = 1;
                    }
                }
            }

            var _deship = new List<Ship>();
            for (var index = 0; index < Core.GlobalData.Ships.Count; index++)
            {

                var ship = Core.GlobalData.Ships[index];
                if (ship.TargetId != 0 && ship.Activated != 0)
                {
                    if (ship.Loaded == 1 && ship.Type == "upgradeable")
                    {
                        var lvl = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                            ?.Levels.Level
                            .First(n => n.Id == ship.TargetLevel);
                        if (lvl != null && AutoShipUtils.isVoyageCompleted(ship))
                        {
                            Logger.Logger.Info(
                                Localization.SHIPS_UNLOADING + LocalizationCache.GetNameFromLoc(
                                    Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                                    Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
                            var upg = Core.GlobalData.Upgradeables.FirstOrDefault(n => n.DefId == ship.TargetId);
                            if (upg != null)
                            {
                              upg.Progress +=
                                    lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship);
                            }
                        
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
                                Core.GlobalData.Level, Enums.EObject.upgradeable,
                                AutoShipUtils.GetCapacity(ship),
                                lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship),
                                AutoShipUtils.GetSailors(ship), lvl.Sailors,
                                ship.TargetLevel,
                                null, _deship.Count(n => n.DefId == ship.DefId)));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    if (ship.Type == "marketplace")
                    {
                        var market = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId);
                        var lvl = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId).Materials
                            .Material.Where(n => n.Id == ship.MaterialId).FirstOrDefault();

                        if (lvl != null && AutoShipUtils.isVoyageCompleted(ship))
                        {
                            Logger.Logger.Info(
                            Localization.SHIPS_UNLOADING + LocalizationCache.GetNameFromLoc(
                                Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId)?.NameLoc,
                                Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId)?.Name));
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
                                Core.GlobalData.Level, Enums.EObject.marketplace,
                                AutoShipUtils.GetCapacity(ship),
                                lvl.InputKoef * AutoShipUtils.GetCapacity(ship),
                                AutoShipUtils.GetSailors(ship), market.Sailors,
                                ship.TargetLevel,
                                null, _deship.Count(n => n.DefId == ship.DefId)));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    if (ship.Type == "wreck")
                    {
                        var wrk = Core.GlobalData.Wrecks.Where(n => n.InstId == ship.TargetId).FirstOrDefault();
                        var predefined = Definitions.WreckDef.Items.Item.Where(n => n.DefId == wrk.DefId).FirstOrDefault();

                        if (wrk != null && AutoShipUtils.isVoyageCompleted(ship))
                        {
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
                                Core.GlobalData.Level, Enums.EObject.wreck,
                                AutoShipUtils.GetCapacity(ship),
                                0,
                                AutoShipUtils.GetSailors(ship), wrk.Sailors,
                                ship.TargetLevel,
                                null, _deship.Count(n => n.DefId == ship.DefId)));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    //Contractor
                    if (ship.Type == "contractor")
                    {
                        if (AutoShipUtils.isVoyageCompleted(ship))
                        {
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipContactorTask(ship.InstId));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    if (ship.Type == "global_contractor")
                    {
                        var predefined = Definitions.GConDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
                        if (AutoShipUtils.isVoyageCompleted(ship))
                        {
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipGlobalContractorTask(ship.InstId));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    if (ship.Type == "outpost")
                    {
                        if (AutoShipUtils.isVoyageCompleted(ship))
                        {
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipOutpostTask(ship.InstId));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    if (ship.Type == "social_contract")
                    {
                        if (AutoShipUtils.isVoyageCompleted(ship))
                        {
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipSocialContractTask(ship.InstId));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    if (ship.Type == "dealer")
                    {
                        var predefined = Definitions.DealerDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
                        if (AutoShipUtils.isVoyageCompleted(ship))
                        {

                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
                                Core.GlobalData.Level, Enums.EObject.dealer,
                                AutoShipUtils.GetCapacity(ship),
                                0,
                                AutoShipUtils.GetSailors(ship), (int)predefined.Sailors,
                                ship.TargetLevel,
                                null, _deship.Count(n => n.DefId == ship.DefId)));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    if (ship.Type == "treasure")
                    {
                        var predefined = Definitions.TreasureDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
                        if (AutoShipUtils.isVoyageCompleted(ship))
                        {
                            _deship.Add(ship);
                            Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
                                Core.GlobalData.Level, Enums.EObject.treasure,
                                AutoShipUtils.GetCapacity(ship),
                                0,
                                AutoShipUtils.GetSailors(ship), 0,
                                ship.TargetLevel,
                                null, _deship.Count(n => n.DefId == ship.DefId)));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }
                }
            }

            _deship.Clear();
            //now send
            var bestships = Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0 && n.Sent == 0)
                .OrderByDescending(AutoShipUtils.GetCapacity).ToList();
            foreach (var ship in bestships)
            {
                var bestplace = AutoShipUtils.GetBestUpgPlace(type, AutoShipUtils.GetSailors(ship), lootbased);

                if (bestplace == null || Core.GlobalData.Sailors < AutoShipUtils.GetSailors(ship))
                {
                    continue;
                }


                var lvls = Definitions.UpgrDef.Items.Item.First(n => n.DefId == bestplace.DefId).Levels
                    .Level.FirstOrDefault(n => n.Id == bestplace.Level);
                if (lvls != null)
                {
                    var wecan = lvls.MaterialKoef * AutoShipUtils.GetCapacity(ship);
                    var remain = bestplace.Amount - bestplace.Progress;
                    if (remain < wecan)
                    {
                        wecan = remain;
                    }

                    Core.GlobalData.Sailors -= lvls.Sailors;

                    bestplace.CargoOnTheWay += wecan;
                    var shp = Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First();
                    shp.Sent =
                        TimeUtils.GetEpochTime();
                    shp.Loaded =
                        0;
                    shp.Type = "upgradeable";
                    shp.TargetId =
                        bestplace.DefId;
                    shp.TargetLevel = bestplace.Level;
                    Logger.Logger.Info(Localization.SHIPS_SENDING +
                                       LocalizationCache.GetNameFromLoc(
                                           Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                                           Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
                    Networking.AddTask(new Task.SendShipUpgradeableTask(ship, bestplace, wecan));
                }
            }
        }
    }

    internal static class AutoShipUtils
    {
        public static bool isVoyageCompleted(Ship ship)
        {
            int? traveltime = null;
            switch (ship.Type)
            {
                case "upgradeable":
                    traveltime = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                         ?.Levels.Level
                         .First(n => n.Id == ship.TargetLevel).TravelTime;
                    break;
                case "marketplace":
                    traveltime = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId).TravelTime;
                    break;
                case "wreck":

                    var wrk = Core.GlobalData.Wrecks.Where(n => n.InstId == ship.TargetId).FirstOrDefault();
                    traveltime = (int)Definitions.WreckDef.Items.Item.Where(n => n.DefId == wrk.DefId).FirstOrDefault().TravelTime;
                    break;
                case "contractor":

                    traveltime = (int)Definitions.ConDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault().TravelTime;
                    break;
                case "global_contractor":

                    traveltime = (int)Definitions.GConDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault().TravelTime;
                    break;
                case "outpost":

                    traveltime = (int)Definitions.OutpostDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault().TravelTime;
                    break;
                case "social_contract":

                    traveltime = (int)Definitions.SContractDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault().TravelTime;
                    break;
                case "dealer":

                    traveltime = (int)Definitions.DealerDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault().TravelTime;
                    break;
            }
            if(traveltime==null)
            {
                return false;
            }
           return (TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds > traveltime;
        }
        public static ShipDefenitions.Item GetShipDefId(Ship ship)
        {
            return Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId);
        }

        public static void NullShip(Ship ship)
        {
            ship.Cargo = 0;
            ship.Crew = 0;
            ship.Loaded = 0;
            ship.MaterialId = 0;
            ship.Sent = 0;
            ship.Type = string.Empty;
            ship.TargetId = 0;
            ship.TargetLevel = 0;
        }

        public static ShipDefenitions.LevelsLevel GetLevels(Ship ship, int level)
        {
            return GetShipDefId(ship).Levels.Level.FirstOrDefault(n => n.Id == level);
        }

        public static int GetCapacity(Ship ship)
        {
            if (Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels == null)
            {
                var capacity = Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).CapacityLevels
                    .Level.First(n => n.Id == ship.CapacityLevel).Capacity;
                if (capacity != null)
                {
                    return capacity.Value;
                }
            }

            ShipDefenitions.Item first = null;
            foreach (var n in Definitions.ShipDef.Items.Item)
            {
                if (n.DefId == ship.DefId)
                {
                    first = n;
                    break;
                }
            }

            if (first != null)
            {
                return first.Levels.Level
                    .First(n => n.Id == ship.Level).Capacity;
            }

            return 0;
        }

        public static int GetSailors(Ship ship)
        {
            if (Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).SailorsLevels == null)
            {
                ShipDefenitions.Item first = null;
                foreach (var n in Definitions.ShipDef.Items.Item)
                {
                    if (n.DefId == ship.DefId)
                    {
                        first = n;
                        break;
                    }
                }

                if (first != null)
                {
                    return first.Levels
                        .Level.First(n => n.Id == ship.Level).Sailors;
                }
            }

            var sailors = Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId)
                .SailorsLevels.Level
                .First(n => n.Id == ship.SailorsLevel).Sailors;
            if (sailors != null)
            {
                return sailors.Value;
            }

            return int.MaxValue;
        }

        public static Upgradeable GetBestUpgPlace(string itemname, int sailors, bool profitbased)
        {
            var matid =
                Definitions.UpgrDef.Items.Item.Where(n => n.MaterialId == MaterialDB.GetItem(itemname).DefId);
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