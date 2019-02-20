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

    using System.Linq;

    using SeaBotCore.Cache;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    #endregion

    public static class Buildings
    {
        public static void AutoUpgrade(bool onlyfactory)
        {
            foreach (var data in Core.LocalPlayer.Buildings)
            {
                if (data.UpgStart == 0 && data.ProdStart == 0)
                {
                    var defined = Definitions.BuildingDef.Buildings.Item.FirstOrDefault(n => n.DefId == data.DefId);
                    var neededmats = defined?.BuildingLevels.Level.FirstOrDefault(n => n.Id == data.Level + 1);

                    if (defined != null && defined.Type != "factory" && onlyfactory)
                    {
                        continue;
                    }

                    if (neededmats != null)
                    {
                        var ok = true;

                        foreach (var neededmat in neededmats.Materials.Material)
                        {
                            if (Core.LocalPlayer.Inventory.FirstOrDefault(n => n.Id == neededmat.Id) != null)
                            {
                                var m = Core.LocalPlayer.Inventory.First(n => n.Id == neededmat.Id);
                                if (neededmat.Amount > m.Amount)
                                {
                                    ok = false;
                                }
                            }
                            else
                            {
                                ok = false;
                                break;
                            }
                        }

                        if (ok)
                        {
                            if (neededmats.ReqId != 0)
                            {
                                var def = Core.LocalPlayer.Buildings.FirstOrDefault(n => n.DefId == neededmats.ReqId);
                                if (def != null)
                                {
                                    ok = def.Level >= neededmats.ReqLevel;
                                }
                                else
                                {
                                    ok = false;
                                }
                            }

                            if (neededmats.PlayerLevel > Core.LocalPlayer.Level)
                            {
                                ok = false;
                            }
                        }

                        if (ok)
                        {
                            foreach (var neededmat in neededmats.Materials.Material)
                            {
                                var m = Core.LocalPlayer.Inventory.First(n => n.Id == neededmat.Id);
                                m.Amount -= neededmat.Amount;
                            }

                            Logger.Info(
                                string.Format(
                                    Localization.BUILDINGS_STARTED_UPG,
                                    LocalizationCache.GetNameFromLoc(defined.NameLoc, defined.Name)));
                            Networking.AddTask(
                                new Task.StartBuildingUpgradeTask(
                                    data.InstId,
                                    data.ProdId,
                                    data.Level,
                                    data.UpgType.ToString(),
                                    data.DefId,
                                    data.GridX,
                                    data.GridY));
                            data.UpgStart = TimeUtils.GetEpochTime();
                        }
                    }
                }
            }
        }

        public static void CollectMaterials()
        {
            foreach (var data in Core.LocalPlayer.Buildings)
            {
                if (data.UpgStart == 0 && data.ProdStart != 0)
                {
                    var def = Definitions.BuildingDef.Buildings.Item.First(n => n.DefId == data.DefId);
                    if (def.Type != "factory")
                    {
                        continue;
                    }

                    var defs = def.BuildingLevels.Level.First(n => n.Id == data.Level);
                    var started = TimeUtils.FromUnixTime(data.ProdStart);

                    // var prodtime = defs.ProdOutputs.ProdOutput[data.ProdId - 1]; //todo add this!
                    if ((TimeUtils.FixedUTCTime - started).TotalSeconds > defs.ProdOutputs.ProdOutput[0].Time)
                    {
                        Logger.Info(
                            string.Format(
                                Localization.BUILDINGS_COLLECTING,
                                defs.ProdOutputs.ProdOutput[0].Amount,
                                MaterialDB.GetLocalizedName(defs.ProdOutputs.ProdOutput[0].MaterialId)));

                        Networking.AddTask(new Task.FinishBuildingProducingTask(data.InstId));

                        data.ProdStart = 0;
                    }
                }
            }
        }

        public static void FinishUpgrade()
        {
            foreach (var data in Core.LocalPlayer.Buildings)
            {
                if (data.UpgStart != 0 && data.ProdStart == 0)
                {
                    var defined = Definitions.BuildingDef.Buildings.Item.FirstOrDefault(n => n.DefId == data.DefId);
                    var upgrade = defined.BuildingLevels.Level.FirstOrDefault(n => n.Id == data.Level + 1);
                    if (upgrade != null)
                    {
                        if ((TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime(data.UpgStart)).TotalSeconds
                            > upgrade.UpgradeTime)
                        {
                            Logger.Info(
                                string.Format(
                                    Localization.BUILDINGS_FINISHED_UPG,
                                    LocalizationCache.GetNameFromLoc(defined.NameLoc, defined.Name)));
                            Networking.AddTask(new Task.FinishBuildingUpgradeTask(data.InstId));
                            data.UpgStart = 0;
                            data.Level++;
                        }
                    }
                }
            }
        }
    }
}