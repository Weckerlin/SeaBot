using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeaBotCore.Data.Definitions
{
  public  class LevelUPDefenition 
    {
        public partial class Root : IDefinition
        {
            [JsonProperty("items")]
            public Items Items { get; set; }
        }

        public partial class Items
        {
            [JsonProperty("item")]
            public Item[] Item { get; set; }
        }

        public partial class Item
        {
            [JsonProperty("def_id")]
            public int DefId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("xp")]
            public int Xp { get; set; }

            [JsonProperty("median_capacity")]
            public int MedianCapacity { get; set; }

            [JsonProperty("median_crew")]
            public int MedianCrew { get; set; }

            [JsonProperty("rewards", NullValueHandling = NullValueHandling.Ignore)]
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
            public int Id { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("amount")]
            public int Amount { get; set; }
        }

    }
}
