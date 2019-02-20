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

    public class OutpostDefinitions
    {
        public class Outpost
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("controls")]
            public string Controls { get; set; }

            [J("crew")]
            public int Crew { get; set; }

            [J("def_id")]
            public int DefId { get; set; }

            [J("difficulty")]
            public int Difficulty { get; set; }

            [J("event_id")]
            public int EventId { get; set; }

            [J("flavour_loc")]
            public string FlavourLoc { get; set; }

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

            [J("points")]
            public string Points { get; set; }

            [J("req_level")]
            public int ReqLevel { get; set; }

            [J("required_locations", NullValueHandling = N.Ignore)]
            public RequiredLocations RequiredLocations { get; set; }

            [J("required_one", NullValueHandling = N.Ignore)]
            public RequiredLocations RequiredOne { get; set; }

            [J("round")]
            public int Round { get; set; }

            [J("sailors")]
            public int Sailors { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("title_loc")]
            public string TitleLoc { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("type")]
            public string Type { get; set; }

            [J("unlocked_fog")]
            public string UnlockedFog { get; set; }

            [J("unlocked_locations", NullValueHandling = N.Ignore)]
            public RequiredLocations UnlockedLocations { get; set; }

            [J("unlock_time")]
            public int UnlockTime { get; set; }

            [J("version_id")]
            public int VersionId { get; set; }

            [J("xp")]
            public int Xp { get; set; }

            [J("xp_pct")]
            public int XpPct { get; set; }
        }

        public class Outposts
        {
            [J("item")]
            public List<Outpost> Outpost { get; set; }
        }

        public class Location
        {
            [J("ids")]
            public string Ids { get; set; }

            [J("type")]
            public string Type { get; set; }
        }

        public class RequiredLocations
        {
            [J("location")]
            public List<Location> Location { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Outposts Outposts { get; set; }
        }
    }
}