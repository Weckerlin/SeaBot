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

    public class MuseumLevelDefenitions
    {
        public class Item
        {
            [J("def_id")]
            public int DefId { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("output_amount")]
            public int OutputAmount { get; set; }

            [J("turn_count")]
            public int TurnCount { get; set; }

            [J("turn_time")]
            public int TurnTime { get; set; }
        }

        public class Items
        {
            [J("item")]
            public List<Item> Item { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Items Items { get; set; }
        }
    }
}