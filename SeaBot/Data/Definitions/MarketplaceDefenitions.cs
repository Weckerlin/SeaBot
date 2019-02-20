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

    public class MarketplaceDefenitions
    {
        public class Marketplace
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("contractor_id")]
            public int ContractorId { get; set; }

            [J("controls")]
            public string Controls { get; set; }

            [J("def_id")]
            public int DefId { get; set; }

            [J("iso_height")]
            public int IsoHeight { get; set; }

            [J("iso_width")]
            public int IsoWidth { get; set; }

            [J("map_x")]
            public int MapX { get; set; }

            [J("map_y")]
            public int MapY { get; set; }

            [J("materials")]
            public Materials Materials { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("outpost_id")]
            public int OutpostId { get; set; }

            [J("points")]
            public string Points { get; set; }

            [J("sailors")]
            public int Sailors { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("texture")]
            public string Texture { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("version_id")]
            public int VersionId { get; set; }
        }

        public class Marketplaces
        {
            [J("item")]
            public List<Marketplace> Marketplace { get; set; }
        }

        public class Material
        {
            [J("amount")]
            public int Amount { get; set; }

            [J("contractor_id")]
            public int ContractorId { get; set; }

            [J("desc_loc")]
            public string DescLoc { get; set; }

            [J("event_id")]
            public int EventId { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("input_id")]
            public int InputId { get; set; }

            [J("input_koef")]
            public int InputKoef { get; set; }

            [J("output_id")]
            public int OutputId { get; set; }

            [J("output_type")]
            public string OutputType { get; set; }
        }

        public class Materials
        {
            [J("material")]
            public List<Material> Material { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Marketplaces Marketplaces { get; set; }
        }
    }
}