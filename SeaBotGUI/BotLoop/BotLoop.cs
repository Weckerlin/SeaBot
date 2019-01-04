// SeabotGUI
// Copyright (C) 2019 Weespin
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
using System.Windows.Forms;
using SeaBotCore;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Logger;
using SeaBotCore.Utils;

namespace SeaBotGUI.BotLoop
{
    public static class BotLoop
    {
        public static void UnloadShips()
        {
            // Logger.Info("Unloading first 3 ship");
            var TimeShips = new Dictionary<Ship, double>();
            foreach (var ship in Core.GlobalData.Ships)
            {
                if (ship.Type == "upgradeable" && ship.TargetId != 0 && ship.Activated != 0)
                {
                    var lvl = Defenitions.UpgrDef.Items.Item.First(n => n.DefId == ship.TargetId).Levels.Level
                        .First(n => n.Id == ship.TargetLevel);
                    if ((DateTime.UtcNow - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds >
                        lvl.TravelTime)
                    {
                        var idle = (DateTime.UtcNow - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds - lvl.TravelTime;
                        TimeShips.Add(ship, idle);
                    }
                }
            }

            var ordered = TimeShips.OrderByDescending(n => n.Value).Take(3);
            //Oldest ships
            var _deship = new List<Ship>();
            for (var i = 0; i < ordered.Select(kvp => kvp.Key).ToList().Count; i++)
            {
                var ship = ordered.Select(kvp => kvp.Key).ToList()[i];
//check if still in reis)
                //unload if at port
                if (ship.Type == "upgradeable" && ship.TargetId != 0 && ship.TargetLevel != 0)
                {
                    var lvl = Defenitions.UpgrDef.Items.Item.First(n => n.DefId == ship.TargetId).Levels.Level
                        .First(n => n.Id == ship.TargetLevel);
                    if ((DateTime.UtcNow - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds >
                        lvl.TravelTime)
                    {
                        long sailors = 0;
                        long capacity = 0;
                        // i hate event ships)
                        if (Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels == null)
                        {
                            capacity = Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).CapacityLevels
                                .Level
                                .Where(n => n.Id == ship.CapacityLevel).First().Capacity.Value;
                        }
                        else
                        {
                            capacity = Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels.Level
                                .Where(n => n.Id == ship.Level).First().Capacity;
                        }

                        if (Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).SailorsLevels == null)
                        {
                            sailors = (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels
                                .Level
                                .Where(n => n.Id == ship.Level).First().Sailors;
                        }
                        else
                        {
                            sailors = (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId)
                                .SailorsLevels.Level
                                .First(n => n.Id == ship.SailorsLevel).Sailors.Value;
                        }

                        Logger.Info(
                            "Unloading " + Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name);
                        Core.GlobalData.Upgradeables.Where(n => n.DefId == ship.TargetId).First().Progress +=
                            (int) lvl.Amount;


                        _deship.Add(ship);
                        Networking.AddTask(new Task.UnloadShipTask(ship.InstId.ToString(),
                            Core.GlobalData.Level.ToString(), Enums.EObject.upgradeable, capacity.ToString(),
                            ((int) lvl.MaterialKoef * capacity).ToString(), sailors.ToString(), lvl.Sailors.ToString(),
                            ship.TargetLevel.ToString(),
                            Core.GlobalData.Upgradeables.Where(n => n.DefId == ship.TargetId).First().Done.ToString(),
                            null, _deship.Where(n => n.DefId == ship.DefId).Count().ToString()));

                        Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().Sent = 0;
                        Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().Type = String.Empty;
                        Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().TargetId = 0;
                        Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().TargetLevel = 0;
                    }
                }
            }
        }

        public static void SendToUpgradable(bool profitbased)
        {
            //    Logger.Info("Sending first 3 ship");
            ////Example: Needed Gold, Profit only
            var TimeShips = new Dictionary<Ship, double>();
            foreach (var ship in Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0))
            {
                var lvl = Defenitions.UpgrDef.Items.Item.Where(n => n.DefId == ship.TargetId).First().Levels.Level
                    .Where(n => n.Id == ship.TargetLevel).First();
                if ((DateTime.UtcNow - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds >
                    lvl.TravelTime)
                {
                    var idle = (DateTime.UtcNow - TimeUtils.FromUnixTime(ship.Sent)).TotalSeconds - lvl.TravelTime;
                    TimeShips.Add(ship, idle);
                }
            }

            var ordered = TimeShips.OrderByDescending(n => n.Value).Take(3);
            foreach (var ord in ordered)
            {
                var ship = ord.Key;
                if (ship.TargetId != 0)
                {
                    continue;
                }

                var goldshit =
                    Defenitions.UpgrDef.Items.Item.Where(n => n.MaterialId == MaterialDB.GetItem("coins").DefId);
                var p = goldshit
                    .Where(shtItem =>
                        Core.GlobalData.Upgradeables.FirstOrDefault(n => n.DefId == shtItem.DefId && n.Amount != 0) !=
                        null)
                    .ToDictionary(shtItem => shtItem,
                        shtItem => Core.GlobalData.Upgradeables.First(n => n.DefId == shtItem.DefId));
                var best =
                    new Dictionary<UpgradeableDefenition.Item, decimal>();
                var sailorss = 0;
                long capacity = 0;
                // i hate event ships)
                if (Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels == null)
                {
                    capacity = Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).CapacityLevels.Level
                        .Where(n => n.Id == ship.CapacityLevel).First().Capacity.Value;
                }
                else
                {
                    capacity = Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels.Level
                        .Where(n => n.Id == ship.Level).First().Capacity;
                }

                if (Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).SailorsLevels == null)
                {
                    sailorss = (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Levels.Level
                        .Where(n => n.Id == ship.Level).First().Sailors;
                }
                else
                {
                    sailorss = (int) Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).SailorsLevels
                        .Level
                        .First(n => n.Id == ship.SailorsLevel).Sailors.Value;
                }

                foreach (var up in p)
                {
                    if (up.Key.Levels.Level.First(n => n.Id == up.Value.Level).Sailors > sailorss || sailorss == 0)
                    {
                        continue;
                    }

                    if ((DateTime.UtcNow - TimeUtils.FromUnixTime(up.Value.UpgradeTimeStarted)).TotalSeconds >
                        Defenitions.UpgrDef.Items.Item.Where(n => n.DefId == up.Value.DefId).First().RefreshTime)
                    {
                        Core.GlobalData.Upgradeables.Where(n => n.DefId == up.Value.DefId).First().UpgradeTimeStarted =
                            0;
                    }
                    else
                    {
                        continue;
                    }

                    if (Defenitions.UpgrDef.Items.Item.Where(n => n.DefId == up.Value.DefId).First().MaxLevel !=
                        ship.TargetLevel)
                    {
                        //var progress = Core.GlobalData.Upgradeables.Where(n => n.DefId == up.Value.DefId).First()
                        //    .Progress;
                        //var need = Defenitions.UpgrDef.Items.Item.Where(n => n.DefId == up.Value.DefId).First().Levels
                        //    .Level
                        //    .Where(n => n.Id == up.Value.Level+1).First().Amount;
                        //if (progress > need) 
                        //{
                        //    Core.GlobalData.Upgradeables.Where(n => n.DefId == up.Value.DefId).First().Level++;
                        //    Core.GlobalData.Upgradeables.Where(n => n.DefId == up.Value.DefId).First().UpgradeTimeStarted
                        //        = TimeUtils.GetEpochTime();
                        //    continue;
                        //}
                    }

                    if (profitbased)
                    {
                        var time = (decimal) (up.Key.Levels.Level.Where(n => n.Id == up.Value.Level).First()
                            .TravelTime);
                        var koef = up.Key.Levels.Level.Where(n => n.Id == up.Value.Level).First().MaterialKoef;
                        var timepercoin = koef / time;
                        best.Add(up.Key, timepercoin);
                    }
                    else
                    {
                        var sailors =
                            (decimal) (up.Key.Levels.Level.Where(n => n.Id == up.Value.Level).First().Sailors);
                        var koef = (decimal)
                            up.Key.Levels.Level.Where(n => n.Id == up.Value.Level).First().MaterialKoef;
                        var timepercoin = koef / sailors;
                        best.Add(up.Key, timepercoin);
                    }
                }

                var bestplace = best.OrderBy(n => n.Value).LastOrDefault();
                if (bestplace.Key == null)
                {
                    return;
                }

                var lvl = Core.GlobalData.Upgradeables.Where(n => n.DefId == bestplace.Key.DefId).First();
                var lvls = bestplace.Key.Levels.Level.Where(n => n.Id == lvl.Level).First();
                Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().Sent = TimeUtils.GetEpochTime();
                Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().Type = "upgradeable";
                Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().TargetId = (int) bestplace.Key.DefId;
                Core.GlobalData.Ships.Where(n => n.InstId == ship.InstId).First().TargetLevel = lvl.Level;
                Logger.Info("Sending " + Defenitions.ShipDef.Items.Item.First(n => n.DefId == ship.DefId).Name);
                Networking.AddTask(new Task.SendShipUpgradeableTask(ship.InstId.ToString(),
                    bestplace.Key.DefId.ToString(), lvls.Amount.ToString(), lvls.MaterialKoef.ToString(),
                    lvls.Sailors.ToString(), (lvls.MaterialKoef * capacity).ToString(),
                    Core.GlobalData.Level.ToString()));
            }

            //var allupgradables = Core.GlobalData.Upgradeables.Where(n=>n.DefId==)
        }

