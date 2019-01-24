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

using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public class MarketplaceDefenitions
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
            [J("def_id")] public int DefId { get; set; }
            [J("version_id")] public int VersionId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("speedupable")] public int Speedupable { get; set; }
            [J("sailors")] public int Sailors { get; set; }
            [J("slots")] public int Slots { get; set; }
            [J("travel_time")] public int TravelTime { get; set; }
            [J("outpost_id")] public int OutpostId { get; set; }
            [J("contractor_id")] public int ContractorId { get; set; }
            [J("map_x")] public int MapX { get; set; }
            [J("map_y")] public int MapY { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public int IsoWidth { get; set; }
            [J("iso_height")] public int IsoHeight { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }
            [J("materials")] public Materials Materials { get; set; }
        }

        public class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public class Material
        {
            [J("desc_loc")] public string DescLoc { get; set; }
            [J("contractor_id")] public int ContractorId { get; set; }
            [J("id")] public int Id { get; set; }
            [J("input_id")] public int InputId { get; set; }
            [J("input_koef")] public int InputKoef { get; set; }
            [J("output_id")] public int OutputId { get; set; }
            [J("output_type")] public string OutputType { get; set; }
            [J("amount")] public int Amount { get; set; }
            [J("event_id")] public int EventId { get; set; }
        }
    }
}