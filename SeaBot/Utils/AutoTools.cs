// // SeaBotCore
// // Copyright (C) 2018 - 2019 Weespin
// // 
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU General Public License for more details.
// // 
// // You should have received a copy of the GNU General Public License
// // along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace SeaBotCore.Utils
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;

    #endregion

    public static class AutoTools
    {
        public static List<Item> GetEnabledMarketPlacePoints()
        {
            var list = new List<Item>();
            var marketplacepoints = GetUsableMarketplacePoints();
            foreach (var n in marketplacepoints)
            {
                if (Core.Config.marketitems.Contains(n.Id))
                {
                    list.Add(n);
                }
            }

            return list;
        }

        public static Dictionary<int, int> GetLocalProducionPerHour()
        {
            var ret = new Dictionary<int, int>();
            decimal fishprod = 0;
            foreach (var boat in Core.GlobalData.Boats)
            {
                var b = Definitions.BoatDef.Items.Item.FirstOrDefault(n => n.DefId == 1)?.Levels.Level
                    .FirstOrDefault(n => n.Id == Core.GlobalData.BoatLevel);
                if (b == null)
                {
                    return ret;
                }

                var turnsperhour = 60 / b.TurnTime * 60;
                fishprod += b.OutputAmount * turnsperhour;
            }

            // Fish = 3;
            ret.Add(3, (int)fishprod);
            foreach (var building in Core.GlobalData.Buildings)
            {
                var bdef = Definitions.BuildingDef.Items.Item.Where(n => n.DefId == building.DefId).FirstOrDefault();
                if (bdef?.Type == "factory")
                {
                    var level = bdef.Levels.Level.Where(n => n.Id == building.Level).FirstOrDefault();
                    foreach (var outputsOutput in level.ProdOutputs.ProdOutput)
                    {
                        if (outputsOutput.Time == 0)
                        {
                            continue;
                        }

                        var perhour = 60M / outputsOutput.Time;
                        var outperhour = perhour * outputsOutput.Amount;
                        if (ret.ContainsKey(outputsOutput.MaterialId))
                        {
                            ret[outputsOutput.MaterialId] += (int)outperhour;
                        }
                        else
                        {
                            ret.Add(outputsOutput.MaterialId, (int)outperhour);
                        }
                    }
                }
            }

            return ret;
        }

        public static List<Item> GetUsableMarketplacePoints()
        {
            return Core.GlobalData.Inventory.Where(
                    n => n.Amount > 0 && Definitions.MarketDef.Items.Item[1].Materials.Material
                             .Any(b => b.InputId == n.Id))
                .ToList();
        }

        public static Dictionary<int, int> NeededItemsForUpgrade()
        {
            var ret = new Dictionary<int, int>();

            // 1. Needed for upgrades!
            var locinv = new List<Item>(Core.GlobalData.Inventory.Count);
            locinv.AddRange(Core.GlobalData.Inventory.Select(item => new Item { Amount = item.Amount, Id = item.Id }));

            foreach (var building in Core.GlobalData.Buildings)
            {
                try
                {
                    // Next level
                    var nextlvlbuilding = Definitions.BuildingDef.Items.Item.Where(n => n.DefId == building.DefId)
                        .FirstOrDefault()?.Levels.Level.Where(n => n.Id == building.Level).FirstOrDefault();
                    if (nextlvlbuilding?.Materials.Material != null)
                    {
                        foreach (var mats in nextlvlbuilding?.Materials.Material)
                        {
                            if (locinv.Any(n => n.Id == mats.Id))
                            {
                                locinv.Where(n => n.Id == mats.Id).First().Amount -= mats.Amount;
                            }
                            else
                            {
                                locinv.Add(new Item { Id = mats.Id, Amount = mats.Amount * -1 });
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            var b = locinv.Where(n => n.Amount < 0).ToList();
            foreach (var item in b)
            {
                ret.Add(item.Id, item.Amount * -1);
            }

            return ret;
        }

        public static Dictionary<int, decimal> NeededItemsForUpgradePercentage()
        {
            var ret = new Dictionary<int, decimal>();

            // 1. Needed for upgrades!
            var locinv = new List<Item>();
            var makingph = GetLocalProducionPerHour();

            foreach (var building in Core.GlobalData.Buildings)
            {
             
                    // Next level
                    var nextlvlbuilding = Definitions.BuildingDef.Items.Item.Where(n => n.DefId == building.DefId)
                        .FirstOrDefault()?.Levels.Level.Where(n => n.Id == building.Level).FirstOrDefault();
                    if (nextlvlbuilding?.Materials.Material != null)
                    {
                        foreach (var mats in nextlvlbuilding?.Materials.Material.Where(n => n.Amount != 0))
                        {
                            if (locinv.Any(n => n.Id == mats.Id))
                            { 
                                locinv.Where(n => n.Id == mats.Id).First().Amount += mats.Amount;
                            }
                            else
                            {
                                var mat = new Item { Id = mats.Id, Amount = mats.Amount };
                                locinv.Add(mat);
                            }
                        }
                    }
            }

            foreach (var item in locinv)
            {
                if (makingph.ContainsKey(item.Id))
                {
                    
                    if (makingph[item.Id] != 0)
                    {
                        decimal koef = (decimal)item.Amount / makingph[item.Id];
                        if (koef == 0)
                        {
                            continue;
                        }

                        ret.Add(item.Id, 100M / koef);
                    }
                    else
                    {
                        ret.Add(item.Id, 100M);
                    }
                   
                }
            }

            return ret;
        }
    }
}