        public static void AutoUpgrade(bool onlyfactory)
        {
            foreach (var data in Core.GlobalData.Buildings)
                if (data.UpgStart == 0 && data.ProdStart == 0)
                {
                    var defined =
                        Defenitions.BuildingDef.Items.Item.FirstOrDefault(n =>
                            n.DefId == data.DefId);
                    var neededmats = defined?.Levels.Level.FirstOrDefault(n => n.Id == data.Level + 1);

                    if (defined != null && (defined.Type != "factory" && onlyfactory))
                    {
                        continue;
                    }

                    if (neededmats != null)
                    {
                        var ok = true;

                        foreach (var neededmat in neededmats.Materials.Material)
                            if (Core.GlobalData.Inventory
                                    .FirstOrDefault(n => n.Id == neededmat.Id) != null)
                            {
                                var m = Core.GlobalData.Inventory
                                    .First(n => n.Id == neededmat.Id);
                                if (neededmat.Amount > m.Amount) ok = false;
                            }
                            else
                            {
                                ok = false;
                                break;
                            }

                        if (ok)
                        {
                            if (neededmats.ReqId != 0)
                            {
                                var def = Core.GlobalData.Buildings
                                    .FirstOrDefault(n => n.DefId == neededmats.ReqId);
                                if (def != null)
                                {
                                    ok = def.Level >= neededmats.ReqLevel;
                                }
                                else
                                {
                                    ok = false;
                                }
                            }

                            if (neededmats.PlayerLevel > Core.GlobalData.Level)
                            {
                                ok = false;
                            }
                        }

                        if (ok)
                        {
                            foreach (var neededmat in neededmats.Materials.Material)
                            {
                                var m = Core.GlobalData.Inventory
                                    .First(n => n.Id == neededmat.Id);
                                m.Amount -= (int) neededmat.Amount;
                            }

                            Logger.Info(
                                $"Started upgrading {defined.Name}");
                            Networking.AddTask(new Task.StartBuildingUpgradeTask(data.InstId.ToString(),
                                data.ProdId.ToString(), data.Level, data.UpgType.ToString(), data.DefId.ToString(),
                                data.GridX.ToString(), data.GridY.ToString()));
                            data.UpgStart = 0;
                        }
                    }
                }
        }

