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
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    public static class Buildings
    {
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

                            Logger.Logger.Info(
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
                            Logger.Logger.Info(
                                $"Finishing upgrading {defined.Name}");
                            Networking.AddTask(new Task.FinishBuildingUpgradeTask(data.InstId.ToString()));
                            data.UpgStart = 0;
                            data.Level++;
                        }
                }
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
                        Logger.Logger.Info(
                            $"Сollecting {defs.ProdOutputs.ProdOutput[0].Amount} {MaterialDB.GetItem(defs.ProdOutputs.ProdOutput[0].MaterialId).Name}");

                        Networking.AddTask(new Task.FinishBuildingProducingTask(data.InstId.ToString()));

                        data.ProdStart = 0;
                    }
                }
            }
        }
    }
}