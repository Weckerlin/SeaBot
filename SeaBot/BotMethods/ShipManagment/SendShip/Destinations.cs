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
    using SeaBotCore.Cache;
    using SeaBotCore.Config;
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    public static class Destinations
    {
        public static bool SendToContractor(Ship ship)
        {
            
            if (ship == null)
            {
                return false;
            }
            var statiopst = new List<Contractor>();
            var genopst = new List<Contractor>();
            foreach (var contractor in Core.LocalPlayer.Contracts)
            {
                if (contractor == null)
                {
                    continue;
                }

                if (contractor.Done != 0)
                {
                    continue;
                }

                if (Core.Config.ignoreddestination.Count(
                        n => n.Destination == ShipDestType.Contractor && n.DefId == contractor.DefId) != 0)
                {
                    continue;
                }

                var def = LocalDefinitions.Contractors.FirstOrDefault(c => contractor.DefId == c.DefId);
                if (def.Sailors > ship.Sailors() || def.Sailors > Core.LocalPlayer.Sailors)
                {
                    continue;
                }

                var quest = def?.Quests.Quest.FirstOrDefault(q => contractor.QuestId == q.Id);
                if (quest == null)
                {
                    continue;
                }


                if (def.EventId != 0 && def.EventId != TimeUtils.GetCurrentEvent().DefId)
                {
                    if (!Core.Config.exploitmode)
                    {
                        continue;
                    }
                }

                if (quest.ObjectiveTypeId == "sailor")
                {
                    if (Core.LocalPlayer.Sailors < ship.Sailors())
                    {
                        continue;
                    }
                }
                else
                {
                    if (Core.LocalPlayer.GetAmountItem(quest.ObjectiveDefId) < quest.InputAmount())
                    {
                        continue;
                    }
                }

                if (def.Type == "static")
                {
                    statiopst.Add(contractor);
                }
                else
                {
                    genopst.Add(contractor);
                }
            }

            foreach (var opst in statiopst.OrderBy(n => n.QuestId))
            {

                var def = LocalDefinitions.Contractors.FirstOrDefault(c => opst.DefId == c.DefId);
                var quest = def?.Quests.Quest.FirstOrDefault(q => opst.QuestId == q.Id);
                if (def.Sailors > ship.Sailors() || def.Sailors>Core.LocalPlayer.Sailors)
                {
                    continue;
                }
                if (quest == null || def == null)
                {
                    continue;
                }
                var exists = quest.Amount - opst.Progress;
                if (exists <= 0)
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
              
                Core.LocalPlayer.Contracts.First(n => n.DefId == opst.DefId).CargoOnTheWay +=
                    wecan * quest.MaterialKoef;
                Core.LocalPlayer.Contracts.First(n => n.DefId == opst.DefId).Progress +=
                    wecan * quest.MaterialKoef;
                Logger.Info(string.Format(Localization.DESTINATION_CONTRACTOR, ship.GetShipName()));
                Networking.AddTask(
                    new Task.SendShipContractorTask(
                        ship.InstId,
                        opst.DefId,
                        quest.ObjectiveDefId,
                        quest.Id,
                        wecan * quest.MaterialKoef));
                var lship = Core.LocalPlayer.Ships.FirstOrDefault(n => n.DefId == ship.DefId);
                lship.Sent = TimeUtils.GetEpochTime();
                lship.Loaded = 0;
                lship.Type = "contractor";
                lship.TargetId = opst.DefId;
                lship.MaterialId = quest.ObjectiveDefId;
                lship.TargetLevel = quest.Id;
                return true;
            }

            foreach (var opst in genopst.OrderBy(n => n.QuestId))
            {
                var def = LocalDefinitions.Contractors.FirstOrDefault(c => opst.DefId == c.DefId);
                var quest = def?.Quests.Quest.FirstOrDefault(q => opst.QuestId == q.Id);
                if (quest == null)
                {
                    continue;
                }
                var already = opst.Progress;
                var exists = (int)quest.InputAmount() - opst.Progress;
                if (exists <= 0)
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

                Core.LocalPlayer.Contracts.First(n => n.DefId == opst.DefId).CargoOnTheWay +=
                    wecan * quest.MaterialKoef;
                Core.LocalPlayer.Contracts.First(n => n.DefId == opst.DefId).Progress +=
                    wecan * quest.MaterialKoef;
                var lship = Core.LocalPlayer.Ships.FirstOrDefault(n => n.DefId == ship.DefId);
                lship.Sent = TimeUtils.GetEpochTime();
                lship.Loaded = 0;
                lship.Type = "contractor";
                lship.TargetId = opst.DefId;
                lship.MaterialId = quest.ObjectiveDefId;
                lship.TargetLevel = quest.Id;
                Logger.Info(string.Format(Localization.DESTINATION_CONTRACTOR, ship.GetShipName()));
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
        }

        public static bool SendToMarketplace(Ship ship)
        {
            // Outpost send!

            // 1. Find with done == 0
            var marketplacepoints = AutoTools.GetEnabledMarketPlacePoints();

            var neededitems = AutoTools.NeededItemsForUpgradePercentage().OrderBy(pair => pair.Value).Select(n => n.Key)
                .ToList();

            var maktplc = new MarketplaceDefenitions.Material();

            if (ship.Sailors() < LocalDefinitions.Marketplaces[1].Sailors||Core.LocalPlayer.Sailors<ship.Sailors())
            {
                return false;
            }
            

            var placeswithneeded = new List<MarketplaceDefenitions.Material>();
            foreach (var need in neededitems)
            foreach (var plc in  LocalDefinitions.Marketplaces[1].Materials.Material)
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
                var can = LocalDefinitions.Marketplaces[1].Materials.Material.Where(
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

            var wehaveininv = Core.LocalPlayer.GetAmountItem(maktplc.InputId);
            var canproceed = 0;
            canproceed = wehaveininv < ship.Capacity() ? wehaveininv : ship.Capacity();

            Core.LocalPlayer.Inventory.Where(n => n.Id == maktplc.InputId).FirstOrDefault().Amount -= canproceed;
            Logger.Info(string.Format(Localization.DESTINATION_MARKETPLACE, ship.GetShipName()));
            Networking.AddTask(new Task.SendShipMarketplaceTask(ship.InstId, maktplc.Id, 1, canproceed));
            var locship = Core.LocalPlayer.Ships.Where(n => n.InstId == ship.InstId).First();
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
            var opst = Core.LocalPlayer.Outposts
                .FirstOrDefault(
                    n => !n.Done && n.Crew < n.RequiredCrew && !Core.Config.ignoreddestination.Any(b => b.Destination == ShipDestType.Outpost && b.DefId == n.DefId));
            if (Core.LocalPlayer.Sailors < ship.Sailors())
            {
                return false;
            }
            if (opst != null)
            {
                
                var can = opst.RequiredCrew - opst.Crew;
                var sending = 0;
                if (can > SendingHelper.GetSailors(ship))
                {
                    sending = SendingHelper.GetSailors(ship);
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
                var locked = SendingHelper.GetUnlockableOutposts();
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
                sending = next.Crew > ship.Sailors() ? ship.Sailors() : next.Crew;
                Logger.Info(string.Format(Localization.DESTINATION_OUTPOST, ship.GetShipName()));
                Networking.AddTask(new Task.OutpostSendShipTask(ship.InstId, next.DefId, sending));
                Core.LocalPlayer.Outposts.Add(
                    new Outpost
                        {
                            CargoOnTheWay = sending,
                            Crew = sending,
                            DefId = sending,
                            Done = false,
                            PlayerLevel = Core.LocalPlayer.Level,
                            RequiredCrew = next.Crew
                        });
                return true;
            }

            // KAAAAAAAAAAAAZOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO?
        }

        public static bool SendToUpgradable(Ship ship, string itemname)
        {
            var bestplace = SendingHelper.GetBestUpgPlace(itemname, ship.Sailors(), Core.Config.upgradablestrategy);
          
            if (bestplace == null || Core.LocalPlayer.Sailors < ship.Sailors())
            {
                return false;
            }
            var place = bestplace.Definition;
            var shipfull = ship.Definition;
            var lvls = place?.Levels.Level.FirstOrDefault(n => n.Id == bestplace.Level);

            if (shipfull?.SlotUsage < place?.Slots)
            {
                return false;
            }

            if (lvls != null)
            {
                var wecan = lvls.MaterialKoef * ship.Capacity();
                var remain = bestplace.Amount - bestplace.Progress;
                if (remain < wecan)
                {
                    wecan = remain;
                }

                Core.LocalPlayer.Sailors -= lvls.Sailors;
                
                bestplace.CargoOnTheWay += wecan;
                var shp = Core.LocalPlayer.Ships.Where(n => n.InstId == ship.InstId).First();
                shp.Sent = TimeUtils.GetEpochTime();
                shp.Loaded = 0;
                shp.Type = "upgradeable";
                shp.TargetId = bestplace.DefId;
                shp.TargetLevel = bestplace.Level;
                Logger.Info(
                    Localization.SHIPS_SENDING + LocalizationCache.GetNameFromLoc(
                        LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).NameLoc,
                        LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).Name));
                Networking.AddTask(new Task.SendShipUpgradeableTask(ship, bestplace, wecan));
                return true;
            }

            return false;
        }
        
        public static bool SendToWreck(Ship ship)
        {
            var wreck = Core.LocalPlayer.Wrecks.Where(n => n.Status == 0).FirstOrDefault();

            if (wreck != null)
            {
                if (wreck.Sailors < ship.Sailors())
                {
                    var shp = Core.LocalPlayer.Ships.Where(n => n.InstId == ship.InstId).First();
                    shp.Sent = TimeUtils.GetEpochTime();
                    shp.Loaded = 0;
                    shp.Type = "wreck";
                    shp.TargetId = wreck.InstId;
                    shp.TargetLevel = 0;
                    wreck.Status = 1;
                    Logger.Info(string.Format(Localization.DESTINATION_WRECK, ship.GetShipName()));
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