        public static void FinishUpgrade()
        {
            foreach (var data in Core.GlobalData.Buildings)
                if (data.UpgStart != 0 && data.ProdStart == 0)
                {
                    var defined = Defenitions.BuildingDef.Items.Item.FirstOrDefault(n => n.DefId == data.DefId);
                    var upgrade = defined.Levels.Level.FirstOrDefault(n => n.Id == data.Level + 1);
                    if (upgrade != null)
                        if ((DateTime.UtcNow - TimeUtils.FromUnixTime(data.UpgStart)).TotalSeconds >
                            upgrade.UpgradeTime)
                        {
                            Logger.Info(
                                $"Finishing upgrading {defined.Name}");
                            Networking.AddTask(new Task.FinishBuildingUpgradeTask(data.InstId.ToString()));
                            data.UpgStart = 0;
                            data.Level++;
                        }
                }
        }

        public static void ProduceFactories(int num_ironlimit, int num_stonelimit, int num_woodlimit)
        {
            foreach (var data in Core.GlobalData.Buildings)
                if (data.UpgStart == 0 && data.ProdStart == 0)
                {
                    var def = Defenitions.BuildingDef.Items.Item.First(n => n.DefId == data.DefId);
                    if (def.Type != "factory") continue;

                    //lets start?
                    //DO WE HAVE ENOUGH RESOURCES
                    var needed = def.Levels.Level.FirstOrDefault(n => n.Id == data.Level);
                    if (needed == null)
                    {
                        continue;
                    }

                    var ok = true;
                    var inputs = needed.ProdOutputs.ProdOutput;
                    var Dict = new Dictionary<long, long>();
                    foreach (var input in inputs)
                    {
                        foreach (var inp in input.Inputs.Input)
                        {
                            var ourmat = Core.GlobalData.Inventory.FirstOrDefault(n => n.Id == inp.Id);
                            if (ourmat == null)
                            {
                                ok = false;
                            }
                            else
                            {
                                if (ourmat.Amount >= inp.Amount)
                                {
                                    if (Dict.ContainsKey(inp.Id))
                                    {
                                        Dict[inp.Id] += inp.Amount;
                                    }
                                    else
                                    {
                                        Dict.Add(inp.Id, inp.Amount);
                                    }
                                }
                                else
                                {
                                    ok = false;
                                }
                            }
                        }

                        if (MaterialDB.GetItem(input.MaterialId).Name == "wood")
                        {
                            var amount =
                                Core.GlobalData.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);

                            if (amount != null && amount.Amount > num_woodlimit)
                            {
                                if (num_woodlimit != 0)
                                {
                                    ok = false;
                                }
                            }
                            else
                            {
                                ok = false;
                            }
                        }

                        if (MaterialDB.GetItem(input.MaterialId).Name == "iron")
                        {
                            var amount =
                                Core.GlobalData.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);
                            if (amount != null && amount.Amount > num_ironlimit)
                            {
                                if (num_ironlimit != 0)
                                {
                                    ok = false;
                                }
                            }
                            else
                            {
                                ok = false;
                            }
                        }

                        if (MaterialDB.GetItem(input.MaterialId).Name == "stone")
                        {
                            var amount =
                                Core.GlobalData.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);
                            if (amount != null && amount.Amount > num_stonelimit)
                            {
                                if (num_stonelimit != 0)
                                {
                                    ok = false;
                                }
                            }
                            else
                            {
                                ok = false;
                            }
                        }
                    }

