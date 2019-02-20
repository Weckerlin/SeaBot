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

    using System.Collections.Generic;

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

    public static class LocalDefinitions
    {
        public static List<BarrelDefenitions.Barrel> Barrels =>(
            (BarrelDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Barrels)).Barrels.Barrel;

        public static List<BoatDefenitions.Boat> Boats =>(
            (BoatDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Boat)).Boats.Boat;

        public static List<BuildingDefentions.Building> Buildings =>(
            (BuildingDefentions.Root)DefenitionCache.GetDefinition(EDefinitionType.Buildings)).Buildings.Item;

        public static List<ContractorDefinitions.Contractor> Contractors =>(
            (ContractorDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Contractor)).Contractors.Contractor;

        public static List<DealerDefenitions.Dealer> Dealers =>(
            (DealerDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Dealer)).Dealers.Dealer;

        public static List<EventsDefenitions.Event> Events =>(
            (EventsDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Events)).Events.Event;

        public static GlobalContractorDefinitions.GlobalContractor[] GlobalContractors =>(
            (GlobalContractorDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.GlobalContractor)).GlobalContractors.GlobalContractor;

        public static LevelUPDefenition.LevelUP[] LevelUPs =>
            ((LevelUPDefenition.Root)DefenitionCache.GetDefinition(EDefinitionType.LevelUp)).LevelUPs.LevelUp;

        public static List<MarketplaceDefenitions.Marketplace> Marketplaces =>
            ((MarketplaceDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Marketplace)).Marketplaces.Marketplace;

        public static List<MaterialsData.Material> Materials =>(
            (MaterialsData.Root)DefenitionCache.GetDefinition(EDefinitionType.Material)).Materials.Material;

        public static List<MuseumLevelDefenitions.MuseumLevel> MuseumLevels =>(
            (MuseumLevelDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.MuseumLevels)).MuseumLevels.MuseumLevel;

        public static List<OutpostDefinitions.Outpost> Outposts =>(
            (OutpostDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Outpost)).Outposts.Outpost;

        public static List<SocialContractDefenitions.SocialContract> SocialContracts =>(
            (SocialContractDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.SocialContract)).SocialContracts.SocialContract;

        public static List<ShipDefenitions.Ship> Ships =>(
            (ShipDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Ship)).Ships.Ship;

        public static List<TreasureDefenitions.Treasure> Treasures =>(
            (TreasureDefenitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Treasure)).Treasures.Treasure;

        public static List<UpgradeableDefenition.Upgradable> Upgradables =>(
            (UpgradeableDefenition.Root)DefenitionCache.GetDefinition(EDefinitionType.Upgradable)).Upgradables.Upgradable;

        public static List<WreckDefinitions.Wreck> Wrecks =>(
            (WreckDefinitions.Root)DefenitionCache.GetDefinition(EDefinitionType.Wreck)).Wrecks.Wreck;
    }
}