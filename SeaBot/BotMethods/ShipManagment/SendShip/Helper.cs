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

namespace SeaBotCore.BotMethods.ShipManagment.SendShip
{
    using Newtonsoft.Json;

    using SeaBotCore.Cache;
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

  public static class SendingHelper
    {
         public static int GetPercentage(int procent, int total)
        {
            var globalkoef = total / 100D;
            var loccoef = globalkoef * procent;
            return (int) Math.Truncate(loccoef);
        }
        public static int Capacity(this Ship ship)
        {
            return GetCapacity(ship);
        }

        public static Upgradeable GetBestUpgPlace(string itemname, int sailors, bool profitbased)
        {
            var mat = MaterialDB.GetItem(itemname).DefId;
            var needed = new List<UpgradeableDefenition.Item>();
            foreach (var item in Definitions.UpgrDef.Items.Item)
            {
                if (item.MaterialId == mat)
                {
                    needed.Add(item);
                }
            }

            var sito = needed.Where(
                shtItem => Core.GlobalData.Upgradeables.FirstOrDefault(
                               n => n.DefId == shtItem.DefId && n.Amount != 0 && n.Progress < n.Amount
                                    || n.DefId == shtItem.DefId && shtItem.MaxLevel == 1 && shtItem.EventId == 0)
                           != null);
            var p = sito.ToDictionary(
                shtItem => shtItem,
                shtItem => Core.GlobalData.Upgradeables.First(n => n.DefId == shtItem.DefId));
            var best = new Dictionary<Upgradeable, decimal>();
            foreach (var up in p)
            {
                if (up.Key.Levels.Level.First(n => n.Id == up.Value.Level).Sailors > sailors)
                {
                    continue;
                }

                if (profitbased)
                {
                    var itemFirst = up.Key.Levels.Level.First(n => n.Id == up.Value.Level);
                    var time = (decimal)itemFirst.TravelTime;
                    var koef = itemFirst.MaterialKoef;
                    var timepercoin = koef / time;
                    best.Add(up.Value, timepercoin);
                }
                else
                {
                    var koef = (decimal)up.Key.Levels.Level.First(n => n.Id == up.Value.Level).MaterialKoef;
                    var timepercoin = koef / sailors;
                    best.Add(up.Value, timepercoin);
                }
            }

            var bestplace = best.OrderBy(n => n.Value).LastOrDefault();
            return bestplace.Key;
        }

        public static int GetCapacity(Ship ship)
        {
           
                if (Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId)?.Levels == null)
                {
                    var capacity = Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).CapacityLevels.Level
                        .FirstOrDefault(n => n.Id == ship.CapacityLevel)?.Capacity;
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
                    var pr = first.Levels.Level.FirstOrDefault(n => n.Id == ship.Level);
                    if (pr != null)
                    {
                        return pr.Capacity;
                    }
                }

