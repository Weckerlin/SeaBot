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

    using System.Collections.Generic;
    using System.Linq;

    using SeaBotCore.Config;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    #endregion

    internal class Factories
    {
        public static void ProduceFactories()
        {
            foreach (var data in Core.LocalPlayer.Buildings)
            {
                if (data.UpgStart == 0 && data.ProdStart == 0)
                {
                    var def = LocalDefinitions.Buildings.First(n => n.DefId == data.DefId);
                    if (def.Type != "factory")
                    {
                        continue;
                    }

                    var needed = def.BuildingLevels.Level.FirstOrDefault(n => n.Id == data.Level);
                    if (needed == null)
                    {
                        continue;
                    }

                    if (data.DefId == 11)
                    {
                        // This is a machinery
                        if (Core.Config.autothresholdworkshop)
                        {
                            // Mechanical part
                            if (Core.LocalPlayer.GetAmountItem(33) < Core.Config.thresholdmechanical)
                            {
                                var prodid = 1;
                                var enough = true;
                                var mat = needed.ProdOutputs.ProdOutput.Where(n => n.Id == 1).FirstOrDefault();
                                if (mat == null)
                                {
                                    continue;
                                }

                                foreach (var prod in mat.Inputs.Input)
                                {
                                    if (Core.LocalPlayer.GetAmountItem(prod.Id) < prod.Amount)
                                    {
                                        enough = false;
                                    }
                                }

                                if (enough)
                                {
                                    foreach (var inp in mat.Inputs.Input)
                                    {
                                        Core.LocalPlayer.Inventory.First(n => n.Id == inp.Id).Amount -= inp.Amount;
                                    }

                                    Logger.Info(
                                        string.Format(
                                            Localization.FACTORIES_STARTED_PROD,
                                            MaterialDB.GetLocalizedName(mat.MaterialId)));
                                    Networking.AddTask(new Task.StartBuildingProducingTask(data.InstId, prodid));
                                    data.ProdStart = TimeUtils.GetEpochTime();

                                    continue;
                                }

                                // This will work
                            }

                            // Fuel
                            if (Core.LocalPlayer.GetAmountItem(180) < Core.Config.thresholdfuel)
                            {
                                var prodid = 2;
                                var enough = true;
                                var mat = needed.ProdOutputs.ProdOutput.Where(n => n.Id == prodid).FirstOrDefault();
                                if (mat == null)
                                {
                                    continue;
                                }

                                foreach (var prod in mat.Inputs.Input)
                                {
                                    if (Core.LocalPlayer.GetAmountItem(prod.Id) < prod.Amount)
                                    {
                                        enough = false;
                                    }
                                }

                                if (enough)
                                {
                                    foreach (var inp in mat.Inputs.Input)
                                    {
                                        Core.LocalPlayer.Inventory.First(n => n.Id == inp.Id).Amount -= inp.Amount;
                                    }

                                    Logger.Info(
                                        string.Format(
                                            Localization.FACTORIES_STARTED_PROD,
                                            MaterialDB.GetLocalizedName(mat.MaterialId)));
                                    Networking.AddTask(new Task.StartBuildingProducingTask(data.InstId, prodid));
                                    data.ProdStart = TimeUtils.GetEpochTime();
                                    continue;
                                }
                            }

                            // todo CHECK FOR LEVEL!!!
                            // Concrete
                            if (data.Level >= 5)
                            {
                                if (Core.LocalPlayer.GetAmountItem(182) < Core.Config.thresholdconcrete)
                                {
                                    var prodid = 3;

                                    var enough = true;
                                    var mat = needed.ProdOutputs.ProdOutput.Where(n => n.Id == prodid).FirstOrDefault();
                                    if (mat == null)
                                    {
                                        continue;
                                    }

                                    foreach (var prod in mat.Inputs.Input)
                                    {
                                        if (Core.LocalPlayer.GetAmountItem(prod.Id) < prod.Amount)
                                        {
                                            enough = false;
                                        }
                                    }

                                    if (enough)
                                    {
                                        foreach (var inp in mat.Inputs.Input)
                                        {
                                            Core.LocalPlayer.Inventory.First(n => n.Id == inp.Id).Amount -= inp.Amount;
                                        }

                                        Logger.Info(
                                            string.Format(
                                                Localization.FACTORIES_STARTED_PROD,
                                                MaterialDB.GetLocalizedName(mat.MaterialId)));
                                        Networking.AddTask(new Task.StartBuildingProducingTask(data.InstId, prodid));
                                        data.ProdStart = TimeUtils.GetEpochTime();
                                    }
                                }
                            }
                        }
                        else
                        {
                            var prodid = 0;
                            var matid = 0;
                            var thresholdconc = 0;
                            switch (Core.Config.workshoptype)
                            {
                                case WorkshopType.Concrete:
                                    {
                                        matid = 182;
                                        prodid = 3;
                                        thresholdconc = Core.Config.thresholdconcrete;
                                    }

                                    break;
                                case WorkshopType.Fuel:
                                    {
                                        matid = 180;
                                        prodid = 2;
                                        thresholdconc = Core.Config.thresholdfuel;
                                    }

                                    break;
                                case WorkshopType.MechanicalPart:
                                    {
                                        matid = 33;
                                        prodid = 1;
                                        thresholdconc = Core.Config.thresholdmechanical;
                                        break;
                                    }
                            }

                            if (prodid == 3 && data.Level < 5)
                            {
                                continue;
                            }

                            if (Core.LocalPlayer.GetAmountItem(matid) < thresholdconc || thresholdconc == 0)
                            {
                                var enough = true;
                                var mat = needed.ProdOutputs.ProdOutput.Where(n => n.Id == prodid).FirstOrDefault();
                                if (mat == null)
                                {
                                    continue;
                                }

                                foreach (var prod in mat.Inputs.Input)
                                {
                                    if (Core.LocalPlayer.GetAmountItem(prod.Id) < prod.Amount)
                                    {
                                        enough = false;
                                    }
                                }

                                if (enough)
                                {
                                    foreach (var inp in mat.Inputs.Input)
                                    {
                                        Core.LocalPlayer.Inventory.First(n => n.Id == inp.Id).Amount -= inp.Amount;
                                    }

                                    Logger.Info(
                                        string.Format(
                                            Localization.FACTORIES_STARTED_PROD,
                                            MaterialDB.GetLocalizedName(mat.MaterialId)));
                                    Networking.AddTask(new Task.StartBuildingProducingTask(data.InstId, prodid));
                                    data.ProdStart = TimeUtils.GetEpochTime();
                                }
                            }
                        }

                        // Don't need another shit
                        continue;
                    }

                    // lets start?
                    // DO WE HAVE ENOUGH RESOURCES
                    var ok = true;
                    var inputs = needed.ProdOutputs.ProdOutput;
                    var Dict = new Dictionary<long, long>();
                    foreach (var input in inputs)
                    {
                        foreach (var inp in input.Inputs.Input)
                        {
                            var ourmat = Core.LocalPlayer.Inventory.FirstOrDefault(n => n.Id == inp.Id);
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
                            var amount = Core.LocalPlayer.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);
                            if (amount != null && Core.Config.woodlimit != 0)
                            {
                                if (amount.Amount > Core.Config.woodlimit)
                                {
                                    ok = false;
                                }
                            }
                        }

                        if (MaterialDB.GetItem(input.MaterialId).Name == "iron")
                        {
                            var amount = Core.LocalPlayer.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);
                            if (amount != null && Core.Config.ironlimit != 0)
                            {
                                if (amount.Amount > Core.Config.ironlimit)
                                {
                                    ok = false;
                                }
                            }
                        }

                        if (MaterialDB.GetItem(input.MaterialId).Name == "stone")
                        {
                            var amount = Core.LocalPlayer.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);
                            if (amount != null && Core.Config.stonelimit != 0)
                            {
                                if (amount.Amount > Core.Config.stonelimit)
                                {
                                    ok = false;
                                }
                            }
                        }
                    }

                    if (ok)
                    {
                        foreach (var inp in Dict)
                        {
                            Core.LocalPlayer.Inventory.First(n => n.Id == inp.Key).Amount -= (int)inp.Value;
                        }

                        Logger.Info(
                            string.Format(
                                Localization.FACTORIES_STARTED_PROD,
                                MaterialDB.GetLocalizedName(needed.ProdOutputs.ProdOutput[0].MaterialId)));
                        Networking.AddTask(new Task.StartBuildingProducingTask(data.InstId, data.ProdId));
                        data.ProdStart = TimeUtils.GetEpochTime();
                    }
                }
            }
        }
    }
}