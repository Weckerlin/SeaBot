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

using SeaBotCore.Cache;
using SeaBotCore.Data.Materials;

namespace SeaBotCore.Data.Definitions
{
    public interface IDefinition
    {
    }

    public enum EDefinitionType
    {
        Buildings,
        Barrels,
        Boat,
        Dealer,
        Wreck,
        Ship,
        Marketplace,
        Upgradable,
        Events,
        Material
    }

    public static class Defenitions
    {
        public static BuildingDefentions.Root BuildingDef =>
            (BuildingDefentions.Root) DefenitionCache.GetDefinition(EDefinitionType.Buildings);

        public static BarrelDefenitions.Root BarrelDef =>
            (BarrelDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Barrels);

        public static BoatDefenitions.Root BoatDef =>
            (BoatDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Boat);

        public static DealerDefenitions.Root DealerDef =>
            (DealerDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Dealer);

        public static WreckDefinitions.Root WreckDef =>
            (WreckDefinitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Wreck);

        public static ShipDefenitions.Root ShipDef =>
            (ShipDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Ship);

        public static MarketplaceDefenitions.Root MarketDef =>
            (MarketplaceDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Marketplace);

        public static UpgradeableDefenition.Root UpgrDef =>
            (UpgradeableDefenition.Root) DefenitionCache.GetDefinition(EDefinitionType.Upgradable);

        public static EventsDefenitions.Root EvntDef =>
            (EventsDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Events);

        public static MaterialsData.Root MatDef =>
            (MaterialsData.Root) DefenitionCache.GetDefinition(EDefinitionType.Material);
    }
}