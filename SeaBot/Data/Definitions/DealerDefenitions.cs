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

#endregion

namespace SeaBotCore.Data.Definitions
{
    #region

    using System.Collections.Generic;

    #endregion

    public static class DealerDefenitions
    {
        public class Dealer
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("contractor_id")]
            public int ContractorId { get; set; }

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

            [J("sailors")]
            public int Sailors { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("texture")]
            public string Texture { get; set; }

            [J("trades")]
            public Trades Trades { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("version_id")]
            public int VersionId { get; set; }
        }

        public class Items
        {
            [J("item")]
            public List<Dealer> Item { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Items Items { get; set; }
        }

        public class Trade
        {
            [J("id")]
            public int Id { get; set; }

            [J("input_amount")]
            public int InputAmount { get; set; }

            [J("input_id")]
            public int InputId { get; set; }

            [J("input_type")]
            public string InputType { get; set; }

            [J("output_amount")]
            public int OutputAmount { get; set; }

            [J("output_id")]
            public int OutputId { get; set; }

            [J("output_type")]
            public string OutputType { get; set; }
        }

        public class Trades
        {
            [J("trade")]
            public List<Trade> Trade { get; set; }
        }
    }
}