                    if (ok)
                    {
                        foreach (var inp in Dict)
                        {
                            Core.GlobalData.Inventory.First(n => n.Id == inp.Key).Amount -= (int) inp.Value;
                        }

                        Logger.Info(
                            $"Started producing {MaterialDB.GetItem(needed.ProdOutputs.ProdOutput[0].MaterialId).Name}");
                        Networking.AddTask(new Task.StartBuildingProducingTask(data.InstId.ToString(),
                            data.ProdId.ToString()));
                        data.ProdStart = TimeUtils.GetEpochTime();
                    }
                }
        }

        public static void CollectFish()
        {
            var totalfish = 0;
            foreach (var boat in Core.GlobalData.Boats)
            {
                var started = TimeUtils.FromUnixTime(boat.ProdStart);
                var b = Defenitions.BoatDef.Items.Item.First(n => n.DefId == 1).Levels.Level
                    .First(n => n.Id == Core.GlobalData.BoatLevel);
                var turns = Math.Round((DateTime.UtcNow - started).TotalSeconds / b.TurnTime);
                if (turns > 5)
                {
                    totalfish += (int) (b.OutputAmount * turns);
                    Networking.AddTask(new Task.TakeFish(boat));
                }
            }

            if (totalfish > 0) Logger.Info($"Collecting {totalfish} fish");
        }

        public static void CollectBarrel()
        {
            var bar = BarrelController.GetNextBarrel(Defenitions.BarrelDef.Items.Item
                .Where(n => n.DefId == 21).First());
            if (bar.Definition.Id != 0)
            {
                Logger.Info(
                    $"Barrel! Collecting {bar.Amount} {MaterialDB.GetItem(bar.Definition.Id).Name}");
                if (Core.GlobalData.Inventory.Where(n => n.Id == bar.Definition.Id).FirstOrDefault() != null)
                {
                    Core.GlobalData.Inventory.Where(n => n.Id == bar.Definition.Id).First().Amount += bar.Amount;
                }
                else
                {
                    Core.GlobalData.Inventory.Add(new Item {Amount = bar.Amount, Id = (int) bar.Definition.Id});
                }
            }
            else
            {
                Logger.Info(
                    $"Barrel! Collecting {bar.Amount} sailors!");
            }

            Networking.AddTask(new Task.ConfirmBarrelTask("21", bar.get_type(), bar.Amount.ToString(),
                bar.Definition.Id.ToString(), Core.GlobalData.Level.ToString()));
        }

        public static void CollectMaterials()
        {
            foreach (var data in Core.GlobalData.Buildings)
            {
                if (data.UpgStart == 0 && data.ProdStart != 0)
                {
                    var def = Defenitions.BuildingDef.Items.Item.First(n => n.DefId == data.DefId);
                    if (def.Type != "factory") continue;

                    var defs = def.Levels.Level.First(n => n.Id == data.Level);
                    var started = TimeUtils.FromUnixTime(data.ProdStart);
                    //var prodtime = defs.ProdOutputs.ProdOutput[data.ProdId - 1]; //todo add this!
                    if ((DateTime.UtcNow - started).TotalSeconds > defs.ProdOutputs.ProdOutput[0].Time)
                    {
                        Logger.Info(
                            $"Сollecting {defs.ProdOutputs.ProdOutput[0].Amount} {MaterialDB.GetItem(defs.ProdOutputs.ProdOutput[0].MaterialId).Name}");

                        Networking.AddTask(new Task.FinishBuildingProducingTask(data.InstId.ToString()));

                        data.ProdStart = 0;
                    }
                }
            }
        }
    }
}