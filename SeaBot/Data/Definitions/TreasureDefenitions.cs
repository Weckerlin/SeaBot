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

    public class TreasureDefenitions
    {
        public class Item
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("cargo_model")]
            public string CargoModel { get; set; }

            [J("controls")]
            public string Controls { get; set; }

            [J("def_id")]
            public int DefId { get; set; }

            [J("event_id")]
            public int EventId { get; set; }

            [J("iso_height")]
            public int IsoHeight { get; set; }

            [J("iso_width")]
            public int IsoWidth { get; set; }

            [J("koef")]
            public double Koef { get; set; }

            [J("map_x")]
            public int MapX { get; set; }

            [J("map_y")]
            public int MapY { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("outpost_id")]
            public int OutpostId { get; set; }

            [J("points")]
            public string Points { get; set; }

            [J("reward_count")]
            public int RewardCount { get; set; }

            [J("rewards")]
            public Rewards Rewards { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("texture")]
            public string Texture { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("ui_texture")]
            public string UiTexture { get; set; }

            [J("xp")]
            public int Xp { get; set; }
        }

        public class Items
        {
            [J("item")]
            public List<Item> Item { get; set; }
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
            public Items Items { get; set; }
        }
    }
}