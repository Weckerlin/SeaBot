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

using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public class BarrelDefenitions
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
            [J("def_id")] public int DefId { get; set; }
            [J("version_id")] public int VersionId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("player_level")] public int PlayerLevel { get; set; }
            [J("event_id")] public int EventId { get; set; }
            [J("model_name")] public string ModelName { get; set; }
            [J("bounds_min")] public string BoundsMin { get; set; }
            [J("bounds_max")] public string BoundsMax { get; set; }
            [J("spawn_time")] public string SpawnTime { get; set; }
            [J("lifespan")] public int Lifespan { get; set; }
            [J("sound")] public string Sound { get; set; }
            [J("positions")] public string Positions { get; set; }
            [J("materials")] public Materials Materials { get; set; }
        }

        public class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public class Material
        {
            public string DefId { get; set; }
            [J("id")] public int Id { get; set; }
            [J("type")] public string Type { get; set; }
            [J("koef")] public double Koef { get; set; }
            [J("exponent_min")] public double ExponentMin { get; set; }
            [J("exponent_max")] public double ExponentMax { get; set; }
            [J("offset_min")] public int OffsetMin { get; set; }
            [J("offset_max")] public int OffsetMax { get; set; }
        }
    }
}