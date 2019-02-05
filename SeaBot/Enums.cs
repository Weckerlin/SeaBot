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

using System.Diagnostics.CodeAnalysis;

namespace SeaBotCore
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Enums
    {
        public enum EErrorCode
        {
            TASK_CORRUPTED_XML = 101,
            LOGIN = 102,
            UPDATE = 103,
            GENOME_INIT = 104,
            WATCHDOG_TIMEOUT = 106,
            TASK_IO_ERROR = 107,
            TASK_SECURITY_ERROR = 108,
            TASK_WRONG_COLLECTION = 110,
            MAINTENANCE = 1004,
            PLAYER_MAINTENANCE = 1005,
            PLAYER_BANNED = 1006,
            PLAYER_NOT_FOUND = 1007,
            INVALID_SESSION = 1008,
            WRONG_SESSION = 1009,
            EXPIRED_SESSION = 1010,
            COLLECTION_IN_FUTURE = 2000,
            COLLECTION_IN_PAST = 2001,
            TASK_IN_PAST = 2002,
            TASK_IN_FUTURE = 2003
        }

        public enum EFactoryType
        {
            SteelFactory = 4,
            Stone = 5,
            Saw = 3,
            BigSteel = 15,
            BigStone = 14,
            BigSaw = 13,
            Machinery = 11
        }


        public enum EObject
        {
            achievement,
            building,
            construction_site,
            boat,
            boatCargo,
            ship,
            shipCargo,
            hometown,
            contractor,
            marketplace,
            outpost,
            social_contract,
            upgradeable,
            wreck,
            dealer,
            global_contractor,
            crossroad,
            material,
            buildArea,
            levelUp,
            xp,
            sailor,
            product,
            @event,
            barrel,
            museum_ship,
            museum_level,
            captain,
            gift,
            treasure,
            vintage_offer,
            merchant_offer,
            items_offer,
            items_offerCargo,
            lost_treasure,
            personal_guide,
            quest_line,
            captain_new_rarity,
            equation,
            videoreward
        }
    }
}