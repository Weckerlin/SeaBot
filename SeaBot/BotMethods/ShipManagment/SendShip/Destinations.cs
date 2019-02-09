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
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    public static class Destinations
    {
        public static bool SendToContractor(Ship ship)
        {
            var statiopst = new List<Data.Contractor>();
            var genopst = new List<Data.Contractor>();
            foreach (var contractor in Core.GlobalData.Contracts)
            {
                if (contractor.Done == 0)
                {
                    var def = Definitions.ConDef.Items.Item.Where(c => contractor.DefId == c.DefId).FirstOrDefault();
                    var quest = def.Quests.Quest.Where(q => contractor.QuestId == q.Id).FirstOrDefault();
                    if (quest == null)
                    {
                        continue;
                    }
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

            foreach (var opst in statiopst.OrderBy(n => n.QuestId))
            {
                var def = Definitions.ConDef.Items.Item.Where(c => opst.DefId == c.DefId).FirstOrDefault();
                var quest = def.Quests.Quest.Where(q => opst.QuestId == q.Id).FirstOrDefault();
                if (quest == null)
                {
                    continue;
                }
                var already = opst.Progress;
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
                lship.MaterialId = quest.ObjectiveDefId;
                lship.TargetLevel = quest.Id;
                return true;
            }

            foreach (var opst in genopst.OrderBy(n => n.QuestId))
            {
                var def = Definitions.ConDef.Items.Item.Where(c => opst.DefId == c.DefId).FirstOrDefault();
                var quest = def.Quests.Quest.Where(q => opst.QuestId == q.Id).FirstOrDefault();
                if (quest == null)
                {
                    continue;
                }
                var already = opst.Progress;
                var exists = (int)quest.InputAmount() - opst.Progress;
                if (exists <= 0)
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
                lship.Sent = TimeUtils.GetEpochTime();
                lship.Loaded = 0;
                lship.Type = "contractor";
                lship.TargetId = opst.DefId;
                lship.MaterialId = quest.ObjectiveDefId;
                lship.TargetLevel = quest.Id;
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
            var bestplace = SendingHelper.GetBestUpgPlace(itemname, ship.Sailors(), !Core.Config.autoshipprofit);

            if (bestplace == null || Core.GlobalData.Sailors < ship.Sailors())
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
                var wecan = lvls.MaterialKoef * ship.Capacity();
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
