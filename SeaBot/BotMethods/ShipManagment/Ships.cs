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
namespace SeaBotCore.BotMethods
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SeaBotCore.Cache;
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    #endregion

    public static class Ships
    {
        public static void AutoShip(string type, bool lootbased)
        {
            // unload
            for (var index = 0; index < Core.GlobalData.Ships.Count; index++)
            {
                var ship = Core.GlobalData.Ships[index];
                if (ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 0 && ship.IsVoyageCompleted())
                {
                    if (ship.Type == "upgradeable")
                    {
                        var lvl = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.Levels
                            .Level.First(n => n.Id == ship.TargetLevel);
                        if (lvl != null)
                        {
                            Logger.Info(
                                Localization.SHIPS_LOADING + LocalizationCache.GetNameFromLoc(
                                    Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                                    Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
                            Core.GlobalData.Upgradeables.First(n => n.DefId == ship.TargetId).Progress +=
                                lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship);

                            Networking.AddTask(new Task.LoadShipUpgradeableTask(ship.InstId));
                            Core.GlobalData.Ships[index].Loaded = 1;
                        }
                    }

                    if (ship.Type == "wreck" && ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 0
                        && ship.IsVoyageCompleted())
                    {
                        Logger.Info(
                            Localization.SHIPS_LOADING + LocalizationCache.GetNameFromLoc(
                                Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                                Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));

                        Networking.AddTask(new Task.LoadShipWreck(ship.InstId));
                        Core.GlobalData.Ships[index].Loaded = 1;
                    }
                }
            }

            // unload
           ShipManagment.UnloadShips.UnloadAllShips();
          

            // now send
            if (true)
            {
               
                    List<Ship> failedships = new List<Ship>();

                    var bestships = new Queue<Ship>(Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0 && n.Sent == 0)
                        .OrderByDescending(AutoShipUtils.GetCapacity));
                    var z = 0;
                    //First 30% to upgradable
                    int upgcont = AutoShipUtils.GetPercentage(30,bestships.Count);
                    int outpostcont = AutoShipUtils.GetPercentage(20,bestships.Count);
                    int marketcount = AutoShipUtils.GetPercentage(15,bestships.Count);
                    int contractorcount = AutoShipUtils.GetPercentage(30,bestships.Count);
                    int wreckcount =  AutoShipUtils.GetPercentage(5,bestships.Count);
                    foreach (var ship in AutoTools.TakeAndRemove(bestships,upgcont))
                    {
                        AutoShipUtils.SendToUpgradable(ship, Core.Config.autoshiptype);
                    }
                   

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,outpostcont))
                    {
                        AutoShipUtils.SendToOutpost(ship);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,marketcount))
                    {
                        AutoShipUtils.SendToMarketplace(ship);
                        Logger.Info("mkt"+Definitions.ShipDef.Items.Item.Where(n=>n.DefId==ship.DefId).First().Name);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,contractorcount))
                    {
                        AutoShipUtils.SendToContractor(ship);
                        Logger.Info("ctr"+Definitions.ShipDef.Items.Item.Where(n=>n.DefId==ship.DefId).First().Name);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,wreckcount))
                    {
                        AutoShipUtils.SendToWreck(ship);
                        Logger.Info("wrk"+Definitions.ShipDef.Items.Item.Where(n=>n.DefId==ship.DefId).First().Name);
                    }
                    
            }
        }
    }

    public static class AutoShipUtils
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
            if (Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels == null)
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
                return first.Levels.Level.First(n => n.Id == ship.Level).Capacity;
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
                    traveltime = (int)Definitions.WreckDef.Items.Item.FirstOrDefault(n => n.DefId == wrk.DefId)
                        ?.TravelTime;
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

            if (!traveltime.HasValue)
            {
                return int.MaxValue;
            }

            return traveltime.Value;
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
                    Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                    Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
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

        public static bool SendToContractor(Ship ship)
        {
            var statiopst = new List<Data.Contractor>();
            var genopst = new List<Data.Contractor>();
            foreach (var contractor in Core.GlobalData.Contracts)
            {
                if (contractor.Done == 0)
                {
                    var def = Definitions.ConDef.Items.Item.Where(c => contractor.DefId == c.DefId).FirstOrDefault();
                    var quest = def.Quests.Quest.Where(q => contractor.QuestId == q.Id).First();
                    if (Core.GlobalData.GetAmountItem(quest.ObjectiveDefId) >= quest.InputAmount())
                    {
                        if (def.Type == "static")
                        {
                            statiopst.Add(contractor);
                        }
                        else
                        {
                            genopst.Add(contractor);
                        }
                    }
                }
            }

            foreach (var opst in statiopst)
            {
                var def = Definitions.ConDef.Items.Item.Where(c => opst.DefId == c.DefId).FirstOrDefault();
                var quest = def.Quests.Quest.Where(q => opst.QuestId == q.Id).First();
                var already = opst.Progress;
                var exists = quest.Amount - opst.Progress;
                if (exists == 0)
                {
                    continue;
                }

                var wecan = 0;
                if (exists * quest.MaterialKoef > ship.Capacity())
                {
                    wecan = ship.Capacity();
                }
                else
                {
                    wecan = exists;
                }

                Core.GlobalData.Contracts.Where(n => n.DefId == opst.DefId).First().CargoOnTheWay +=
                    wecan * quest.MaterialKoef;
                Core.GlobalData.Contracts.Where(n => n.DefId == opst.DefId).First().Progress +=
                    wecan * quest.MaterialKoef;
                Logger.Info("TEMPLATE: SENDING A SHIP TO CONTRACTOR");
                Networking.AddTask(
                    new Task.SendShipContractorTask(
                        ship.InstId,
                        opst.DefId,
                        quest.ObjectiveDefId,
                        quest.Id,
                        wecan * quest.MaterialKoef));
                var lship = Core.GlobalData.Ships.Where(n => n.DefId == ship.DefId).FirstOrDefault();
                lship.Sent = TimeUtils.GetEpochTime();
                lship.Loaded = 0;
                lship.Type = "contractor";
                lship.TargetId = opst.DefId;
                lship.TargetLevel = quest.Id;
                return true;
            }

            foreach (var opst in genopst)
            {
                var def = Definitions.ConDef.Items.Item.Where(c => opst.DefId == c.DefId).FirstOrDefault();
                var quest = def.Quests.Quest.Where(q => opst.QuestId == q.Id).First();
                
                var already = opst.Progress;
                var exists = (int)quest.InputAmount() - opst.Progress;
                if (exists == 0)
                {
                    continue;;
                }
                var wecan = 0;
                if (exists * quest.MaterialKoef > ship.Capacity())
                {
                    wecan = ship.Capacity();
                }
                else
                {
                    wecan = exists;
                }

                Core.GlobalData.Contracts.Where(n => n.DefId == opst.DefId).First().CargoOnTheWay +=
                    wecan * quest.MaterialKoef;
                Core.GlobalData.Contracts.Where(n => n.DefId == opst.DefId).First().Progress +=
                    wecan * quest.MaterialKoef;
                var lship = Core.GlobalData.Ships.Where(n => n.DefId == ship.DefId).FirstOrDefault();
                lship.Loaded = 0;
                lship.Type = "contractor";
                lship.TargetId = opst.DefId;
                lship.TargetLevel = quest.Id;
                Logger.Info("TEMPLATE: SENDING A SHIP TO CONTRACTOR");
                Networking.AddTask(
                    new Task.SendShipContractorTask(
                        ship.InstId,
                        opst.DefId,
                        quest.ObjectiveDefId,
                        quest.Id,
                        wecan * quest.MaterialKoef));
                return true;
            }

            return false;

            // KAAAAAAAAAAAAZOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO?
        }

        public static bool SendToMarketplace(Ship ship)
        {
            // Outpost send!

            // 1. Find with done == 0
            var marketplacepoints = AutoTools.GetEnabledMarketPlacePoints();

            var neededitems = AutoTools.NeededItemsForUpgradePercentage().OrderBy(pair => pair.Value).Select(n => n.Key)
                .ToList();

            var maktplc = new MarketplaceDefenitions.Material();

            if (ship.Sailors() < Definitions.MarketDef.Items.Item[1].Sailors)
            {
                return false;
            }

            var placeswithneeded = new List<MarketplaceDefenitions.Material>();
            foreach (var need in neededitems)
            foreach (var plc in Definitions.MarketDef.Items.Item[1].Materials.Material)
            {
                if (plc.OutputId == need)
                {
                    placeswithneeded.Add(plc);
                }
            }

            var found = false;
            foreach (var inpMaterial in placeswithneeded)
            {
                if (found)
                {
                    break;
                }

                foreach (var point in marketplacepoints)
                {
                    if (inpMaterial.InputId == point.Id)
                    {
                        maktplc = inpMaterial;
                        found = true;
                        break;
                    }
                }
            }

            if (maktplc.OutputType == null)
            {
                var can = Definitions.MarketDef.Items.Item[1].Materials.Material.Where(
                    n => n.InputId == marketplacepoints.OrderByDescending(b => b.Amount).FirstOrDefault()?.Id).ToList();
                if (can.Count > 0)
                {
                    maktplc = can[new Random().Next(can.Count)];
                }
            }

            if (maktplc?.OutputType == null)
            {
                return false;
            }

            var wehaveininv = Core.GlobalData.GetAmountItem(maktplc.InputId);
            var canproceed = 0;
            canproceed = wehaveininv < ship.Capacity() ? wehaveininv : ship.Capacity();

            Core.GlobalData.Inventory.Where(n => n.Id == maktplc.InputId).FirstOrDefault().Amount -= canproceed;
            Logger.Info("TEMPLATE: SENDING A SHIP TO MARKETPLACE");
            Networking.AddTask(new Task.SendShipMarketplaceTask(ship.InstId, maktplc.Id, 1, canproceed));
            var locship = Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First();
            locship.Type = "marketplace";
            locship.TargetId = 1;
            locship.TargetLevel = 0;
            locship.MaterialId = 0;
            locship.Sent = TimeUtils.GetEpochTime();
            locship.MaterialId = maktplc.Id;
            locship.Loaded = 0;
            return true;
        }

        public static bool SendToOutpost(Ship ship)
        {
            var opst = Core.GlobalData.Outposts.Where(n => !n.Done && n.Crew < n.RequiredCrew).FirstOrDefault();
            if (opst != null)
            {
                var can = opst.RequiredCrew - opst.Crew;
                var sending = 0;
                if (can > GetSailors(ship))
                {
                    sending = GetSailors(ship);
                }
                else
                {
                    sending = can;
                }

                opst.Crew += sending;
                Networking.AddTask(new Task.OutpostSendShipTask(ship.InstId, opst.DefId, sending));
                return true;
            }
            else
            {
                var locked = GetUnlockableOutposts();
                if (locked.Count == 0)
                {
                    return false;
                }

                var next = locked.OrderBy(n => n.Sailors).FirstOrDefault();
                if (next == null)
                {
                    return false;
                }

                var sending = 0;
                sending = next.Crew > GetSailors(ship) ? GetSailors(ship) : next.Crew;

                Logger.Info("TEMPLATE: SENDING A SHIP TO OUTPOST");
                Networking.AddTask(new Task.OutpostSendShipTask(ship.InstId, next.DefId, sending));
                Core.GlobalData.Outposts.Add(
                    new Outpost
                        {
                            CargoOnTheWay = sending,
                            Crew = sending,
                            DefId = sending,
                            Done = false,
                            PlayerLevel = Core.GlobalData.Level,
                            RequiredCrew = next.Crew
                        });
                return true;
            }

            // KAAAAAAAAAAAAZOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO?
        }

        public static bool SendToUpgradable(Ship ship, string itemname)
        {
            var bestplace = GetBestUpgPlace(itemname, GetSailors(ship), !Core.Config.autoshipprofit);

            if (bestplace == null || Core.GlobalData.Sailors < GetSailors(ship))
            {
                return false;
            }

            var place = Definitions.UpgrDef.Items.Item.First(n => n.DefId == bestplace.DefId);
            var shipfull = Definitions.ShipDef.Items.Item.Where(n => n.DefId == ship.DefId).FirstOrDefault();
            var lvls = place.Levels.Level.FirstOrDefault(n => n.Id == bestplace.Level);

            if (shipfull.SlotUsage < place.Slots)
            {
                return false;
            }

            if (lvls != null)
            {
                var wecan = lvls.MaterialKoef * GetCapacity(ship);
                var remain = bestplace.Amount - bestplace.Progress;
                if (remain < wecan)
                {
                    wecan = remain;
                }

                Core.GlobalData.Sailors -= lvls.Sailors;

                bestplace.CargoOnTheWay += wecan;
                var shp = Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First();
                shp.Sent = TimeUtils.GetEpochTime();
                shp.Loaded = 0;
                shp.Type = "upgradeable";
                shp.TargetId = bestplace.DefId;
                shp.TargetLevel = bestplace.Level;
                Logger.Info(
                    Localization.SHIPS_SENDING + LocalizationCache.GetNameFromLoc(
                        Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                        Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
                Networking.AddTask(new Task.SendShipUpgradeableTask(ship, bestplace, wecan));
                return true;
            }

            return false;
        }
        
        public static bool SendToWreck(Ship ship)
        {
            var wreck = Core.GlobalData.Wrecks.Where(n => n.Status == 0).FirstOrDefault();

            if (wreck != null)
            {
                if (wreck.Sailors < ship.Sailors())
                {
                    var shp = Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First();
                    shp.Sent = TimeUtils.GetEpochTime();
                    shp.Loaded = 0;
                    shp.Type = "wreck";
                    shp.TargetId = wreck.InstId;
                    shp.TargetLevel = 0;
                    wreck.Status = 1;
                    Logger.Info(
                        Localization.SHIPS_SENDING + LocalizationCache.GetNameFromLoc(
                            Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                            Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
                    Networking.AddTask(new Task.SendShipwreckTask(ship.InstId, wreck.InstId));
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}