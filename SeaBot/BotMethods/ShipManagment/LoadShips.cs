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

namespace SeaBotCore.BotMethods.ShipManagment
{
    using SeaBotCore.BotMethods.ShipManagment.SendShip;
    using SeaBotCore.Cache;
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;

    public static class LoadShips
    {
        public static void LoadAllShips()
        {
            for (var index = 0; index < Core.LocalPlayer.Ships.Count; index++)
            {
                var ship = Core.LocalPlayer.Ships[index];
                if (ship.TargetId != 0 && ship.Activated != 0 && ship.Loaded == 0 && ship.IsVoyageCompleted())
                {
                    if (ship.Type == "upgradeable")
                    {
                      LoadUpgradeable(ship);
                    }

                    if (ship.Type == "wreck")
                    {
                      LoadWreck(ship);
                    }

                    if (ship.Type == "social_contract")
                    {
                        LoadSocContractor(ship);
                    }
                    if (ship.Type == "contractor")
                    {
                        LoadContractor(ship);
                    }



            }
            }
        }
        public static void LoadUpgradeable(Ship ship)
        {
            var lvl = LocalDefinitions.Upgradables.FirstOrDefault(n => n.DefId == ship.TargetId)?.Levels
                .Level.First(n => n.Id == ship.TargetLevel);
            if (lvl != null)
            {
                Logger.Info(
                    Localization.SHIPS_LOADING + LocalizationCache.GetNameFromLoc(
                        LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).NameLoc,
                        LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).Name));
                Core.LocalPlayer.Upgradeables.First(n => n.DefId == ship.TargetId).CargoOnTheWay -=
                    lvl.MaterialKoef * SendingHelper.GetCapacity(ship);
                Core.LocalPlayer.Upgradeables.First(n => n.DefId == ship.TargetId).Progress +=
                    lvl.MaterialKoef * SendingHelper.GetCapacity(ship);

                Networking.AddTask(new Task.LoadShipUpgradeableTask(ship.InstId));
                ship.Loaded = 1;
            }
        }
        public static void LoadWreck(Ship ship)
        {
           
                Logger.Info(
                    Localization.SHIPS_LOADING + LocalizationCache.GetNameFromLoc(
                        LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).NameLoc,
                        LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).Name));

                Networking.AddTask(new Task.LoadShipWreck(ship.InstId));
                ship.Loaded = 1;
            
        }
        public static void LoadSocContractor(Ship ship)
        {
           
            Logger.Info(
                Localization.SHIPS_LOADING + LocalizationCache.GetNameFromLoc(
                    LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).NameLoc,
                    LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).Name));

            Networking.AddTask(new Task.UnloadShipSocialContractTask(ship.InstId));
            ship.Loaded = 1;
            
        }
        public static void LoadContractor(Ship ship)
        {
           
            Logger.Info(
                Localization.SHIPS_LOADING + LocalizationCache.GetNameFromLoc(
                    LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).NameLoc,
                    LocalDefinitions.Ships.First(n => n.DefId == ship.DefId).Name));

            Networking.AddTask(new Task.UnloadShipContactorTask(ship.InstId));
            ship.Loaded = 1;
            
        }

    }
}
