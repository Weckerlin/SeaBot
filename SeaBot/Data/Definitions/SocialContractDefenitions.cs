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
#region

using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;
using R = Newtonsoft.Json.Required;

#endregion

namespace SeaBotCore.Data.Definitions
{
    #region

    using System.Collections.Generic;

    #endregion

    public class SocialContractDefenitions
    {
        public class SocialContract
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("controls")]
            public string Controls { get; set; }

            [J("def_id")]
            public int DefId { get; set; }

            [J("enabled")]
            public int Enabled { get; set; }

            [J("event_id")]
            public int EventId { get; set; }

            [J("iso_height")]
            public int IsoHeight { get; set; }

            [J("iso_width")]
            public int IsoWidth { get; set; }

            [J("map_x")]
            public int MapX { get; set; }

            [J("map_y")]
            public int MapY { get; set; }

            [J("materials")]
            public Materials Materials { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("outpost_id")]
            public int OutpostId { get; set; }

            [J("points")]
            public string Points { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("type")]
            public string Type { get; set; }
        }

        public class SocialContracts
        {
            [J("item")]
            public List<SocialContract> SocialContract { get; set; }
        }

        public class Material
        {
            [J("bonus")]
            public int Bonus { get; set; }

            [J("bonus_koef")]
            public int BonusKoef { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("koef")]
            public int Koef { get; set; }

            [J("material_exponent_max")]
            public double MaterialExponentMax { get; set; }

            [J("material_exponent_min")]
            public double MaterialExponentMin { get; set; }

            [J("reward_material_exponent_max")]
            public double RewardMaterialExponentMax { get; set; }

            [J("reward_material_exponent_min")]
            public double RewardMaterialExponentMin { get; set; }

            [J("reward_material_offest_max")]
            public double RewardMaterialOffsetMax { get; set; }

            [J("reward_material_offset_min")]
            public double RewardMaterialOffsetMin { get; set; }

            [J("reward_xp_max")]
            public int RewardXpMax { get; set; }

            [J("reward_xp_min")]
            public int RewardXpMin { get; set; }
        }

        public class Materials
        {
            [J("material")]
            public List<Material> Material { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public SocialContracts SocialContracts { get; set; }
        }
    }
}