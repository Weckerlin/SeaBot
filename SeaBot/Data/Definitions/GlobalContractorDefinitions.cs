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
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public class GlobalContractorDefinitions
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
            [J("name_loc")] public string NameLoc { get; set; }
            [J("title_loc")] public string TitleLoc { get; set; }
            [J("contract_loc")] public string ContractLoc { get; set; }
            [J("wallpost_title")] public string WallpostTitle { get; set; }
            [J("wallpost_desc")] public string WallpostDesc { get; set; }
            [J("wallpost_img")] public string WallpostImg { get; set; }
            [J("objective_amount")] public int ObjectiveAmount { get; set; }
            [J("objective_reward_count")] public int ObjectiveRewardCount { get; set; }
            [J("speedupable")] public int Speedupable { get; set; }
            [J("slots")] public int Slots { get; set; }
            [J("travel_time")] public int TravelTime { get; set; }
            [J("event_id")] public int EventId { get; set; }
            [J("milestone_count")] public int MilestoneCount { get; set; }
            [J("map_x")] public int MapX { get; set; }
            [J("map_y")] public int MapY { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public int IsoWidth { get; set; }
            [J("iso_height")] public int IsoHeight { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }
            [J("milestones")] public Milestones Milestones { get; set; }
            [J("objectives")] public Objectives Objectives { get; set; }
            [J("rewards")] public Rewards Rewards { get; set; }

            [J("unlocked_locations", NullValueHandling = N.Ignore)]
            public UnlockedLocations UnlockedLocations { get; set; }

            [J("required_locations", NullValueHandling = N.Ignore)]
            public RequiredLocations RequiredLocations { get; set; }
        }

        public class Milestones
        {
            [J("milestone")] public List<Milestone> Milestone { get; set; }
        }

        public class Milestone
        {
            [J("id")] public int Id { get; set; }
            [J("amount")] public int Amount { get; set; }
            [J("chest")] public string Chest { get; set; }
            [J("koef_min")] public double KoefMin { get; set; }
            [J("koef_max")] public double KoefMax { get; set; }
            [J("rewards_count")] public int RewardsCount { get; set; }
            [J("progress_koef")] public int ProgressKoef { get; set; }
            [J("rewards")] public Rewards Rewards { get; set; }
        }

        public class Rewards
        {
            [J("reward")] public List<Reward> Reward { get; set; }
        }

        public class Reward
        {
            [J("id")] public int Id { get; set; }
            [J("object_type")] public string ObjectType { get; set; }
            [J("object_def_id")] public int ObjectDefId { get; set; }
            [J("amount")] public int Amount { get; set; }
            [J("bonus")] public double Bonus { get; set; }
            [J("chance")] public int Chance { get; set; }
            [J("round")] public int Round { get; set; }
            [J("min_xp_pct")] public int MinXpPct { get; set; }
            [J("max_xp_pct")] public int MaxXpPct { get; set; }
        }

        public class Objectives
        {
            [J("objective")] public List<Objective> Objective { get; set; }
        }

        public class Objective
        {
            [J("id")] public int Id { get; set; }
            [J("object_type")] public string ObjectType { get; set; }
            [J("object_def_id")] public int ObjectDefId { get; set; }
            [J("material_koef")] public double MaterialKoef { get; set; }
            [J("koef_min")] public double KoefMin { get; set; }
            [J("koef_max")] public double KoefMax { get; set; }
            [J("material_offset")] public double MaterialOffset { get; set; }
            [J("round")] public int Round { get; set; }
        }

        public class RequiredLocations
        {
            [J("location")] public List<RequiredLocationsLocation> Location { get; set; }
        }

        public class RequiredLocationsLocation
        {
            [J("type")] public string Type { get; set; }
            [J("unique_id")] public int UniqueId { get; set; }
        }

        public class UnlockedLocations
        {
            [J("locations")] public List<UnlockedLocationsLocation> Locations { get; set; }
        }

        public class UnlockedLocationsLocation
        {
            [J("type")] public string Type { get; set; }

            [J("ids")]
            [JsonConverter(typeof(ParseStringConverter))]
            public int Ids { get; set; }
        }

        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t)
            {
                return t == typeof(int) || t == typeof(int?);
            }

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }

                var value = serializer.Deserialize<string>(reader);
                long l;
                if (Int64.TryParse(value, out l))
                {
                    return l;
                }

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

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }
    }
}