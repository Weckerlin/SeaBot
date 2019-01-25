﻿// SeaBotCore
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
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public static class ContractorDefinitions
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
            [J("version_id")] public long VersionId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("picture")] public string Picture { get; set; }
            [J("type")] public string Type { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("contractor_loc")] public string ContractorLoc { get; set; }
            [J("final_reward_loc")] public string FinalRewardLoc { get; set; }
            [J("req_level")] public long ReqLevel { get; set; }
            [J("rec_level")] public long RecLevel { get; set; }
            [J("speedupable")] public long Speedupable { get; set; }
            [J("sailors")] public long Sailors { get; set; }
            [J("slots")] public long Slots { get; set; }
            [J("travel_time")] public long TravelTime { get; set; }
            [J("unlock_time")] public long UnlockTime { get; set; }
            [J("event_id")] public long EventId { get; set; }
            [J("quest_count")] public long QuestCount { get; set; }
            [J("map_x")] public long MapX { get; set; }
            [J("map_y")] public long MapY { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public long IsoWidth { get; set; }
            [J("iso_height")] public long IsoHeight { get; set; }
            [J("thumb_size")] public string ThumbSize { get; set; }
            [J("thumb_offset")] public string ThumbOffset { get; set; }
            [J("head_offset")] public string HeadOffset { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }
            [J("final_rewards")] public FinalRewards FinalRewards { get; set; }
            [J("quests")] public Quests Quests { get; set; }

            [J("required_locations", NullValueHandling = N.Ignore)]
            public RequiredLocations RequiredLocations { get; set; }

            [J("unlocked_locations", NullValueHandling = N.Ignore)]
            public UnlockedLocations UnlockedLocations { get; set; }
        }

        public class FinalRewards
        {
            [J("final_reward")] public List<FinalReward> FinalReward { get; set; }
        }

        public class FinalReward
        {
            [J("id")] public long Id { get; set; }
            [J("type")] public string Type { get; set; }
            [J("amount")] public long Amount { get; set; }
            [J("min_level")] public long MinLevel { get; set; }
            [J("max_level")] public long MaxLevel { get; set; }
        }

        public class Quests
        {
            [J("quest")] public List<Quest> Quest { get; set; }
        }

        public class Quest
        {
            [J("title_loc")] public string TitleLoc { get; set; }
            [J("text_loc")] public string TextLoc { get; set; }
            [J("id")] public long Id { get; set; }
            [J("objective_type_id")] public string ObjectiveTypeId { get; set; }
            [J("objective_def_id")] public long ObjectiveDefId { get; set; }
            [J("material_koef")] public long MaterialKoef { get; set; }
            [J("bonus")] public long Bonus { get; set; }
            [J("difficulty")] public long Difficulty { get; set; }
            [J("amount")] public long Amount { get; set; }
            [J("round")] public long Round { get; set; }

            [J("rewards", NullValueHandling = N.Ignore)]
            public Rewards Rewards { get; set; }
        }

        public class Rewards
        {
            [J("reward")] public List<Reward> Reward { get; set; }
        }

        public class Reward
        {
            [J("id")] public long Id { get; set; }
            [J("type")] public string Type { get; set; }
            [J("amount")] public long Amount { get; set; }
            [J("bonus")] public long Bonus { get; set; }
            [J("xp_pct")] public long XpPct { get; set; }
            [J("round")] public long Round { get; set; }
        }

        public class RequiredLocations
        {
            [J("location")] public List<RequiredLocationsLocation> Location { get; set; }
        }

        public class RequiredLocationsLocation
        {
            [J("type")] public string Type { get; set; }
            [J("unique_id")] public long UniqueId { get; set; }
        }

        public class UnlockedLocations
        {
            [J("locations")] public List<UnlockedLocationsLocation> Locations { get; set; }
        }

        public class UnlockedLocationsLocation
        {
            [J("type")] public string Type { get; set; }
            [J("ids")] public string Ids { get; set; }
        }
    }
}