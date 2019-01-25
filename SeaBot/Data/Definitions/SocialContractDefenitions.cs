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
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public class SocialContractDefenitions
    {
        public class Root : IDefinition
        {
            [J("items")] public Items Items { get; set; }
        }

        public class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }

        public class Item
        {
            [J("def_id")] public long DefId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("speedupable")] public long Speedupable { get; set; }
            [J("slots")] public long Slots { get; set; }
            [J("enabled")] public long Enabled { get; set; }
            [J("type")] public string Type { get; set; }
            [J("travel_time")] public long TravelTime { get; set; }
            [J("outpost_id")] public long OutpostId { get; set; }
            [J("event_id")] public long EventId { get; set; }
            [J("map_x")] public long MapX { get; set; }
            [J("map_y")] public long MapY { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public long IsoWidth { get; set; }
            [J("iso_height")] public long IsoHeight { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }
            [J("materials")] public Materials Materials { get; set; }
        }

        public class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public class Material
        {
            [J("id")] public int Id { get; set; }

            [J("bonus_koef")] public int BonusKoef { get; set; }
            [J("bonus")] public int Bonus { get; set; }
            [J("koef")] public int Koef { get; set; }
            [J("material_exponent_min")] public double MaterialExponentMin { get; set; }
            [J("material_exponent_max")] public double MaterialExponentMax { get; set; }
            [J("reward_xp_min")] public int RewardXpMin { get; set; }
            [J("reward_xp_max")] public int RewardXpMax { get; set; }
            [J("reward_material_exponent_min")] public double RewardMaterialExponentMin { get; set; }
            [J("reward_material_exponent_max")] public double RewardMaterialExponentMax { get; set; }
            [J("reward_material_offset_min")] public double RewardMaterialOffsetMin { get; set; }
            [J("reward_material_offest_max")] public double RewardMaterialOffsetMax { get; set; }
        }
    }
}