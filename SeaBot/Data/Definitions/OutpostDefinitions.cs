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
// aint with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public class OutpostDefinitions
    {
        public class Root : IDefinition
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
            [J("title_loc")] public string TitleLoc { get; set; }
            [J("flavour_loc")] public string FlavourLoc { get; set; }
            [J("type")] public string Type { get; set; }
            [J("speedupable")] public int Speedupable { get; set; }
            [J("sailors")] public int Sailors { get; set; }
            [J("slots")] public int Slots { get; set; }
            [J("req_level")] public int ReqLevel { get; set; }
            [J("travel_time")] public int TravelTime { get; set; }
            [J("unlock_time")] public int UnlockTime { get; set; }
            [J("unlocked_fog")] public string UnlockedFog { get; set; }
            [J("xp")] public int Xp { get; set; }
            [J("xp_pct")] public int XpPct { get; set; }
            [J("crew")] public int Crew { get; set; }
            [J("difficulty")] public int Difficulty { get; set; }
            [J("round")] public int Round { get; set; }
            [J("event_id")] public int EventId { get; set; }
            [J("map_x")] public int MapX { get; set; }
            [J("map_y")] public int MapY { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public int IsoWidth { get; set; }
            [J("iso_height")] public int IsoHeight { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }

            [J("required_locations", NullValueHandling = N.Ignore)]
            public RequiredLocations RequiredLocations { get; set; }

            [J("required_one", NullValueHandling = N.Ignore)]
            public RequiredLocations RequiredOne { get; set; }

            [J("unlocked_locations", NullValueHandling = N.Ignore)]
            public RequiredLocations UnlockedLocations { get; set; }
        }

        public class RequiredLocations
        {
            [J("location")] public List<Location> Location { get; set; }
        }

        public class Location
        {
            [J("type")] public string Type { get; set; }
            [J("ids")] public string Ids { get; set; }
        }
    }
}