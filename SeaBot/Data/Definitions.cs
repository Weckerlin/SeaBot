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
// aint with this program.  If not, see <http://www.gnu.org/licenses/>.

using SeaBotCore.Cache;
using SeaBotCore.Data.Materials;

namespace SeaBotCore.Data.Definitions
{
    internal interface IDefinition
    {
    }

    internal enum EDefinitionType
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
        Material,
        Contractor,
        GlobalContractor,
        Outpost,
        SocialContract,
        Treasure,
        MuseumLevels,
        LevelUp
    }

    public static class Definitions
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

        public static GlobalContractorDefinitions.Root GConDef =>
            (GlobalContractorDefinitions.Root) DefenitionCache.GetDefinition(EDefinitionType.GlobalContractor);

        public static ContractorDefinitions.Root ConDef =>
            (ContractorDefinitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Contractor);

        public static OutpostDefinitions.Root OutpostDef =>
            (OutpostDefinitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Outpost);

        public static SocialContractDefenitions.Root SContractDef =>
            (SocialContractDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.SocialContract);

        public static TreasureDefenitions.Root TreasureDef =>
            (TreasureDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.Treasure);

        public static MuseumLevelDefenitions.Root MuseumLvlDef =>
            (MuseumLevelDefenitions.Root) DefenitionCache.GetDefinition(EDefinitionType.MuseumLevels);
        public static LevelUPDefenition.Root LevelUpDef =>
            (LevelUPDefenition.Root)DefenitionCache.GetDefinition(EDefinitionType.LevelUp);
    }
}