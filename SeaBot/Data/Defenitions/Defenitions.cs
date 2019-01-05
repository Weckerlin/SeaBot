// SeaBotCore
// Copyright (C) 2019 Weespin
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

using Newtonsoft.Json;

namespace SeaBotCore.Data.Defenitions
{
    public static class Defenitions
    {
        public static BuildingDefentions.Root BuildingDef => Cache.GetBuildingDefenitions();

        public static BarrelDefenitions.Root BarrelDef => Cache.GetBarrelDefenitions();

        public static BoatDefenitions.Root BoatDef => Cache.GetBoatLevelDefenitions();

        public static ShipDefenitions.Root ShipDef => Cache.GetShipDefenitions();
        public static MarketplaceDefenitions.Root MarketDef => Cache.GetMarketPlaceDefenitions();
        public static UpgradeableDefenition.Root UpgrDef => Cache.GetUpgradeablesDefenitions();
        public static EventsDefenitions.Root EvntDef => Cache.GetEventDefenitions();
    }
}