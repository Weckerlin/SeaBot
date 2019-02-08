// // SeaBotCore
// // Copyright (C) 2018 - 2019 Weespin
// // 
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU General Public License for more details.
// // 
// // You should have received a copy of the GNU General Public License
// // along with this program.  If not, see <http://www.gnu.org/licenses/>.

#region

using J = Newtonsoft.Json.JsonPropertyAttribute;

#endregion

namespace SeaBotCore.Data.Definitions
{
    #region

    using System.Collections.Generic;

    #endregion

    public class UpgradeableDefenition
    {
        public class Item
        {
            [J("atlas")]
            public string Atlas { get; set; }

            [J("contractor_id")]
            public int ContractorId { get; set; }

            [J("controls")]
            public string Controls { get; set; }

            [J("def_id")]
            public int DefId { get; set; }

            [J("depletable")]
            public int Depletable { get; set; }

            [J("event_id")]
            public int EventId { get; set; }

            [J("gives_commodity")]
            public int GivesCommodity { get; set; }

            [J("global_contractor_id")]
            public int GlobalContractorId { get; set; }

            [J("iso_height")]
            public int IsoHeight { get; set; }

            [J("iso_width")]
            public int IsoWidth { get; set; }

            [J("levels")]
            public Levels Levels { get; set; }

            [J("map_x")]
            public int MapX { get; set; }

            [J("map_y")]
            public int MapY { get; set; }

            [J("material_id")]
            public int MaterialId { get; set; }

            [J("max_level")]
            public int MaxLevel { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("outpost_id")]
            public int OutpostId { get; set; }

            [J("points")]
            public string Points { get; set; }

            [J("refresh_time")]
            public int RefreshTime { get; set; }

            [J("slots")]
            public int Slots { get; set; }

            [J("speedupable")]
            public int Speedupable { get; set; }

            [J("texture")]
            public string Texture { get; set; }

            [J("unlock_time")]
            public int UnlockTime { get; set; }

            [J("version_id")]
            public int VersionId { get; set; }
        }

        public class Items
        {
            [J("item")]
            public List<Item> Item { get; set; }
        }

        public class Level
        {
            [J("amount")]
            public int Amount { get; set; }

            [J("amount_fid")]
            public int AmountFid { get; set; }

            [J("id")]
            public int Id { get; set; }

            [J("material_koef")]
            public int MaterialKoef { get; set; }

            [J("material_koef_fid")]
            public int MaterialKoefFid { get; set; }

            [J("sailors")]
            public int Sailors { get; set; }

            [J("sailors_fid")]
            public int SailorsFid { get; set; }

            [J("travel_time")]
            public int TravelTime { get; set; }

            [J("xp")]
            public int Xp { get; set; }

            [J("xp_pct")]
            public int XpPct { get; set; }
        }

        public class Levels
        {
            [J("level")]
            public List<Level> Level { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Items Items { get; set; }
        }
    }
}