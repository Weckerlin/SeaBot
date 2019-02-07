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
                if ( ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 0&&ship.IsVoyageCompleted())
                {
                    if (ship.Type == "upgradeable")
                    {
                        var lvl = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                            ?.Levels.Level
                            .First(n => n.Id == ship.TargetLevel);
                        if (lvl != null)
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
                    if (ship.Type == "wreck" && ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 0&&ship.IsVoyageCompleted())
                    {
                        Logger.Logger.Info(
                            Localization.SHIPS_LOADING +
                            LocalizationCache.GetNameFromLoc(
                                Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                                Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
                     


                        Networking.AddTask(new Task.LoadShipWreck(ship.InstId));
                        Core.GlobalData.Ships[index].Loaded = 1;
                    
                    }
                }
            }
            //unload
       

            var _deship = new List<Ship>();
            for (var index = 0; index < Core.GlobalData.Ships.Count; index++)
            {

                var ship = Core.GlobalData.Ships[index];
                if (ship.TargetId != 0 && ship.Activated != 0)
                {
                    if (ship.Loaded == 1 && ship.Type == "upgradeable"&&ship.IsVoyageCompleted())
                    {
                        var lvl = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                            ?.Levels.Level
                            .FirstOrDefault(n => n.Id == ship.TargetLevel);
                        if (lvl != null)
                        {
                            ship.LogUnload();
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

                    if (ship.Type == "marketplace"&&ship.IsVoyageCompleted())
                    {
                        var market = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId);
                        var lvl = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId).Materials
                            .Material.Where(n => n.Id == ship.MaterialId).FirstOrDefault();

                        if (lvl != null)
                        {
                            ship.LogUnload();
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

                        if (wrk != null && ship.IsVoyageCompleted())
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

                    ////Contractor
                    if (ship.Type == "contractor")
                    {
                        if (ship.IsVoyageCompleted())
                        {
                            var currentcontractor = Definitions.ConDef.Items.Item.Where(n => n.DefId == ship.TargetId)
                                .FirstOrDefault();
                            var quest = currentcontractor?.Quests.Quest.Where(n => n.Id == ship.TargetLevel).FirstOrDefault();
                            if (quest == null) continue;
                            var usedshit = quest.MaterialKoef * AutoShipUtils.GetCapacity(ship);
                            _deship.Add(ship);
                            var lcontract = Core.GlobalData.Contracts.Where(n => n.DefId == ship.TargetId)
                                .FirstOrDefault();
                            //TODO: increasing of progress or amount!

                            ship.LogUnload();
                            Networking.AddTask(new Task.DockShipTaskContractor(ship, false, AutoShipUtils.GetCapacity(ship), usedshit, AutoShipUtils.GetSailors(ship), currentcontractor.Sailors, ship.TargetLevel, currentcontractor.DefId, lcontract.Progress, (int)quest.InputAmount(), quest.ObjectiveTypeId, _deship.Count(n => n.DefId == ship.DefId)));

                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    //if (ship.Type == "global_contractor")
                    //{
                    //    var predefined = Definitions.GConDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
                    //    if (AutoShipUtils.isVoyageCompleted(ship))
                    //    {
                    //        _deship.Add(ship);
                    //        Networking.AddTask(new Task.UnloadShipGlobalContractorTask(ship.InstId));
                    //        AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                    //    }
                    //}

                    if (ship.Type == "outpost"&&ship.IsVoyageCompleted())
                    {
                        
                            _deship.Add(ship);
                            var loc = Core.GlobalData.Outposts.Where(n => n.DefId == ship.TargetId).First();
                            ship.LogUnload();
                            Networking.AddTask(new Task.DockShipTaskOutPost(ship,false,AutoShipUtils.GetCapacity(ship),ship.Cargo,AutoShipUtils.GetSailors(ship),ship.Crew,ship.TargetLevel,loc.CargoOnTheWay+loc.Crew,loc.RequiredCrew, _deship.Count(n => n.DefId == ship.DefId)));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        
                    }

                    if (ship.Type == "social_contract")
                    {
                        if (ship.IsVoyageCompleted())
                        {
                            _deship.Add(ship);
                            var co = Core.GlobalData.SocialContracts.Where(n => n.InstId == ship.TargetId)
                                .FirstOrDefault();
                            if (co == null)
                            {
                                continue;
                            }
                          ship.LogUnload();
                            Networking.AddTask(new Task.DockShipSocialContractor(ship,false,ship.Capacity(),(int)co.MaterialKoef/co.Amount,ship.Sailors(),co.Sailors,ship.TargetLevel,_deship.Count(n => n.DefId == ship.DefId)));
                            AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                        }
                    }

                    //if (ship.Type == "dealer")
                    //{
                    //    var predefined = Definitions.DealerDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
                    //    if (AutoShipUtils.isVoyageCompleted(ship))
                    //    {

                    //        _deship.Add(ship);
                    //        Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
                    //            Core.GlobalData.Level, Enums.EObject.dealer,
                    //            AutoShipUtils.GetCapacity(ship),
                    //            0,
                    //            AutoShipUtils.GetSailors(ship), (int)predefined.Sailors,
                    //            ship.TargetLevel,
                    //            null, _deship.Count(n => n.DefId == ship.DefId)));
                    //        AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                    //    }
                    //}

                    //if (ship.Type == "treasure")
                    //{
                    //    var predefined = Definitions.TreasureDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
                    //    if (AutoShipUtils.isVoyageCompleted(ship))
                    //    {
                    //        _deship.Add(ship);
                    //        Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
                    //            Core.GlobalData.Level, Enums.EObject.treasure,
                    //            AutoShipUtils.GetCapacity(ship),
                    //            0,
                    //            AutoShipUtils.GetSailors(ship), 0,
                    //            ship.TargetLevel,
                    //            null, _deship.Count(n => n.DefId == ship.DefId)));
                    //        AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
                    //    }
                    //}
                }
            }

            _deship.Clear();
            var bestships = Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0 && n.Sent == 0)
                .OrderByDescending(AutoShipUtils.GetCapacity).ToList();
            //now send
            if (true)
            {
                foreach (var VARIABLE in bestships)
                {
                    AutoShipUtils.SendToContractor(VARIABLE);
            //        AutoShipUtils.SendToMarketplace(VARIABLE);
                }
               
            }
            else
            {
               

            }
            
        }
    }

    public static class AutoShipUtils
    {
        public static void LogUnload(this Ship ship)
        {
            Logger.Logger.Info(Localization.SHIPS_UNLOADING +
                               LocalizationCache.GetNameFromLoc(
                                   Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).NameLoc,
                                   Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name));
        }
      
        
        public static List<OutpostDefinitions.Item> GetUnlockableOutposts()
        {
            var lockedspots = new List<OutpostDefinitions.Item>();
            foreach (var lpost in Definitions.OutpostDef.Items.Item)
            { 
                if(Core.GlobalData.Outposts.Any(n=>n.DefId==lpost.DefId))
                {
                    continue;
                }
                //check for level
                if (lpost.ReqLevel < Core.GlobalData.Level)
                {
                    //check for unlocktime
                    if (lpost.UnlockTime <= TimeUtils.GetEpochTime())
                    {
                        //check if unlocked spot
                        if(lpost.RequiredLocations==null)
                        {
                            continue;
                        }
                        if(lpost.EventId!=TimeUtils.GetCurrentEvent().DefId)
                        {
                            if(lpost.EventId!=0)
                            {
                                continue;
                            }
                        }
                        foreach (var postreq in lpost.RequiredLocations.Location)
                        {
                            if (postreq.Type == "outpost")
                            {
                                bool enogh = true;
                                var reqlids = postreq.Ids.Split(',');
                                foreach (var req in reqlids)
                                {
                                    var num = 0;
                                    if (Int32.TryParse(req, out num))
                                    {
                                        if (!Core.GlobalData.Outposts.Where(n=>n.Done).Any(n => n.DefId == num))
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

        public static bool IsVoyageCompleted( this Ship ship)
        {
            return (TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds > ship.GetTravelTime();
        }
        public static int GetTravelTime(this Ship ship)
        {
            int? traveltime = null;
            switch (ship.Type)
            {
                case "upgradeable":
                    traveltime = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                         ?.Levels.Level
                         .FirstOrDefault(n => n.Id == ship.TargetLevel)?.TravelTime;
                    break;
                case "marketplace":
                    traveltime = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.TravelTime;
                    break;
                case "wreck":

                    var wrk = Core.GlobalData.Wrecks.FirstOrDefault(n => n.InstId == ship.TargetId);
                    traveltime = (int)Definitions.WreckDef.Items.Item.FirstOrDefault(n => n.DefId == wrk.DefId)?.TravelTime;
                    break;
                case "contractor":

                    var l = Definitions.ConDef.Items.Item
                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.TravelTime;
                    if (l !=
                        null)
                        traveltime = (int)l;
                    break;
                case "global_contractor":

                    var time = Definitions.GConDef.Items.Item
                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.TravelTime;
                    if (time !=
                        null)
                        traveltime = (int)time;
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
                    var soccontract = Core.GlobalData.SocialContracts.Where(n=>n.InstId==ship.TargetId).FirstOrDefault();
                    var travelTime = Definitions.SContractDef.Items.Item
                        .FirstOrDefault(n => n.DefId == soccontract?.DefId)?.TravelTime;
                    if (travelTime !=
                        null)
                        traveltime = (int)travelTime;
                    break;
                case "dealer":

                    var o = Definitions.DealerDef.Items.Item
                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.TravelTime;
                    if (o !=
                        null)
                        traveltime = (int)o;
                    break;
            }
            if(!traveltime.HasValue)
            {
                return int.MaxValue;
            }

           return traveltime.Value;
        }
        public static string GetTravelName(this Ship ship)
        {
            string pointname ="";
            switch (ship.Type)
            {
                case "upgradeable":
                    pointname = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                         ?.NameLoc;
                    break;
                case "marketplace":
                    pointname = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;
                    break;
                case "wreck":

                    var wrk = Core.GlobalData.Wrecks.FirstOrDefault(n => n.InstId == ship.TargetId);
                    pointname = Definitions.WreckDef.Items.Item.FirstOrDefault(n => n.DefId == wrk.DefId)?.NameLoc;
                    break;
                case "contractor":

                     pointname = Definitions.ConDef.Items.Item
                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;
                 
                    break;
                case "global_contractor":

                    pointname = Definitions.GConDef.Items.Item
                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;
                  
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

                    pointname = Definitions.SContractDef.Items.Item
                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;
                    
                    break;
                case "dealer":

                    pointname = Definitions.DealerDef.Items.Item
                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.NameLoc;
             
                    break;
            }
            return pointname;
        }
        public static ShipDefenitions.Item GetShipDefId(Ship ship)
        {
            return Definitions.ShipDef.Items.Item.FirstOrDefault(n => n.DefId == ship.DefId);
        }

        public static bool SendToUpgradable(Ship ship,string itemname)
        {
             
              
                    var bestplace = AutoShipUtils.GetBestUpgPlace(itemname, AutoShipUtils.GetSailors(ship), !Core.Config.autoshipprofit);

                    if (bestplace == null || Core.GlobalData.Sailors < AutoShipUtils.GetSailors(ship))
                    {
                        return false;
                    }

                    var place = Definitions.UpgrDef.Items.Item.First(n => n.DefId == bestplace.DefId);
                    var shipfull = Definitions.ShipDef.Items.Item.Where(n => n.DefId == ship.DefId).FirstOrDefault();
                    var lvls = place.Levels
                        .Level.FirstOrDefault(n => n.Id == bestplace.Level);

                    if (shipfull.SlotUsage < place.Slots)
                    {
                        return false;
                    }
                    
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
                        return true;
                    }

                    return false;

        }
        public static bool SendToContractor(Ship ship)
        {
            var opst = Core.GlobalData.Outposts.Where(n => !n.Done && n.Crew < n.RequiredCrew).FirstOrDefault();
            if (opst != null)
            {
                var can = opst.RequiredCrew - opst.Crew;
                var sending = 0;
                if (can > AutoShipUtils.GetSailors(ship))
                {
                    sending = AutoShipUtils.GetSailors(ship);
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
                var locked = AutoShipUtils.GetUnlockableOutposts();
                if (locked.Count == 0)
                {
                    return false;
                }

                var next = locked.OrderBy(n=>n.Sailors).FirstOrDefault();
                if (next == null)
                {
                    return false;
                }
               
                var sending = 0;
                sending = next.Crew > AutoShipUtils.GetSailors(ship) ? AutoShipUtils.GetSailors(ship) : next.Crew;

                
                Networking.AddTask(new Task.OutpostSendShipTask(ship.InstId, next.DefId, sending));
                Core.GlobalData.Outposts.Add(new Outpost(){CargoOnTheWay = sending,Crew = sending,DefId = sending,Done = false,PlayerLevel = Core.GlobalData.Level,RequiredCrew = next.Crew});
                return true;
            }
            //KAAAAAAAAAAAAZOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO?
        }
        public static bool SendToOutPost(Ship ship)
        {
            var opst = Core.GlobalData.Outposts.Where(n => !n.Done && n.Crew < n.RequiredCrew).FirstOrDefault();
            if (opst != null)
            {
                var can = opst.RequiredCrew - opst.Crew;
                var sending = 0;
                if (can > AutoShipUtils.GetSailors(ship))
                {
                    sending = AutoShipUtils.GetSailors(ship);
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
                var locked = AutoShipUtils.GetUnlockableOutposts();
                if (locked.Count == 0)
                {
                    return false;
                }

                var next = locked.OrderBy(n=>n.Sailors).FirstOrDefault();
                if (next == null)
                {
                    return false;
                }
               
                var sending = 0;
                sending = next.Crew > AutoShipUtils.GetSailors(ship) ? AutoShipUtils.GetSailors(ship) : next.Crew;

                
                Networking.AddTask(new Task.OutpostSendShipTask(ship.InstId, next.DefId, sending));
               Core.GlobalData.Outposts.Add(new Outpost(){CargoOnTheWay = sending,Crew = sending,DefId = sending,Done = false,PlayerLevel = Core.GlobalData.Level,RequiredCrew = next.Crew});
               return true;
            }
            //KAAAAAAAAAAAAZOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO?
        }
        public static bool SendToMarketplace(Ship ship)
        {
            //Outpost send!

            //1. Find with done == 0

            var marketplacepoints = AutoTools.GetEnabledMarketPlacePoints();
            
            var neededitems = AutoTools.NeededItemsForUpgrade().OrderByDescending(pair => pair.Value).Select(n=>n.Key).ToList();
           
            MarketplaceDefenitions.Material maktplc = new MarketplaceDefenitions.Material();

            if (ship.Sailors() < Definitions.MarketDef.Items.Item[1].Sailors)
            {
                return false;}
            var placeswithneeded = new List<MarketplaceDefenitions.Material>();
            foreach (var need in neededitems)
            {
              
                foreach (var plc in Definitions.MarketDef.Items.Item[1].Materials.Material)
                {
                    if (plc.OutputId == need)
                    {
                        placeswithneeded.Add(plc);
                    }
                }
            }
            bool found = false;
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
                var can = Definitions.MarketDef.Items.Item[1].Materials.Material.Where(n =>
                    n.InputId == marketplacepoints.OrderByDescending(b => b.Amount).FirstOrDefault()?.Id).ToList();
                if (can.Count > 0)
                {
                    maktplc = can[new Random().Next(can.Count)];
                }
            }

            if (maktplc?.OutputType == null)
            {
             
                //Find random
                return false;
            }

            var wehaveininv = Core.GlobalData.GetAmountItem(maktplc.InputId);
            var canproceed = 0;
            canproceed = wehaveininv < ship.Capacity() ? wehaveininv : ship.Capacity();
            
            Core.GlobalData.Inventory.Where(n=>n.Id==maktplc.InputId).FirstOrDefault().Amount-=canproceed;
            Logger.Logger.Info("Sending...");
            Networking.AddTask(new Task.SendShipMarketplaceTask(ship.InstId,maktplc.Id,1,canproceed));
            var locship = Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First();
            locship.Type = "marketplace";
            locship.TargetId = 1;
            locship.TargetLevel = 0;
            locship.Sent = TimeUtils.GetEpochTime();
            locship.Loaded =
                0;
            return true;

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

        public static int Capacity(this Ship ship)
        {
            return GetCapacity(ship);
        }
        public static int GetCapacity(Ship ship)
        {
            if (Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels == null)
            {
                var capacity = Definitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).CapacityLevels
                    .Level.FirstOrDefault(n => n.Id == ship.CapacityLevel)?.Capacity;
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

        public static int Sailors(this Ship ship)
        {
            return GetSailors(ship);
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
            var mat = MaterialDB.GetItem(itemname).DefId;
            List<UpgradeableDefenition.Item> needed = new List<UpgradeableDefenition.Item>();
            foreach (var item in Definitions.UpgrDef.Items.Item)
            {
                if (item.MaterialId == mat)
                {
                    needed.Add(item);
                }
            }

            var sito = needed.Where(shtItem =>
                Core.GlobalData.Upgradeables.FirstOrDefault(n =>
                    n.DefId == shtItem.DefId && n.Amount != 0 && n.Progress < n.Amount ||
                    n.DefId == shtItem.DefId && shtItem.MaxLevel==1 && shtItem.EventId==0) !=
                null);
            var p = sito
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