                return 0;
            
         
        }

        public static ShipDefenitions.LevelsLevel GetLevels(Ship ship, int level)
        {
            return GetShipDefId(ship).Levels.Level.FirstOrDefault(n => n.Id == level);
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
                    return first.Levels.Level.First(n => n.Id == ship.Level).Sailors;
                }
            }

            var sailors = Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).SailorsLevels.Level
                .First(n => n.Id == ship.SailorsLevel).Sailors;
            if (sailors != null)
            {
                return sailors.Value;
            }

            return int.MaxValue;
        }

        public static ShipDefenitions.Item GetShipDefId(Ship ship)
        {
            return Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId);
        }

        public static string GetTravelName(this Ship ship)
        {
            var pointname = string.Empty;
            switch (ship.Type)
            {
                case "upgradeable":
                    pointname = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;
                    break;
                case "marketplace":
                    pointname = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;
                    break;
                case "wreck":

                    var wrk = Core.GlobalData.Wrecks.FirstOrDefault(n => n.InstId == ship.TargetId);
                    pointname = Definitions.WreckDef.Items.Item.FirstOrDefault(n => n.DefId == wrk.DefId)?.NameLoc;
                    break;
                case "contractor":

                    pointname = Definitions.ConDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;

                    break;
                case "global_contractor":

                    pointname = Definitions.GConDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;

                    break;
                case "outpost":

                    OutpostDefinitions.Item first = null;
                    foreach (var n in Definitions.OutpostDef.Items.Item)
                    {
                        if (n.DefId == ship.TargetId)
                        {
                            first = n;
                            break;
                        }
                    }

                    pointname = first?.NameLoc;
                    break;
                case "social_contract":

                    pointname = Definitions.SContractDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                        ?.NameLoc;

                    break;
                case "dealer":

                    pointname = Definitions.DealerDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;

                    break;
            }

            return pointname;
        }

        public static int GetTravelTime(this Ship ship)
        {
            int? traveltime = null;
            switch (ship.Type)
            {
                case "upgradeable":
                    traveltime = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.Levels
                        .Level.FirstOrDefault(n => n.Id == ship.TargetLevel)?.TravelTime;
                    break;
                case "marketplace":
                    traveltime = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                        ?.TravelTime;
                    break;
                case "wreck":

                    var wrk = Core.GlobalData.Wrecks.FirstOrDefault(n => n.InstId == ship.TargetId);
                    var tm = Definitions.WreckDef.Items.Item.FirstOrDefault(n => n.DefId == wrk.DefId)?.TravelTime;
                    if (tm != null)
                    {
                        traveltime = (int)tm;
                    }

                    break;
                case "contractor":

                    var l = Definitions.ConDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.TravelTime;
                    if (l != null)
                    {
                        traveltime = (int)l;
                    }

                    break;
                case "global_contractor":

                    var time = Definitions.GConDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.TravelTime;
                    if (time != null)
                    {
                        traveltime = (int)time;
                    }

                    break;
                case "outpost":

                    OutpostDefinitions.Item first = null;
                    foreach (var n in Definitions.OutpostDef.Items.Item)
                    {
                        if (n.DefId == ship.TargetId)
                        {
                            first = n;
                            break;
                        }
                    }

                    traveltime = first.TravelTime;
                    break;
                case "social_contract":
                    var soccontract = Core.GlobalData.SocialContracts.Where(n => n.InstId == ship.TargetId)
                        .FirstOrDefault();
                    var travelTime = Definitions.SContractDef.Items.Item
                        .FirstOrDefault(n => n.DefId == soccontract?.DefId)?.TravelTime;
                    if (travelTime != null)
                    {
                        traveltime = (int)travelTime;
                    }

                    break;
                case "dealer":

                    var o = Definitions.DealerDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.TravelTime;
                    if (o != null)
                    {
                        traveltime = (int)o;
                    }

                    break;
            }

            return !traveltime.HasValue ? int.MaxValue : traveltime.Value;
        }

        public static List<OutpostDefinitions.Item> GetUnlockableOutposts()
        {
            var lockedspots = new List<OutpostDefinitions.Item>();
            foreach (var lpost in Definitions.OutpostDef.Items.Item)
            {
                if (Core.GlobalData.Outposts.Any(n => n.DefId == lpost.DefId))
                {
                    continue;
                }

                // check for level
                if (lpost.ReqLevel < Core.GlobalData.Level)
                {
                    if (lpost.UnlockTime <= TimeUtils.GetEpochTime())
                    {
                        // check if unlocked spot
                        if (lpost.RequiredLocations == null)
                        {
                            continue;
                        }

                        if (lpost.EventId != TimeUtils.GetCurrentEvent().DefId)
                        {
                            if (lpost.EventId != 0)
                            {
                                continue;
                            }
                        }

                        foreach (var postreq in lpost.RequiredLocations.Location)
                        {
                            if (postreq.Type == "outpost")
                            {
                                var enogh = true;
                                var reqlids = postreq.Ids.Split(',');
                                foreach (var req in reqlids)
                                {
                                    var num = 0;
                                    if (int.TryParse(req, out num))
                                    {
                                        if (!Core.GlobalData.Outposts.Where(n => n.Done).Any(n => n.DefId == num))
                                        {
                                            enogh = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        enogh = false;
                                        break;
                                    }
                                }

                                if (enogh)
                                {
                                    lockedspots.Add(lpost);
                                }
                            }
                        }
                    }
                }
            }

            return lockedspots;
        }

        public static bool IsVoyageCompleted(this Ship ship)
        {
            return (TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds > ship.GetTravelTime();
        }

        public static void LogUnload(this Ship ship)
        {
            Logger.Info(
                Localization.SHIPS_UNLOADING + LocalizationCache.GetNameFromLoc(
                    Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId)?.NameLoc,
                    Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId)?.Name));
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

        public static int Sailors(this Ship ship)
        {
            return GetSailors(ship);
        }

    }
}
