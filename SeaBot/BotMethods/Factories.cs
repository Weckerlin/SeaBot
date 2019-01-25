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

using System.Collections.Generic;
using System.Linq;
using SeaBotCore.Data.Definitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Localizaion;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    internal class Factories
    {
        public static void ProduceFactories(int num_ironlimit, int num_stonelimit, int num_woodlimit)
        {
            foreach (var data in Core.GlobalData.Buildings)
            {
                if (data.UpgStart == 0 && data.ProdStart == 0)
                {
                    var def = Definitions.BuildingDef.Items.Item.First(n => n.DefId == data.DefId);
                    if (def.Type != "factory")
                    {
                        continue;
                    }

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
                            if (amount != null && num_woodlimit != 0)
                            {
                                if (amount.Amount > num_woodlimit)
                                {
                                    ok = false;
                                }
                            }
                        }

                        if (MaterialDB.GetItem(input.MaterialId).Name == "iron")
                        {
                            var amount =
                                Core.GlobalData.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);
                            if (amount != null && num_ironlimit != 0)
                            {
                                if (amount.Amount > num_ironlimit)
                                {
                                    ok = false;
                                }
                            }
                        }

                        if (MaterialDB.GetItem(input.MaterialId).Name == "stone")
                        {
                            var amount =
                                Core.GlobalData.Inventory.FirstOrDefault(n => n.Id == input.MaterialId);
                            if (amount != null && num_stonelimit != 0)
                            {
                                if (amount.Amount > num_stonelimit)
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
                            Core.GlobalData.Inventory.First(n => n.Id == inp.Key).Amount -= (int) inp.Value;
                        }

                        Logger.Logger.Info(
                            string.Format(Localization.FACTORIES_STARTED_PROD,
                                MaterialDB.GetLocalizedName(needed.ProdOutputs.ProdOutput[0].MaterialId)));
                        Networking.AddTask(new Task.StartBuildingProducingTask(data.InstId,
                            data.ProdId));
                        data.ProdStart = TimeUtils.GetEpochTime();
                    }
                }
            }
        }
    }
}