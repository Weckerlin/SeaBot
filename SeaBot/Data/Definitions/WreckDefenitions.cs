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
    public static class WreckDefinitions
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
            [J("req_level")] public long ReqLevel { get; set; }
            [J("travel_time")] public long TravelTime { get; set; }
            [J("event_id")] public long EventId { get; set; }
            [J("cooldown")] public long Cooldown { get; set; }
            [J("xp_pct_min")] public long XpPctMin { get; set; }
            [J("xp_pct_max")] public long XpPctMax { get; set; }
            [J("chest")] public string Chest { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public long IsoWidth { get; set; }
            [J("iso_height")] public long IsoHeight { get; set; }
            [J("sets")] public Sets Sets { get; set; }

            [J("spots", NullValueHandling = N.Ignore)]
            public Spots Spots { get; set; }
        }

        public class Sets
        {
            [J("set")] public List<Set> Set { get; set; }
        }

        public class Set
        {
            [J("id")] public long Id { get; set; }
            [J("reward_count")] public long RewardCount { get; set; }
            [J("chance")] public long Chance { get; set; }
            [J("rewards")] public Rewards Rewards { get; set; }
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
            [J("bonus")] public double Bonus { get; set; }
            [J("koef_min")] public double KoefMin { get; set; }
            [J("koef_max")] public double KoefMax { get; set; }
            [J("chance")] public long Chance { get; set; }
            [J("round")] public long Round { get; set; }
        }

        public class Spots
        {
            [J("spot")] public List<Spot> Spot { get; set; }
        }

        public class Spot
        {
            [J("id")] public long Id { get; set; }
            [J("name")] public string Name { get; set; }
            [J("map_x")] public long MapX { get; set; }
            [J("map_y")] public long MapY { get; set; }
            [J("outpost_id")] public long OutpostId { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }
        }
    }
}