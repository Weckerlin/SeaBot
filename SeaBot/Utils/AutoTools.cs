using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SeaBotCore.Data;
using SeaBotCore.Data.Definitions;
using SeaBotCore.Data.Materials;

namespace SeaBotCore.Utils
{
   public static class AutoTools
   {
       public static List<Item> GetUsableMarketplacePoints()
       {
           return Core.GlobalData.Inventory.Where(n =>
                   n.Amount > 0 && Definitions.MarketDef.Items.Item[1].Materials.Material.Any(b => b.InputId == n.Id))
               .ToList();
       }

       public static List<Item> GetEnabledMarketPlacePoints()
       {
           List<Item> list = new List<Item>();
           var marketplacepoints = GetUsableMarketplacePoints();
           foreach (var n in marketplacepoints)
           {
               if (Core.Config.marketitems.Contains(n.Id)) list.Add(n);
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

               var turnsperhour = (60/b.TurnTime)*60;
              fishprod+=b.OutputAmount * turnsperhour;
                
           }
           //Fish = 3;
           ret.Add(3,(int)fishprod);
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
                           ret.Add(outputsOutput.MaterialId,(int)outperhour);
                       }
                   }
               }
           }
           
           return ret;
       }
       public static Dictionary<int, int> NeededItemsForUpgrade()
       {
           var ret = new Dictionary<int, int>();
           //1. Needed for upgrades!
           var locinv = new List<Item>(Core.GlobalData.Inventory.Count);
           locinv.AddRange(Core.GlobalData.Inventory.Select(item => new Item() {Amount = item.Amount, Id = item.Id}));

           foreach (var building in Core.GlobalData.Buildings)
           {
               try
               {
                 
                    //Next level
                    var nextlvlbuilding = Definitions.BuildingDef.Items.Item.Where(n => n.DefId == building.DefId)
                        .FirstOrDefault()?.Levels.Level.Where(n=>n.Id==building.Level).FirstOrDefault();
                    if (nextlvlbuilding?.Materials.Material != null)
                    {
                        foreach (var mats in nextlvlbuilding?.Materials.Material)
                        {
                            if (locinv.Any(n=>n.Id==mats.Id))
                            {
                                locinv.Where(n => n.Id == mats.Id).First().Amount -= mats.Amount;
                            }
                            else
                            {
                                locinv.Add(new Item() {Id = mats.Id,Amount = mats.Amount*-1});
                            }
                        }
                    }
               }
               catch (Exception)
               {
                   // ignored
               }
           }

           var b =locinv.Where(n => n.Amount < 0).ToList();
           foreach (var  item in b)
           {
               ret.Add(item.Id,item.Amount*-1);
           }

           return ret;
       }
       public static Dictionary<int, decimal> NeededItemsForUpgradePercentage()
       {
           var ret = new Dictionary<int, decimal>();
           //1. Needed for upgrades!
           var locinv = new List<Item>();
           var makingph = GetLocalProducionPerHour();
         

           foreach (var building in Core.GlobalData.Buildings)
           {
               try
               {
                 
                   //Next level
                   var nextlvlbuilding = Definitions.BuildingDef.Items.Item.Where(n => n.DefId == building.DefId)
                       .FirstOrDefault()?.Levels.Level.Where(n=>n.Id==building.Level).FirstOrDefault();
                   if (nextlvlbuilding?.Materials.Material != null)
                   {
                       foreach (var mats in nextlvlbuilding?.Materials.Material)
                       {
                           if (locinv.Any(n=>n.Id==mats.Id))
                           {
                               locinv.Where(n => n.Id == mats.Id).First().Amount += mats.Amount;
                           }
                           else
                           {
                              
                               locinv.Add(new Item() {Id = mats.Id,Amount = mats.Amount});
                           }
                       }
                   }
               }
               catch (Exception)
               {
                   // ignored
               }
           }

          
           foreach (var item in locinv)
           {
               if (makingph.ContainsKey(item.Id))
               {

                   decimal koef = ( (decimal) item.Amount/(decimal) makingph[item.Id]);
                   if (koef == 0)
                   {
                       continue;
                   }

                   ret.Add(item.Id, 100M / koef);
               }
             
           }

           return ret;
       }

   }
}
