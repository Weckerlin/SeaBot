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
namespace SeaBotCore.Data.Definitions
{
    #region

    using Newtonsoft.Json;

    #endregion

    public class LevelUPDefenition
    {
        public class Item
        {
            [JsonProperty("def_id")]
            public int DefId { get; set; }

            [JsonProperty("median_capacity")]
            public int MedianCapacity { get; set; }

            [JsonProperty("median_crew")]
            public int MedianCrew { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("rewards", NullValueHandling = NullValueHandling.Ignore)]
            public Rewards Rewards { get; set; }

            [JsonProperty("xp")]
            public int Xp { get; set; }
        }

        public class Items
        {
            [JsonProperty("item")]
            public Item[] Item { get; set; }
        }

        public class Reward
        {
            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class Rewards
        {
            [JsonProperty("reward")]
            public Reward[] Reward { get; set; }
        }

        public class Root : IDefinition
        {
            [JsonProperty("items")]
            public Items Items { get; set; }
        }
    }
}