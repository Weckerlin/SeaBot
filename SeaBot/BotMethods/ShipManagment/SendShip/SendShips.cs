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
                       
                    }
                    break;
                case ShipDestType.Outpost:
                    break;
                case ShipDestType.Marketplace:
                    break;
                case ShipDestType.Contractor:
                    break;
                case ShipDestType.Auto:
                    break;
               
            }
        }
        public static void SendShipsAutoDestination()
        {
            List<Ship> failedships = new List<Ship>();

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
                        Logger.Info("mkt"+Definitions.ShipDef.Items.Item.Where(n=>n.DefId==ship.DefId).First().Name);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,contractorcount))
                    {
                        Destinations.SendToContractor(ship);
                       Logger.Info("ctr"+Definitions.ShipDef.Items.Item.Where(n=>n.DefId==ship.DefId).First().Name);
                    }

                    foreach (var ship in AutoTools.TakeAndRemove(bestships,wreckcount))
                    {
                        Destinations.SendToWreck(ship);
                       Logger.Info("wrk"+Definitions.ShipDef.Items.Item.Where(n=>n.DefId==ship.DefId).First().Name);
                    }
        }
    }
}
