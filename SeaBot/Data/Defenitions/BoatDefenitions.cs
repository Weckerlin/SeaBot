using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Defenitions
{
    public class BoatDefenitions
    {
        public partial class Root
        {
            [J("items")] public Items Items { get; set; }
        }

        public partial class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }

        public partial class Item
        {
            [J("def_id")] public long DefId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("req_type")] public string ReqType { get; set; }
            [J("req_id")] public long ReqId { get; set; }
            [J("req_level")] public long ReqLevel { get; set; }
            [J("shop_order_id")] public long ShopOrderId { get; set; }
            [J("hide")] public long Hide { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("bounds_min")] public string BoundsMin { get; set; }
            [J("bounds_max")] public string BoundsMax { get; set; }
            [J("twist_amount")] public long TwistAmount { get; set; }
            [J("twist_speed")] public long TwistSpeed { get; set; }
            [J("levels")] public Levels Levels { get; set; }
            [J("prices")] public Prices Prices { get; set; }
        }

        public partial class Levels
        {
            [J("level")] public List<Level> Level { get; set; }
        }

        public partial class Level
        {
            [J("id")] public long Id { get; set; }
            [J("mass")] public long Mass { get; set; }
            [J("turn_count")] public long TurnCount { get; set; }
            [J("turn_time")] public long TurnTime { get; set; }
            [J("output_id")] public long OutputId { get; set; }
            [J("output_amount")] public long OutputAmount { get; set; }
            [J("model_name")] public string ModelName { get; set; }
        }

        public partial class Prices
        {
            [J("price")] public List<Price> Price { get; set; }
        }

        public partial class Price
        {
            [J("id")] public long Id { get; set; }
            [J("materials")] public Materials Materials { get; set; }
        }

        public partial class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public partial class Material
        {
            [J("id")] public long Id { get; set; }
            [J("amount")] public long Amount { get; set; }
        }
    }
}
