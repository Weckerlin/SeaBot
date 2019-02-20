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

    public static class WreckDefinitions
    {
        public class Wreck
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("chest")]
            public string Chest { get; set; }

            [J("cooldown")]
            public int Cooldown { get; set; }

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

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("req_level")]
            public int ReqLevel { get; set; }

            [J("sets")]
            public Sets Sets { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("spots", NullValueHandling = N.Ignore)]
            public Spots Spots { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("xp_pct_max")]
            public int XpPctMax { get; set; }

            [J("xp_pct_min")]
            public int XpPctMin { get; set; }
        }

        public class Wrecks
        {
            [J("item")]
            public List<Wreck> Wreck { get; set; }
        }

        public class Reward
        {
            [J("amount")]
            public int Amount { get; set; }

            [J("bonus")]
            public double Bonus { get; set; }

            [J("chance")]
            public int Chance { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("koef_max")]
            public double KoefMax { get; set; }

            [J("koef_min")]
            public double KoefMin { get; set; }

            [J("round")]
            public int Round { get; set; }

            [J("type")]
            public string Type { get; set; }
        }

        public class Rewards
        {
            [J("reward")]
            public List<Reward> Reward { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Wrecks Wrecks { get; set; }
        }

        public class Set
        {
            [J("chance")]
            public int Chance { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("reward_count")]
            public int RewardCount { get; set; }

            [J("rewards")]
            public Rewards Rewards { get; set; }
        }

        public class Sets
        {
            [J("set")]
            public List<Set> Set { get; set; }
        }

        public class Spot
        {
            [J("controls")]
            public string Controls { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("map_x")]
            public int MapX { get; set; }

            [J("map_y")]
            public int MapY { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("outpost_id")]
            public int OutpostId { get; set; }

            [J("points")]
            public string Points { get; set; }
        }

        public class Spots
        {
            [J("spot")]
            public List<Spot> Spot { get; set; }
        }
    }
}