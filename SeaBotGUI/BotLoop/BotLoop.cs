using System;
using System.Linq;
using SeaBotCore;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Logger;
using SeaBotCore.Utils;

namespace SeaBotGUI.BotLoop
{
    public static class BotLoop
    {
        public static void AutoUpgrade()
        {
            foreach (var data in Core.GolobalData.Buildings)
                if (data.UpgStart == 0 && data.ProdStart == 0)
                {
                    var defined =
                        Defenitions.BuildingDef.Items.Item.FirstOrDefault(n =>
                            n.DefId == data.DefId);
                    var neededmats = defined?.Levels.Level.FirstOrDefault(n => n.Id == data.Level + 1);
                    if (neededmats != null)
                    {
                        var ok = true;
                        foreach (var neededmat in neededmats.Materials.Material)
                            if (Core.GolobalData.Inventory
                                    .FirstOrDefault(n => n.Id == neededmat.Id) != null)
                            {
                                var m = Core.GolobalData.Inventory
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
                            foreach (var neededmat in neededmats.Materials.Material)
                            {
                                var m = Core.GolobalData.Inventory
                                    .First(n => n.Id == neededmat.Id);
                                m.Amount -= (int)neededmat.Amount;
                               
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
            foreach (var data in Core.GolobalData.Buildings)
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
            foreach (var data in Core.GolobalData.Buildings)
                if (data.UpgStart == 0 && data.ProdStart == 0)
                {
                    var def = Defenitions.BuildingDef.Items.Item.First(n => n.DefId == data.DefId);
                    if (def.Type != "factory") continue;

                    //lets start?
                    //DO WE HAVE ENOUGH RESOURCES
                    var needed = def.Levels.Level.First(n => n.Id == data.Level);
                    var input = needed.ProdOutputs.ProdOutput[0].Inputs.Input;
                    var can = false;
                    foreach (var material in input)
                        if (Core.GolobalData.Inventory.Any(n => n.Id == material.Id))
                        {
                            var mat = Core.GolobalData.Inventory
                                .First(n => n.Id == material.Id);
                            if (mat.Amount > material.Amount)
                            {
                                can = true;
                                mat.Amount -= (int) material.Amount;
                            }
                            else
                            {
                                can = false;
                            }
                        }

                    if (can)
                    {
                        //
                        var output = MaterialDB.GetItem(needed.ProdOutputs.ProdOutput[0].MaterialId);

                        if (output.Name == "wood")
                        {
                            var amount =
                                Core.GolobalData.Inventory.Where(n =>
                                    n.Id == output.DefId).First();
                            if (amount.Amount > num_woodlimit)
                            {
                                if (num_woodlimit == 0)
                                {
                                    can = true;
                                    break;
                                }

                                can = false;
                            }
                        }

                        if (output.Name == "iron")
                        {
                          var  amount =
                                Core.GolobalData.Inventory.Where(n =>
                                    n.Id == output.DefId).First();
                            if (amount.Amount > num_ironlimit)
                            {
                                if (num_ironlimit == 0)
                                {
                                    can = true;
                                    break;
                                }

                                can = false;
                            }

                        }

                        if (output.Name == "stone")
                        {
                            var amount =
                                Core.GolobalData.Inventory.Where(n =>
                                    n.Id == output.DefId).First();
                            if (amount.Amount > num_stonelimit)
                            {
                                if (num_stonelimit == 0)
                                {
                                    can = true;
                                    break;
                                }

                                can = false;
                            }
                        }
                        
                           
                    }

                    if (can)
                    {
                        Logger.Info(
                            $"Started producing {MaterialDB.GetItem(needed.ProdOutputs.ProdOutput[0].MaterialId).Name}");
                        Networking.AddTask(new Task.StartBuildingProductionTask(data.InstId.ToString(),
                            data.ProdId.ToString()));
                        data.ProdStart = TimeUtils.GetEpochTime();
                    }
                }
        }

        public static void CollectFish()
        {
            var totalfish = 0;
            foreach (var boat in Core.GolobalData.Boats)
            {
                var started = TimeUtils.FromUnixTime(boat.ProdStart);
                var b = Defenitions.BoatDef.Items.Item.First(n => n.DefId == 1).Levels.Level
                    .First(n => n.Id == Core.GolobalData.BoatLevel);
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
            }

            Networking.AddTask(new Task.ConfirmBarrelTask("21", bar.get_type(), bar.Amount.ToString(),
                bar.Definition.Id.ToString(), Core.GolobalData.Level.ToString()));
        }

        public static void CollectMaterials()
        {
            foreach (var data in Core.GolobalData.Buildings)
            {
                if (data.UpgStart == 0 && data.ProdStart != 0)
                {
                    var def = Defenitions.BuildingDef.Items.Item.First(n => n.DefId == data.DefId);
                    if (def.Type != "factory") continue;

                    var defs = def.Levels.Level.First(n => n.Id == data.Level);
                    var started = TimeUtils.FromUnixTime(data.ProdStart);
                    if ((DateTime.UtcNow - started).TotalSeconds > defs.ProdOutputs.ProdOutput[0].Time)
                    {
                        Logger.Info(
                            $"Сollecting {defs.ProdOutputs.ProdOutput[0].Amount} {MaterialDB.GetItem(defs.ProdOutputs.ProdOutput[0].MaterialId).Name}");

                        Networking.AddTask(new Task.FinishBuildingProductionTask(data.InstId.ToString()));

                        data.ProdStart = 0;
                    }
                }
            }
        }
    }
}