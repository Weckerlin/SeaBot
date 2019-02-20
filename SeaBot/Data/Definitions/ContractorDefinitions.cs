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

    public static class ContractorDefinitions
    {
        public class FinalReward
        {
            [J("amount")]
            public int Amount { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("max_level")]
            public int MaxLevel { get; set; }

            [J("min_level")]
            public int MinLevel { get; set; }

            [J("type")]
            public string Type { get; set; }
        }

        public class FinalRewards
        {
            [J("final_reward")]
            public List<FinalReward> FinalReward { get; set; }
        }

        public class Contractor
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("contractor_loc")]
            public string ContractorLoc { get; set; }

            [J("controls")]
            public string Controls { get; set; }

            [J("def_id")]
            public int DefId { get; set; }

            [J("event_id")]
            public int EventId { get; set; }

            [J("final_reward_loc")]
            public string FinalRewardLoc { get; set; }

            [J("final_rewards")]
            public FinalRewards FinalRewards { get; set; }

            [J("head_offset")]
            public string HeadOffset { get; set; }

            [J("iso_height")]
            public int IsoHeight { get; set; }

            [J("iso_width")]
            public int IsoWidth { get; set; }

            [J("map_x")]
            public int MapX { get; set; }

            [J("map_y")]
            public int MapY { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("picture")]
            public string Picture { get; set; }

            [J("points")]
            public string Points { get; set; }

            [J("quest_count")]
            public int QuestCount { get; set; }

            [J("quests")]
            public Quests Quests { get; set; }

            [J("rec_level")]
            public int RecLevel { get; set; }

            [J("req_level")]
            public int ReqLevel { get; set; }

            [J("required_locations", NullValueHandling = N.Ignore)]
            public RequiredLocations RequiredLocations { get; set; }

            [J("sailors")]
            public int Sailors { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("thumb_offset")]
            public string ThumbOffset { get; set; }

            [J("thumb_size")]
            public string ThumbSize { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("type")]
            public string Type { get; set; }

            [J("unlocked_locations", NullValueHandling = N.Ignore)]
            public UnlockedLocations UnlockedLocations { get; set; }

            [J("unlock_time")]
            public int UnlockTime { get; set; }

            [J("version_id")]
            public int VersionId { get; set; }
        }

        public class Items
        {
            [J("item")]
            public List<Contractor> Item { get; set; }
        }

        public class Quest
        {
            [J("amount")]
            public int Amount { get; set; }

            [J("bonus")]
            public int Bonus { get; set; }

            [J("difficulty")]
            public int Difficulty { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("material_koef")]
            public int MaterialKoef { get; set; }

            [J("objective_def_id")]
            public int ObjectiveDefId { get; set; }

            [J("objective_type_id")]
            public string ObjectiveTypeId { get; set; }

            [J("rewards", NullValueHandling = N.Ignore)]
            public Rewards Rewards { get; set; }

            [J("round")]
            public int Round { get; set; }

            [J("text_loc")]
            public string TextLoc { get; set; }

            [J("title_loc")]
            public string TitleLoc { get; set; }
        }

        public class Quests
        {
            [J("quest")]
            public List<Quest> Quest { get; set; }
        }

        public class RequiredLocations
        {
            [J("location")]
            public List<RequiredLocationsLocation> Location { get; set; }
        }

        public class RequiredLocationsLocation
        {
            [J("type")]
            public string Type { get; set; }

            [J("unique_id")]
            public int UniqueId { get; set; }
        }

        public class Reward
        {
            [J("amount")]
            public int Amount { get; set; }

            [J("bonus")]
            public int Bonus { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("round")]
            public int Round { get; set; }

            [J("type")]
            public string Type { get; set; }

            [J("xp_pct")]
            public int XpPct { get; set; }
        }

        public class Rewards
        {
            [J("reward")]
            public List<Reward> Reward { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Items Items { get; set; }
        }

        public class UnlockedLocations
        {
            [J("locations")]
            public List<UnlockedLocationsLocation> Locations { get; set; }
        }

        public class UnlockedLocationsLocation
        {
            [J("ids")]
            public string Ids { get; set; }

            [J("type")]
            public string Type { get; set; }
        }
    }
}