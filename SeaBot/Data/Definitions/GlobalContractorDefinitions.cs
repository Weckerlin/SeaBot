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

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    #endregion

    public class GlobalContractorDefinitions
    {
        public partial class Root : IDefinition
        {
            [JsonProperty("items")]
            public GlobalContractors GlobalContractors { get; set; }
        }
        public partial class GlobalContractors
        {
            [JsonProperty("item")]
            public GlobalContractor[] GlobalContractor { get; set; }
        }

        public partial class GlobalContractor
        {
            [JsonProperty("def_id")]
            public long DefId { get; set; }

            [JsonProperty("version_id")]
            public long VersionId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("name_loc")]
            public string NameLoc { get; set; }

            [JsonProperty("title_loc")]
            public string TitleLoc { get; set; }

            [JsonProperty("contract_loc")]
            public string ContractLoc { get; set; }

            [JsonProperty("wallpost_title")]
            public string WallpostTitle { get; set; }

            [JsonProperty("wallpost_desc")]
            public string WallpostDesc { get; set; }

            [JsonProperty("objective_amount")]
            public long ObjectiveAmount { get; set; }

            [JsonProperty("objective_reward_count")]
            public long ObjectiveRewardCount { get; set; }

            [JsonProperty("speedupable")]
            public long Speedupable { get; set; }

            [JsonProperty("slots")]
            public long Slots { get; set; }

            [JsonProperty("travel_time")]
            public long TravelTime { get; set; }

            [JsonProperty("event_id")]
            public long EventId { get; set; }

            [JsonProperty("milestone_count")]
            public long MilestoneCount { get; set; }

            [JsonProperty("map_x")]
            public long MapX { get; set; }

            [JsonProperty("map_y")]
            public long MapY { get; set; }

            [JsonProperty("atlas")]
            public Atlas Atlas { get; set; }

            [JsonProperty("iso_width")]
            public long IsoWidth { get; set; }

            [JsonProperty("iso_height")]
            public long IsoHeight { get; set; }

            [JsonProperty("points")]
            public string Points { get; set; }

            [JsonProperty("controls")]
            public string Controls { get; set; }

            [JsonProperty("milestones")]
            public Milestones Milestones { get; set; }

            [JsonProperty("objectives")]
            public Objectives Objectives { get; set; }

            [JsonProperty("rewards")]
            public Rewards Rewards { get; set; }

            [JsonProperty("unlocked_locations", NullValueHandling = NullValueHandling.Ignore)]
            public UnlockedLocations UnlockedLocations { get; set; }

            [JsonProperty("required_locations", NullValueHandling = NullValueHandling.Ignore)]
            public RequiredLocations RequiredLocations { get; set; }
        }

        public partial class Milestones
        {
            [JsonProperty("milestone")]
            public Milestone[] Milestone { get; set; }
        }

        public partial class Milestone
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("amount")]
            public long Amount { get; set; }

            [JsonProperty("chest")]
            public string Chest { get; set; }

            [JsonProperty("koef_min")]
            public double KoefMin { get; set; }

            [JsonProperty("koef_max")]
            public double KoefMax { get; set; }

            [JsonProperty("rewards_count")]
            public long RewardsCount { get; set; }

            [JsonProperty("progress_koef")]
            public long ProgressKoef { get; set; }

            [JsonProperty("rewards")]
            public Rewards Rewards { get; set; }
        }

        public partial class Rewards
        {
            [JsonProperty("reward")]
            public Reward[] Reward { get; set; }
        }

        public partial class Reward
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("object_type")]
            public RewardObjectType ObjectType { get; set; }

            [JsonProperty("object_def_id")]
            public long ObjectDefId { get; set; }

            [JsonProperty("amount")]
            public long Amount { get; set; }

            [JsonProperty("bonus")]
            public double Bonus { get; set; }

            [JsonProperty("chance")]
            public long Chance { get; set; }

            [JsonProperty("round")]
            public long Round { get; set; }

            [JsonProperty("min_xp_pct")]
            public long MinXpPct { get; set; }

            [JsonProperty("max_xp_pct")]
            public long MaxXpPct { get; set; }
        }

        public partial class Objectives
        {
            [JsonProperty("objective")]
            public Objective[] Objective { get; set; }
        }

        public partial class Objective
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("object_type")]
            public ObjectiveObjectType ObjectType { get; set; }

            [JsonProperty("object_def_id")]
            public long ObjectDefId { get; set; }

            [JsonProperty("material_koef")]
            public double MaterialKoef { get; set; }

            [JsonProperty("koef_min")]
            public double KoefMin { get; set; }

            [JsonProperty("koef_max")]
            public double KoefMax { get; set; }

            [JsonProperty("material_offset")]
            public double MaterialOffset { get; set; }

            [JsonProperty("round")]
            public long Round { get; set; }
        }

        public partial class RequiredLocations
        {
            [JsonProperty("location")]
            public RequiredLocationsLocation[] Location { get; set; }
        }

        public partial class RequiredLocationsLocation
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("unique_id")]
            public long UniqueId { get; set; }
        }

        public partial class UnlockedLocations
        {
            [JsonProperty("locations")]
            public UnlockedLocationsLocation[] Locations { get; set; }
        }

        public partial class UnlockedLocationsLocation
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("ids")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Ids { get; set; }
        }

        public enum Atlas
        {
            MapGround1
        };


        public enum RewardObjectType
        {
            Captain,

            Material,

            Ship,

            Xp
        };

        public enum ObjectiveObjectType
        {
            Material,

            Sailor
        };



        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
                                                                         {
                                                                             MetadataPropertyHandling =
                                                                                 MetadataPropertyHandling.Ignore,
                                                                             DateParseHandling = DateParseHandling.None,
                                                                             Converters =
                                                                                 {
                                                                                     AtlasConverter.Singleton,
                                                                                     RewardObjectTypeConverter
                                                                                         .Singleton,
                                                                                     ObjectiveObjectTypeConverter
                                                                                         .Singleton,
                                                                                     new IsoDateTimeConverter
                                                                                         {
                                                                                             DateTimeStyles =
                                                                                                 DateTimeStyles
                                                                                                     .AssumeUniversal
                                                                                         }
                                                                                 },
                                                                         };
        }

        internal class AtlasConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Atlas) || t == typeof(Atlas?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "mapGround1")
                {
                    return Atlas.MapGround1;
                }

                throw new Exception("Cannot unmarshal type Atlas");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }

                var value = (Atlas)untypedValue;
                if (value == Atlas.MapGround1)
                {
                    serializer.Serialize(writer, "mapGround1");
                    return;
                }

                throw new Exception("Cannot marshal type Atlas");
            }

            public static readonly AtlasConverter Singleton = new AtlasConverter();
        }

     

        internal class RewardObjectTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(RewardObjectType) || t == typeof(RewardObjectType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "captain":
                        return RewardObjectType.Captain;
                    case "material":
                        return RewardObjectType.Material;
                    case "ship":
                        return RewardObjectType.Ship;
                    case "xp":
                        return RewardObjectType.Xp;
                }

                throw new Exception("Cannot unmarshal type RewardObjectType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }

                var value = (RewardObjectType)untypedValue;
                switch (value)
                {
                    case RewardObjectType.Captain:
                        serializer.Serialize(writer, "captain");
                        return;
                    case RewardObjectType.Material:
                        serializer.Serialize(writer, "material");
                        return;
                    case RewardObjectType.Ship:
                        serializer.Serialize(writer, "ship");
                        return;
                    case RewardObjectType.Xp:
                        serializer.Serialize(writer, "xp");
                        return;
                }

                throw new Exception("Cannot marshal type RewardObjectType");
            }

            public static readonly RewardObjectTypeConverter Singleton = new RewardObjectTypeConverter();
        }

        internal class ObjectiveObjectTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) =>
                t == typeof(ObjectiveObjectType) || t == typeof(ObjectiveObjectType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "material":
                        return ObjectiveObjectType.Material;
                    case "sailor":
                        return ObjectiveObjectType.Sailor;
                }

                throw new Exception("Cannot unmarshal type ObjectiveObjectType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }

                var value = (ObjectiveObjectType)untypedValue;
                switch (value)
                {
                    case ObjectiveObjectType.Material:
                        serializer.Serialize(writer, "material");
                        return;
                    case ObjectiveObjectType.Sailor:
                        serializer.Serialize(writer, "sailor");
                        return;
                }

                throw new Exception("Cannot marshal type ObjectiveObjectType");
            }

            public static readonly ObjectiveObjectTypeConverter Singleton = new ObjectiveObjectTypeConverter();
        }

        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                long l;
                if (Int64.TryParse(value, out l))
                {
                    return l;
                }

                throw new Exception("Cannot unmarshal type long");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }

                var value = (long)untypedValue;
                serializer.Serialize(writer, value.ToString());
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }
    }
}