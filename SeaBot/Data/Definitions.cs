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
namespace SeaBotCore.Data.Definitions
{
    #region

    using SeaBotCore.Cache;
    using SeaBotCore.Data.Materials;

    #endregion

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
        public static BarrelDefenitions.Root BarrelDef =>
            (BarrelDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Barrels);

        public static BoatDefenitions.Root BoatDef =>
            (BoatDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Boat);

        public static BuildingDefentions.Root BuildingDef =>
            (BuildingDefentions.Root)DefenitionCache.GetDefinition(EDefinitionType.Buildings);

        public static ContractorDefinitions.Root ConDef =>
            (ContractorDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Contractor);

        public static DealerDefenitions.Root DealerDef =>
            (DealerDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Dealer);

        public static EventsDefenitions.Root EvntDef =>
            (EventsDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Events);

        public static GlobalContractorDefinitions.Root GConDef =>
            (GlobalContractorDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.GlobalContractor);

        public static LevelUPDefenition.Root LevelUpDef =>
            (LevelUPDefenition.Root)DefenitionCache.GetDefinition(EDefinitionType.LevelUp);

        public static MarketplaceDefenitions.Root MarketDef =>
            (MarketplaceDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Marketplace);

        public static MaterialsData.Root MatDef =>
            (MaterialsData.Root)DefenitionCache.GetDefinition(EDefinitionType.Material);

        public static MuseumLevelDefenitions.Root MuseumLvlDef =>
            (MuseumLevelDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.MuseumLevels);

        public static OutpostDefinitions.Root OutpostDef =>
            (OutpostDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Outpost);

        public static SocialContractDefenitions.Root SContractDef =>
            (SocialContractDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.SocialContract);

        public static ShipDefenitions.Root ShipDef =>
            (ShipDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Ship);

        public static TreasureDefenitions.Root TreasureDef =>
            (TreasureDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Treasure);

        public static UpgradeableDefenition.Root UpgrDef =>
            (UpgradeableDefenition.Root)DefenitionCache.GetDefinition(EDefinitionType.Upgradable);

        public static WreckDefinitions.Root WreckDef =>
            (WreckDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Wreck);
    }
}