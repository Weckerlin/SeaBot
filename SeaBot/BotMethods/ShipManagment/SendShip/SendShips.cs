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
    using SeaBotCore.Config;
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    public static  class SendShips
    {
        public static void SendShipsDestination(ShipDestType type)
        {
            var bestships = new Queue<Ship>(Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0 && n.Sent == 0)
                .OrderByDescending(SendingHelper.GetCapacity));
            switch (type)
            {
                case ShipDestType.Upgradable:
                    foreach (var ship in bestships)
                    {
                        Destinations.SendToUpgradable(ship, Core.Config.autoshiptype);
                    }
                    break;
                case ShipDestType.Outpost:
                    foreach (var ship in bestships)
                    {
                        Destinations.SendToOutpost(ship);
                    }
                    break;
                case ShipDestType.Marketplace:
                    foreach (var ship in bestships)
                    {
                        Destinations.SendToMarketplace(ship);
                    }
                    break;
                case ShipDestType.Contractor:
                    foreach (var ship in bestships)
                    {
                        Destinations.SendToContractor(ship);
                    }
                    break;
                case ShipDestType.Auto:
                    SendShips.SendShipsAutoDestination();
                    break;
                case ShipDestType.Wreck:
                    foreach (var ship in bestships)
                    {
                        Destinations.SendToWreck(ship);
                    }
                    break;


            }
        }
        public static void SendShipsAutoDestination()
        {
         
                    var bestships = new Queue<Ship>(Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0 && n.Sent == 0)
                        .OrderByDescending(SendingHelper.GetCapacity));
                    var z = 0;
                    //First 30% to upgradable
                    int upgcont = SendingHelper.GetPercentage(30,bestships.Count);
                    int outpostcont = SendingHelper.GetPercentage(20,bestships.Count);
                    int marketcount = SendingHelper.GetPercentage(15,bestships.Count);
                    int contractorcount = SendingHelper.GetPercentage(30,bestships.Count);
                    int wreckcount =  SendingHelper.GetPercentage(5,bestships.Count);
                   
                    foreach (var ship in AutoTools.TakeAndRemove(bestships,upgcont))
                    {
                       Destinations.SendToUpgradable(ship, Core.Config.autoshiptype);
                    }
                   

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,outpostcont))
                    {
                        Destinations.SendToOutpost(ship);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,marketcount))
                    {
                        Destinations.SendToMarketplace(ship);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,contractorcount))
                    {
                        Destinations.SendToContractor(ship);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,wreckcount))
                    {
                        Destinations.SendToWreck(ship);
                       Logger.Info("wrk"+Definitions.ShipDef.Items.Item.Where(n=>n.DefId==ship.DefId).First().Name);
                    }
                    //Use reverse lookup
                    var failed = new Queue<Ship>(
                        Core.GlobalData.Ships.Where(n => n.TargetId == 0 && n.Activated != 0 && n.Sent == 0)
                            .OrderByDescending(SendingHelper.GetCapacity));

                    foreach (var ship in failed)
                    {
                        var perc = PercentageDest();
                        if (contractorcount > perc[ShipDestType.Contractor])
                        {
                            if (Destinations.SendToContractor(ship))
                            {
                                continue;
                            }
                        }
                        if (upgcont > perc[ShipDestType.Upgradable])
                        {
                            if (Destinations.SendToUpgradable(ship, Core.Config.autoshiptype))
                            {
                                continue;
                            }
                        }
                        if (marketcount > perc[ShipDestType.Marketplace])
                        {
                            if (Destinations.SendToMarketplace(ship))
                            {
                                continue;
                            }
                        }
                        if (outpostcont > perc[ShipDestType.Outpost])
                        {
                            if (Destinations.SendToOutpost(ship))
                            {
                                continue;
                            }

                        }

                        if (Destinations.SendToContractor(ship))
                        {
                            continue;
                        }

                        if (Destinations.SendToUpgradable(ship, Core.Config.autoshiptype))
                        {
                            continue;
                        }

                        if (Destinations.SendToMarketplace(ship))
                        {
                            continue;
                        }

                        if (Destinations.SendToOutpost(ship))
                        {
                            continue;
                        }


                    }
        }

        public static Dictionary<ShipDestType,double> PercentageDest()
        {
            var temp = new Dictionary<ShipDestType,double>();
            temp.Add(ShipDestType.Upgradable,GetPercentage(ShipDestType.Upgradable));
            temp.Add(ShipDestType.Contractor,GetPercentage(ShipDestType.Contractor));
            temp.Add(ShipDestType.Marketplace,GetPercentage(ShipDestType.Marketplace));
            temp.Add(ShipDestType.Outpost,GetPercentage(ShipDestType.Outpost));
            temp.Add(ShipDestType.Wreck,GetPercentage(ShipDestType.Wreck));
            return temp;
        }
        public static double GetPercentage(ShipDestType dest)
        {
            int count = 0;
            switch (dest)
            {
                case ShipDestType.Upgradable:
                    count = Core.GlobalData.Ships.Where(n => n.Sent != 0 && n.Activated!=0 && n.TargetId != 0).Count(n => n.Type == "upgradeable");
                    break;
                case ShipDestType.Outpost:
                    count = Core.GlobalData.Ships.Where(n => n.Sent != 0 && n.Activated!=0 && n.TargetId != 0).Count(n => n.Type == "outpost");
                    break;
                case ShipDestType.Marketplace:
                    count = Core.GlobalData.Ships.Where(n => n.Sent != 0 && n.Activated!=0 && n.TargetId != 0).Count(n => n.Type == "marketplace");
                    break;
                case ShipDestType.Contractor:
                    count = Core.GlobalData.Ships.Where(n => n.Sent != 0 && n.Activated!=0 && n.TargetId != 0).Count(n => n.Type == "contractor");
                    break;
                
                case ShipDestType.Wreck:
                    count = Core.GlobalData.Ships.Where(n => n.Sent != 0 && n.Activated!=0 && n.TargetId != 0).Count(n => n.Type == "wreck");
                    break;

                
            }

            if (count == 0)
            {
                return 0;
            }
            var perc = 100D / (Core.GlobalData.Ships.Count
                               / (double)count);
            return perc;
        }
    }
}
