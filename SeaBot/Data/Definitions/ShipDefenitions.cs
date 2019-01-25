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
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public class ShipDefenitions
    {
        public class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")] public Items Items { get; set; }
        }

        public class Item
        {
            [J("def_id")] public int DefId { get; set; }
            [J("version_id")] public int VersionId { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("desc_loc")] public string DescLoc { get; set; }
            [J("ship_type")] public string ShipType { get; set; }
            [J("propulsion")] public string Propulsion { get; set; }
            [J("slot_usage")] public int SlotUsage { get; set; }

            [J("mass")]
            [JsonConverter(typeof(ParseStringConverter))]
            public int Mass { get; set; }

            [J("start_level")] public int StartLevel { get; set; }
            [J("player_level")] public int PlayerLevel { get; set; }
            [J("req_type")] public string ReqType { get; set; }
            [J("req_id")] public int ReqId { get; set; }
            [J("req_level")] public int ReqLevel { get; set; }
            [J("max_count")] public int MaxCount { get; set; }
            [J("shop_order_id")] public int ShopOrderId { get; set; }
            [J("event_id")] public int EventId { get; set; }
            [J("museum_xp")] public int MuseumXp { get; set; }
            [J("hide")] public int Hide { get; set; }
            [J("bounds_min")] public string BoundsMin { get; set; }
            [J("bounds_max")] public string BoundsMax { get; set; }

            [J("animations", NullValueHandling = N.Ignore)]
            public Animations Animations { get; set; }

            [J("levels", NullValueHandling = N.Ignore)]
            public LevelsClass Levels { get; set; }

            [J("capacity_levels", NullValueHandling = N.Ignore)]
            public Levels CapacityLevels { get; set; }

            [J("sailors_levels", NullValueHandling = N.Ignore)]
            public Levels SailorsLevels { get; set; }

            [J("particles", NullValueHandling = N.Ignore)]
            public Particles Particles { get; set; }

            [J("particles_new", NullValueHandling = N.Ignore)]
            public ParticlesNew ParticlesNew { get; set; }
        }

        public class Animations
        {
            [J("animation")] public List<Animation> Animation { get; set; }
        }

        public class Animation
        {
            [J("id")] public int Id { get; set; }
            [J("data")] public string Data { get; set; }
        }

        public class Levels
        {
            [J("level")] public List<CapacityLevelsLevel> Level { get; set; }
        }

        public class CapacityLevelsLevel
        {
            [J("id")] public int Id { get; set; }
            [J("xp")] public int Xp { get; set; }
            [J("gem_price")] public int GemPrice { get; set; }

            [J("capacity", NullValueHandling = N.Ignore)]
            public int? Capacity { get; set; }

            [J("materials")] public Materials Materials { get; set; }

            [J("sailors", NullValueHandling = N.Ignore)]
            public int? Sailors { get; set; }
        }

        public class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public class Material
        {
            [J("id")] public int Id { get; set; }
            [J("amount")] public int Amount { get; set; }
        }

        public class LevelsClass
        {
            [J("level")] public List<LevelsLevel> Level { get; set; }
        }

        public class LevelsLevel
        {
            [J("id")] public int Id { get; set; }
            [J("xp")] public int Xp { get; set; }
            [J("gem_price")] public int GemPrice { get; set; }
            [J("sailors")] public int Sailors { get; set; }
            [J("capacity")] public int Capacity { get; set; }
            [J("speed")] public int Speed { get; set; }

            [J("materials", NullValueHandling = N.Ignore)]
            public Materials Materials { get; set; }
        }

        public class Particles
        {
            [J("particle")] public List<Animation> Particle { get; set; }
        }

        public class ParticlesNew
        {
            [J("particle")] public List<Particle> Particle { get; set; }
        }

        public class Particle
        {
            [J("particle_id")] public int ParticleId { get; set; }
            [J("data")] public string Data { get; set; }
        }

        internal class ParseStringConverter : JsonConverter
        {
            public static readonly ParseStringConverter Singleton = new ParseStringConverter();

            public override bool CanConvert(Type t)
            {
                return t == typeof(int) || t == typeof(int?);
            }

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                int l;
                if (int.TryParse(value, out l)) return l;

                throw new Exception("Cannot unmarshal type int");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }

                var value = (int) untypedValue;
                serializer.Serialize(writer, value.ToString());
            }
        }
    }
}