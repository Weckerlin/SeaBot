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

#endregion

namespace SeaBotCore.Data.Definitions
{
    #region

    using System.Collections.Generic;

    #endregion

    public class BoatDefenitions
    {
        public class Boat
        {
            [J("bounds_max")]
            public string BoundsMax { get; set; }

            [J("bounds_min")]
            public string BoundsMin { get; set; }

            [J("def_id")]
            public int DefId { get; set; }

            [J("hide")]
            public int Hide { get; set; }

            [J("levels")]
            public BoatLevels BoatLevels { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("prices")]
            public Prices Prices { get; set; }

            [J("req_id")]
            public int ReqId { get; set; }

            [J("req_level")]
            public int ReqLevel { get; set; }

            [J("req_type")]
            public string ReqType { get; set; }

            [J("shop_order_id")]
            public int ShopOrderId { get; set; }

            [J("texture")]
            public string Texture { get; set; }

            [J("twist_amount")]
            public int TwistAmount { get; set; }

            [J("twist_speed")]
            public int TwistSpeed { get; set; }
        }

        public class Boats
        {
            [J("item")]
            public List<Boat> Boat { get; set; }
        }

        public class BoatLevel
        {
            [J("id")]
            public int Id { get; set; }

            [J("mass")]
            public int Mass { get; set; }

            [J("model_name")]
            public string ModelName { get; set; }

            [J("output_amount")]
            public int OutputAmount { get; set; }

            [J("output_id")]
            public int OutputId { get; set; }

            [J("turn_count")]
            public int TurnCount { get; set; }

            [J("turn_time")]
            public int TurnTime { get; set; }
        }

        public class BoatLevels
        {
            [J("level")]
            public List<BoatLevel> Level { get; set; }
        }

        public class Material
        {
            [J("amount")]
            public int Amount { get; set; }

            [J("id")]
            public int Id { get; set; }
        }

        public class Materials
        {
            [J("material")]
            public List<Material> Material { get; set; }
        }

        public class Price
        {
            [J("id")]
            public int Id { get; set; }

            [J("materials")]
            public Materials Materials { get; set; }
        }

        public class Prices
        {
            [J("price")]
            public List<Price> Price { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Boats Boats { get; set; }
        }
    }
}