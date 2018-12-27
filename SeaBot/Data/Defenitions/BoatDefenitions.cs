// SeaBotCore
// Copyright (C) 2018 Weespin
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
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Defenitions
{
    public class BoatDefenitions
    {
        public class Root
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

        public class Levels
        {
            [J("level")] public List<Level> Level { get; set; }
        }

        public class Level
        {
            [J("id")] public long Id { get; set; }
            [J("mass")] public long Mass { get; set; }
            [J("turn_count")] public long TurnCount { get; set; }
            [J("turn_time")] public long TurnTime { get; set; }
            [J("output_id")] public long OutputId { get; set; }
            [J("output_amount")] public long OutputAmount { get; set; }
            [J("model_name")] public string ModelName { get; set; }
        }

        public class Prices
        {
            [J("price")] public List<Price> Price { get; set; }
        }

        public class Price
        {
            [J("id")] public long Id { get; set; }
            [J("materials")] public Materials Materials { get; set; }
        }

        public class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public class Material
        {
            [J("id")] public long Id { get; set; }
            [J("amount")] public long Amount { get; set; }
        }
    }
}